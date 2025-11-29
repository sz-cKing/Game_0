using System;
using System.Collections.Generic;
using Script.CommonUI;
using Script.Core;
using Script.Core.CheckCollider;
using Script.Core.Common;
using Script.Core.Entity;
using Script.Core.Movement;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

namespace Script.Level_0
{
    /// <summary>
    /// 场景0的管理器
    /// </summary>
    public class Level0 : MonoBehaviour,IUpdate
    {
        public UILevel0 uiLevel0;
        public UISettlement uiSettlement;

        /// <summary>
        /// 分离参数
        /// </summary>
        public float separationThreshold = 1f;

        public float force = 5f;

        private int _currentMainHeroHp;
        private readonly float _CreateBulletTime = 1.5f;
        private float _runTime;
        private List<Bullet> _runBullets;
        private Stack<Bullet> _releaseBulletStack;
        private Stack<Monster> _releaseMonsterStack;
        private float _runCreateMonsterTime;
        private Dictionary<float, CreateMonsterInfo> _CreateMonserDic;
        private Timer _timer;
        private int _canCreateBulletTotal;
        private Dictionary<Monster, float> _colliderMonsterDic;


        void Awake()
        {
            _runBullets = new List<Bullet>();
            _releaseBulletStack = new Stack<Bullet>();
            _releaseMonsterStack = new Stack<Monster>();
            _colliderMonsterDic = new Dictionary<Monster, float>();
            //玩家操作的主控英雄
            MainHero mainHero = uiLevel0.v_HeroImage.gameObject.AddComponent<MainHero>();
            mainHero.TeamType = enTeamType.Self;
            mainHero.F_Init();
            EntityManager.Instance.F_AddEntity(mainHero);
            //倒计时
            _timer = new Timer();
            _CreateMonserDic = new Dictionary<float, CreateMonsterInfo>();
            _CreateMonserDic.Add(59, new CreateMonsterInfo() { CreateTotal = 6 });
            _CreateMonserDic.Add(40, new CreateMonsterInfo() { CreateTotal = 8 });
            _CreateMonserDic.Add(30, new CreateMonsterInfo() { CreateTotal = 10 });
            _CreateMonserDic.Add(15, new CreateMonsterInfo() { CreateTotal = 15 });
            //
            UpdateManager.Instance.F_AddUpdate(this);
            OnStartGame();
        }

        private void OnDestroy()
        {
            UpdateManager.Instance.F_RemoveUpdate(this);
        }

        private void OnStartGame()
        {
            UpdateManager.Instance.F_SetState(false);
            _currentMainHeroHp = 5;
            _timer.F_Init(60, (lastTime) =>
            {
                //UI层的倒讲时
                uiLevel0.F_OnTime(lastTime);
                //倒计到指定的点就创建怪物
                int lastTimeInt = Mathf.RoundToInt(lastTime);
                if (_CreateMonserDic.ContainsKey(lastTimeInt) && _CreateMonserDic[lastTimeInt].IsHaveCreate == false)
                {
                    for (int i = 0; i < _CreateMonserDic[lastTimeInt].CreateTotal; i++)
                    {
                        _CreateMonserDic[lastTimeInt].IsHaveCreate = true;
                        CreateCreateMonster();
                        SetBulletTotal(lastTimeInt);
                    }
                }

                //
                if (lastTime <= 0)
                {
                    OnGameFinish();
                }
            });
        }

        private void SetBulletTotal(int lastTimeInt)
        {
            switch (lastTimeInt)
            {
                case 59:
                    _canCreateBulletTotal = 3;
                    break;
                case 40:
                    _canCreateBulletTotal = 5;
                    break;
                case 30:
                    _canCreateBulletTotal = 6;
                    break;
                case 15:
                    _canCreateBulletTotal = 8;
                    break;
            }
        }

        private void OnGameFinish()
        {
            Debug.LogError("游戏结束");
            if (_currentMainHeroHp > 0)
            {
                //挑战成功进入下一关卡
                uiSettlement.F_SetData(new DataUISettlement()
                    { Result = enResult.挑战成功, OnClickRestartGame = OnStartGame });
            }
        }
        
