using System;
using UnityEngine;

namespace Script.Core.CheckCollider
{
    /// <summary>
    /// 抽象碰撞检测
    /// </summary>
    public interface ICheckColliderInstance
    {
        void F_CheckOtherCollider(IColliderInstance otherCollider, Action<CheckResult> callback);
    }
}