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

        /// <summary>
        /// 玩家操作的主控英雄
        /// </summary>
        private Hero _MainHero;

        private readonly float _CreateBulletTime = 0.5f;
        private float _runTime;
        private List<Bullet> _allBullet;
        private Stack<Bullet> _bulletStack;
        private Camera _camera;

        void Awake()
        {
            _camera = Camera.main;
            // _MainHero
            _allBullet = new List<Bullet>();
            _bulletStack = new Stack<Bullet>();
            _MainHero = uiLevel0.v_HeroImage.gameObject.AddComponent<Hero>();
        }

        void Update()
        {
            _runTime += Time.deltaTime;
            while (_runTime >= _CreateBulletTime)
            {
                _runTime -= _CreateBulletTime;
                //
                Bullet bullet = null;
                if (_bulletStack.Count > 0)
                {
                    bullet = _bulletStack.Pop();
                }
                else
                {
                    GameObject newBullet = Instantiate(uiLevel0.v_BulletImage.gameObject, uiLevel0.transform);
                    bullet = newBullet.AddComponent<Bullet>();
                }

                //
                Move move = new Move();
                move.F_SetData(new DataBaseMove()
                {
                    //给子弹的飞行的方向
                    Direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized
                });
                bullet.F_AddMove(move);
                bullet.F_SetCurrentPos(_MainHero.F_GetCurrentPos());
                _allBullet.Add(bullet);
            }

            //
            CheckBullet();
        }

        private void CheckBullet()添加
        {
            for (int i = _allBullet.Count - 1; i >= 0; i--)
            {
                Bullet bullet = _allBullet[i];
                if (CanvasOverlayCheck.F_IsUIElementOutOfScreen(bullet.F_GetRectTransform()))
                {
                    bullet.F_Clear();
                    //
                    _allBullet.Remove(bullet);
                    _bulletStack.Push(bullet);
                }
            }
        }
    }
}