        public void F_Update(float deltaTime)
        {
            UpdateCreateBullet(Time.deltaTime);
            UpdateCheckBulletRelease();
        }
        
        
        private void CreateCreateMonster()
        {
            Monster monster = null;
            if (_releaseMonsterStack.Count > 0)
            {
                monster = _releaseMonsterStack.Pop();
            }
            else
            {
                GameObject newBullet = Instantiate(uiLevel0.v_MonsterImage.gameObject, uiLevel0.transform);
                monster = newBullet.AddComponent<Monster>();
            }

            //追踪目标移动
            FollowTarget followTarget = new FollowTarget(new DataFollowTarget()
            {
                //追踪的目标
                MoveToTargetEntity = EntityManager.Instance.F_GetMainHero(),
                MoveSpeed = monster.F_GetMoveSpeed(),
                MoveController = monster,
                UpdateMoveCallback = OnMonsterMove,
            });
            monster.TeamType = enTeamType.Enemy;
            monster.F_AddMove(followTarget);
            //从屏幕外随机一个坐标
            monster.F_SetCurrentPos(GetRandomOutOfScreenPoint());
            EntityManager.Instance.F_AddEntity(monster);
        }

        private static Vector2 GetRandomOutOfScreenPoint(float offset = 100f)
        {
            float screenW = Screen.width;
            float screenH = Screen.height;

            int side = Random.Range(0, 4); // 0=上 1=下 2=左 3=右
            float x = 0, y = 0;

            switch (side)
            {
                case 0: // 上
                    x = Random.Range(-offset, screenW + offset);
                    y = screenH + offset;
                    break;
                case 1: // 下
                    x = Random.Range(-offset, screenW + offset);
                    y = -offset;
                    break;
                case 2: // 左
                    x = -offset;
                    y = Random.Range(-offset, screenH + offset);
                    break;
                case 3: // 右
                    x = screenW + offset;
                    y = Random.Range(-offset, screenH + offset);
                    break;
            }

            return new Vector2(x, y);
        }


        /// <summary>
        /// 怪物移动需要处理之间的间隔问题
        /// </summary>
        /// <param name="baseMove"></param>
        private void OnMonsterMove(BaseMove baseMove)
        {
            OnHandleMoveSeparation(baseMove);
            OnCheckMonsterCollider(baseMove);
        }

        /// <summary>
        /// 检测怪物碰撞
        /// </summary>
        /// <param name="baseMove"></param>
        private void OnCheckMonsterCollider(BaseMove baseMove)
        {
            //这个是怪物,获取它的测试碰撞
            if (baseMove.F_GetData().MoveController is ICheckColliderInstance bulletCheckColliderInstance)
            {
                //
                MainHero mainHero = (MainHero)EntityManager.Instance.F_GetMainHero();

                //检测子弹是否碰撞到这个怪物
                bulletCheckColliderInstance.F_CheckOtherCollider(mainHero, (result =>
                {
                    if (result.IsCollider)
                    {
                        MonsterColliderMainHero((Monster)baseMove.F_GetData().MoveController);
                        // Debug.LogError("碰撞到了，主角");
                    }
                }));
            }
            else
            {
                Debug.LogError("为什么会存在传入的数据不是检测实例呢？");
            }
        }

        /// <summary>
        /// 怪物碰撞到了主控角色
        /// </summary>
        /// <param name="monster"></param>
        private void MonsterColliderMainHero(Monster monster)
        {
            if (monster != null)
            {
                if (_colliderMonsterDic.ContainsKey(monster))
                {
                    if (_colliderMonsterDic[monster] - Time.time >= 0.5f) //之前生效过，需要CD-0.5f秒后再认为碰撞生效一次
                    {
                        //
                        SetMainHeroHurt();
                        _colliderMonsterDic[monster] = Time.time;
                    }
                }
                else
                {
                    //
                    SetMainHeroHurt();
                    _colliderMonsterDic.Add(monster, Time.time);
                }
            }
        }

