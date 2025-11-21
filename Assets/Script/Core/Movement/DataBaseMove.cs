using Script.Core.Entity;
using UnityEngine;

namespace Script.Core.Movement
{
    /// <summary>
    /// 移动需要的数据
    /// </summary>
    public struct DataBaseMove
    {
        /// <summary>
        /// 运动的方向
        /// </summary>
        public Vector3 MoveDirection;

        /// <summary>
        /// 移动要到达的目标（追随，跟踪）
        /// </summary>
        public BaseEntity MoveToTargetEntity;

        /// <summary>
        /// 当前运动的实例对象
        /// </summary>
        public IMoveController MoveController;
    }
}