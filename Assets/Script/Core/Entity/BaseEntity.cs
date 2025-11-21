using System.Collections.Generic;
using Script.Core.Movement;
using UnityEngine;

namespace Script.Core.Entity
{
    public abstract class BaseEntity : MonoBehaviour, IMoveController
    {
        /// <summary>
        /// 所属阵营
        /// </summary>
        public enTeamType TeamType { get; set; }

        /// <summary>
        /// 身上挂载的运动行为
        /// </summary>
        public readonly List<BaseMove> v_BaseMoves = new List<BaseMove>();

        /// <summary>
        /// 添加移动实例
        /// </summary>
        /// <param name="baseMove"></param>
        public void F_AddMove(BaseMove baseMove)
        {
            if (v_BaseMoves.Contains(baseMove))
            {
                Debug.LogError($"要加载的运动实例已经存在里列表里了，baseMove:{baseMove}");
            }
            else
            {
                v_BaseMoves.Add(baseMove);
            }
        }

        /// <summary>
        /// 移除指定的移动实例
        /// </summary>
        /// <param name="baseMove"></param>
        public void F_RemoveMove(BaseMove baseMove)
        {
            if (v_BaseMoves.Contains(baseMove))
            {
                v_BaseMoves.Remove(baseMove);
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

        public abstract enEntityType F_GetEntityType();
    }
}