        private void SetMainHeroHurt()
        {
            _currentMainHeroHp--;
            uiLevel0.F_SetHp(_currentMainHeroHp);
            Debug.LogError($"当前玩家的血量:{_currentMainHeroHp}");
            if (_currentMainHeroHp <= 0)
            {
                uiSettlement.F_SetData(new DataUISettlement()
                    { Result = enResult.挑战失败, OnClickRestartGame = OnStartGame });
                Debug.LogError("血量为0.挑战失败，");
            }
        }

        /// <summary>
        /// 处理移动分离
        /// </summary>
        /// <param name="baseMove"></param>
        private void OnHandleMoveSeparation(BaseMove baseMove)
        {
            // 转换成当前的怪物
            if (baseMove.F_GetData().MoveController is not Monster monster)
            {
                return;
            }

            float separationThresholdSqr = separationThreshold * separationThreshold;

            // 获取怪物列表
            var monsters = EntityManager.Instance.F_GetEntityByType(enEntityType.Monster);
            int count = monsters.Count;

            if (count < 2) return;

            Vector3 selfPos = monster.F_GetCurrentPos();
            Vector3 separationVector = Vector3.zero;
            int neighborCount = 0;

            foreach (var other in monsters)
            {
                // 排除自己
                if (other.gameObject == monster.gameObject)
                    continue;

                Vector3 diff = selfPos - other.transform.position;
                float sqrDist = diff.sqrMagnitude;

                // 使用平方距离，避免频繁开方
                if (sqrDist > 0f && sqrDist < separationThresholdSqr)
                {
                    // 距离越近，权重越大
                    float invDist = 1f / Mathf.Sqrt(sqrDist);
                    separationVector += diff * invDist;
                    neighborCount++;
                }
            }

            // 应用分离力
            if (neighborCount > 0)
            {
                separationVector /= neighborCount;
                Vector3 newPos = selfPos + separationVector * force;
                monster.F_SetCurrentPos(newPos);
            }
        }

        private void UpdateCreateBullet(float deltaTime)
        {
            _runTime += deltaTime;
            while (_runTime >= _CreateBulletTime)
            {
                _runTime -= _CreateBulletTime;
                //最多创建3个子弹自己
                int bulletTotal = EntityManager.Instance.F_GetEntityByType(enEntityType.Bullet).Count;
                if (bulletTotal < _canCreateBulletTotal)
                {
                    CreateBullet();
                }
            }
        }

        private void CreateBullet()
        {
            //
            Bullet bullet = null;
            if (_releaseBulletStack.Count > 0)
            {
                bullet = _releaseBulletStack.Pop();
            }
            else
            {
                GameObject newBullet = Instantiate(uiLevel0.v_BulletImage.gameObject, uiLevel0.transform);
                bullet = newBullet.AddComponent<Bullet>();
            }

            //给子弹添加直接飞行
            DirectionMove directionMove = new DirectionMove(new DataBaseMove()
            {
                //给子弹的飞行的方向
                MoveDirection = GetBulletTargetDirection(bullet),
                MoveController = bullet,
                MoveSpeed = bullet.F_GetMoveSpeed(),
                //检测飞行中是否碰撞
                UpdateMoveCallback = OnCheckBulletCollider,
            });
            bullet.TeamType = enTeamType.Self;
            bullet.F_AddMove(directionMove);
            //设置子弹的初始坐标
            bullet.F_SetCurrentPos(EntityManager.Instance.F_GetMainHero().F_GetCurrentPos());
            bullet.gameObject.SetActive(true);
            EntityManager.Instance.F_AddEntity(bullet);
            _runBullets.Add(bullet);
        }

