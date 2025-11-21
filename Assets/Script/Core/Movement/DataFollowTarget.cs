using Script.Core.Entity;

namespace Script.Core.Movement
{
    public class DataFollowTarget : DataBaseMove
    {
        /// <summary>
        /// 移动要到达的目标（追随，跟踪）
        /// </summary>
        public BaseEntity MoveToTargetEntity;
    }
}