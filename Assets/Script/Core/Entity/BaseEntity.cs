using System;
using System.Collections.Generic;
using Script.Core.Movement;
using UnityEngine;
using UnityEngine.Pool;

namespace Script.Core.Entity
{
    public abstract class BaseEntity : MonoBehaviour, IMoveController
    {
        /// <summary>
        /// 所属阵营
        /// </summary>
        public enTeamType TeamType;

        /// <summary>
        /// 各类运动
        /// </summary>
        protected readonly List<BaseMove> _baseMoves = new List<BaseMove>();

        private void Update()
        {
            F_MoveUpdate(Time.deltaTime);
        }

        public virtual void F_MoveUpdate(float deltaTime)
        {
            List<BaseMove> temp = ListPool<BaseMove>.Get();
            temp.AddRange(_baseMoves);
            foreach (var baseMove in temp)
            {
                baseMove.F_Update(deltaTime);
            }

            ListPool<BaseMove>.Release(temp);
        }

        /// <summary>
        /// 添加移动实例
        /// </summary>
        /// <param name="baseMove"></param>
        public void F_AddMove(BaseMove baseMove)
        {
            baseMove.F_SetData(new DataBaseMove()
            {
                Direction = baseMove.F_GetData().Direction,
                MoveController = this,
            });
            _baseMoves.Add(baseMove);
        }

        /// <summary>
        /// 移除指定的移动实例
        /// </summary>
        /// <param name="baseMove"></param>
        public void F_RemoveMove(BaseMove baseMove)
        {
            if (_baseMoves.Contains(baseMove))
            {
                _baseMoves.Remove(baseMove);
            }
            else
            {
                Debug.LogError($"要移除的运动实例已经不存在baseMove:{baseMove}");
            }
        }

        /// <summary>
        /// 设置当前实体的位置
        /// </summary>
        /// <param name="vector3"></param>
        public void F_SetCurrentPos(Vector3 vector3)
        {
            transform.position = vector3;
        }

        /// <summary>
        /// 获取当前实体的位置
        /// </summary>
        /// <returns></returns>
        public Vector3 F_GetCurrentPos()
        {
            return transform.position;
        }

        public abstract float F_GetMoveSpeed();
    }
}