        /// <summary>
        /// 检测已经碰撞
        /// </summary>
        /// <param name="baseMove"></param>
        private void OnCheckBulletCollider(BaseMove baseMove)
        {
            //这个是子弹,获取它的测试碰撞
            if (baseMove.F_GetData().MoveController is ICheckColliderInstance bulletCheckColliderInstance)
            {
                //
                List<BaseEntity> monsters = EntityManager.Instance.F_GetEntityByType(enEntityType.Monster);
                foreach (var tempMonster in monsters)
                {
                    //检测子弹是否碰撞到这个怪物
                    bulletCheckColliderInstance.F_CheckOtherCollider(tempMonster, (result =>
                    {
                        if (result.IsCollider)
                        {
                            // Debug.LogError($"碰撞到对象：{result.Other.Entity.name}");
                            Vector3 beatBackDirection = (tempMonster.F_GetCurrentPos() -
                                                         baseMove.F_GetData().MoveController.F_GetCurrentPos())
                                .normalized;
                            //击飞效果
                            BeatBack beatBack = new BeatBack(new DataBeatBack()
                            {
                                EffectTime = 0.25f,
                                MoveController = tempMonster,
                                MoveDirection = beatBackDirection,
                                MoveSpeed = 2.5f,
                            });
                            tempMonster.F_AddMove(beatBack);

                            Bullet bullet = baseMove.F_GetData().MoveController as Bullet;
                            // 更改当前子弹的移动朝向
                            if (bullet != null)
                            {
                                bullet.F_ClearAllMove();
                                bullet.F_AddMove((new DirectionMove(new DataBaseMove()
                                {
                                    MoveController = bullet, MoveDirection = -beatBackDirection,
                                    MoveSpeed = bullet.F_GetMoveSpeed(),
                                    //检测飞行中是否碰撞
                                    UpdateMoveCallback = OnCheckBulletCollider,
                                })));
                            }
                            else
                            {
                                Debug.LogError("为什么这个不是子弹呢？");
                            }

                            // 子弹对象移除
                            // ClearBullet(bullet);
                        }
                    }));
                }
            }
            else
            {
                Debug.LogError("为什么会存在传入的数据不是检测实例呢？");
            }
        }

        private Vector3 GetBulletTargetDirection(Bullet bullet)
        {
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
            List<BaseEntity> monsters = EntityManager.Instance.F_GetEntityByType(enEntityType.Monster);
            if (monsters.Count > 0)
            {
                float minDistance = float.MaxValue;
                foreach (var tempMonster in monsters)
                {
                    var tempDirection = tempMonster.F_GetCurrentPos() - bullet.F_GetCurrentPos();
                    float distance = tempDirection.sqrMagnitude;
                    if (distance < minDistance)
                    {
                        direction = tempDirection.normalized;
                    }
                }
            }

            return direction;
        }

        private void UpdateCheckBulletRelease()
        {
            for (int i = _runBullets.Count - 1; i >= 0; i--)
            {
                Bullet bullet = _runBullets[i];
                if (CanvasOverlayCheck.F_IsUIElementOutOfScreen(bullet.F_GetRectTransform()))
                {
                    // ClearBullet(bullet);

                    //给子弹添加直接飞行-反弹
                    DirectionMove directionMove = new DirectionMove(new DataBaseMove()
                    {
                        //给子弹的飞行的方向
                        MoveDirection =
                            (EntityManager.Instance.F_GetMainHero().F_GetCurrentPos() - bullet.F_GetCurrentPos())
                            .normalized,
                        MoveController = bullet,
                        MoveSpeed = bullet.F_GetMoveSpeed(),
                        //检测飞行中是否碰撞
                        UpdateMoveCallback = OnCheckBulletCollider,
                    });
                    bullet.F_ClearAllMove();
                    bullet.F_AddMove(directionMove);
                }
            }
        }

        private void ClearBullet(Bullet bullet)
        {
            bullet.F_ClearAllMove();
            bullet.gameObject.SetActive(false);
            //
            EntityManager.Instance.F_RemoveEntity(bullet);
            _runBullets.Remove(bullet);
            _releaseBulletStack.Push(bullet);
        }

        public void F_Clear()
        {
            EntityManager.Instance.F_Clear();
            _timer.F_Clear();
        }

    }

    public class CreateMonsterInfo
    {
        public bool IsHaveCreate;

        public int CreateTotal;
    }
}