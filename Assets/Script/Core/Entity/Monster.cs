using System;
using Script.Core.CheckCollider;

namespace Script.Core.Entity
{
    public class Monster : BaseEntity
    {
        public override float F_GetMoveSpeed()
        {
            return 1;
        }

        public override enEntityType F_GetEntityType()
        {
            return enEntityType.Monster;
        }

        public override void F_CheckOtherCollider(IColliderInstance otherCollider, Action<CheckResult> callback)
        {
            base.F_CheckOtherCollider(otherCollider, callback);
        }
    }
}