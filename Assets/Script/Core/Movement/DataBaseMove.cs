using Script.Core.Entity;
using UnityEngine;

namespace Script.Core.Movement
{
    /// <summary>
    /// 移动需要的数据
    /// </summary>
    public class DataBaseMove
    {
        /// <summary>
        /// 运动的方向
        /// </summary>
        public Vector3 MoveDirection;

        /// <summary>
        /// 速度
        /// </summary>
        public float MoveSpeed;

        /// <summary>
        /// 当前运动的实例对象
        /// </summary>
        public IMoveController MoveController;
    }
}