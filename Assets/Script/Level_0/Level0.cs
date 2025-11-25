using System.Collections.Generic;
using Script.Core.CheckCollider;
using Script.Core.Common;
using Script.Core.Entity;
using Script.Core.Movement;
using UnityEngine;

namespace Script.Level_0
{
    /// <summary>
    /// 场景0的管理器
    /// </summary>
    public class Level0 : MonoBehaviour
    {
        public UILevel0 uiLevel0;

        /// <summary>
        /// 分离参数
        /// </summary>
        public float separationThreshold = 1f;
        
        public float force = 5f;


        private readonly float _CreateBulletTime = 1.5f;
        private float _runTime;
        private List<Bullet> _runBullets;
        private Stack<Bullet> _releaseBulletStack;
        private Stack<Monster> _releaseMonsterStack;
        private float _runCreateMonsterTime;
        private Timer _timer;

        void Awake()
        {
            _runBullets = new List<Bullet>();
            _releaseBulletStack = new Stack<Bullet>();
            _releaseMonsterStack = new Stack<Monster>();
            //玩家操作的主控英雄
            MainHero mainHero = uiLevel0.v_HeroImage.gameObject.AddComponent<MainHero>();
            mainHero.TeamType = enTeamType.Self;
            mainHero.F_Init();
            EntityManager.Instance.F_AddEntity(mainHero);
            //倒计时
            _timer = new Timer();
            _timer.F_Init(60, (lastTime) =>
            {
                //UI层的倒讲时
                uiLevel0.OnTime(lastTime);
                //
                if (lastTime <= 0)
                {
                    OnGameFinish();
                }
            });
        }

        private void OnGameFinish()
        {
            Debug.LogError("游戏结束");
        }

        void Update()
        {
            UpdateCreateMonster(Time.deltaTime);
            UpdateCreateBullet(Time.deltaTime);
            UpdateCheckBulletRelease();
        }

        private void UpdateCreateMonster(float deltaTime)
        {
            _runCreateMonsterTime += deltaTime;
            float createMonsterInterval = 0.5f;
            while (_runCreateMonsterTime >= createMonsterInterval &&
                   EntityManager.Instance.F_GetEntityByType(enEntityType.Monster).Count < 4)
            {
                _runCreateMonsterTime -= createMonsterInterval;
                //
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
                monster.F_SetCurrentPos(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 0));
                EntityManager.Instance.F_AddEntity(monster);
            }
        }

        /// <summary>
        /// 怪物移动需要处理之间的间隔问题
        /// </summary>
        /// <param name="obj"></param>
        private void OnMonsterMove(BaseMove obj)
        {
            // 转换成当前的怪物
            if (obj.F_GetData().MoveController is not Monster monster)
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
                    MoveDirection = GetBulletTargetDirection(),
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
                                EffectTime = 0.5f,
                                MoveController = tempMonster,
                                MoveDirection = beatBackDirection,
                                MoveSpeed = 4,
                            });
                            tempMonster.F_AddMove(beatBack);
                            //子弹对象移除
                            Bullet bullet = baseMove.F_GetData().MoveController as Bullet;
                            ClearBullet(bullet);
                        }
                    }));
                }
            }
            else
            {
                Debug.LogError("为什么会存在传入的数据不是检测实例呢？");
            }
        }

        private Vector3 GetBulletTargetDirection()
        {
            Vector3 direction;
            List<BaseEntity> monsters = EntityManager.Instance.F_GetEntityByType(enEntityType.Monster);
            if (monsters.Count > 0)
            {
                int randomIndex = Random.Range(0, monsters.Count);
                direction = (monsters[randomIndex].F_GetCurrentPos() -
                             EntityManager.Instance.F_GetMainHero().F_GetCurrentPos()).normalized;
            }
            else
            {
                direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
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
                    ClearBullet(bullet);
                }
            }
        }

        private void ClearBullet(Bullet bullet)
        {
            bullet.F_Clear();
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
}