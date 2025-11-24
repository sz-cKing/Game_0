using Script.Core.Entity;
using UnityEngine;

namespace Script.Core.CheckCollider
{
    /// <summary>
    /// 碰撞实例
    /// </summary>
    public interface IColliderInstance
    {
        /// <summary>
        /// 碰撞体
        /// </summary>
        public Collider2D Collider2D { get; }
        
        /// <summary>
        /// 实体对象
        /// </summary>
        public BaseEntity Entity { get; }
    }
}