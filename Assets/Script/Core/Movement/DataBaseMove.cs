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
        public Vector3 Direction;

        /// <summary>
        /// 当前运动的实例对象
        /// </summary>
        public IMoveController MoveController;
    }
}