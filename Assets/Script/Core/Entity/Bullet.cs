using System;
using Script.Core.CheckCollider;
using UnityEngine;

namespace Script.Core.Entity
{
    public class Bullet : BaseEntity
    {
        private RectTransform _rectTransform;

        public override float F_GetMoveSpeed()
        {
            return 10f;
        }

        public override enEntityType F_GetEntityType()
        {
            return enEntityType.Bullet;
        }

        public override void F_CheckOtherCollider(IColliderInstance otherCollider, Action<CheckResult> callback)
        {
            base.F_CheckOtherCollider(otherCollider, callback);
        }

        /// <summary>
        /// 清除所有的移动方式
        /// </summary>
        public void F_ClearAllMove()
        {
            for (int i = v_BaseMoves.Count - 1; i >= 0; i--)
            {
                v_BaseMoves[i].F_Clear();
                F_RemoveMove(v_BaseMoves[i]);
            }
        }

        public RectTransform F_GetRectTransform()
        {
            if (_rectTransform == null)
            {
                _rectTransform = transform.GetComponent<RectTransform>();
            }

            return _rectTransform;
        }
    }
}