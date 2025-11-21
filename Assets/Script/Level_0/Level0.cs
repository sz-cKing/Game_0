using System.Collections.Generic;
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

        private readonly float _CreateBulletTime = 0.5f;
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
            _timer.F_Init(60, uiLevel0.OnTime);
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
                });
                monster.TeamType = enTeamType.Enemy;
                monster.F_AddMove(followTarget);
                monster.F_SetCurrentPos(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 0));
                EntityManager.Instance.F_AddEntity(monster);
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
                //
                Move move = new Move(new DataBaseMove()
                {
                    //给子弹的飞行的方向
                    MoveDirection = GetBulletTargetDirection(),
                    MoveController = bullet,
                    MoveSpeed = bullet.F_GetMoveSpeed(),
                });
                bullet.TeamType = enTeamType.Self;
                bullet.F_AddMove(move);
                bullet.F_SetCurrentPos(EntityManager.Instance.F_GetMainHero().F_GetCurrentPos());
                EntityManager.Instance.F_AddEntity(bullet);
                _runBullets.Add(bullet);
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
                    bullet.F_Clear();
                    //
                    EntityManager.Instance.F_RemoveEntity(bullet);
                    _runBullets.Remove(bullet);
                    _releaseBulletStack.Push(bullet);
                }
            }
        }

        public void F_Clear()
        {
            EntityManager.Instance.F_Clear();
            _timer.F_Clear();
        }
    }
}