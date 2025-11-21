using Script.Core.Common;
using UnityEngine;

namespace Script.Core.Movement
{
    /// <summary>
    /// 跟踪目标移动
    /// </summary>
    public class FollowTarget : BaseMove
    {
        private DataFollowTarget _dataFollowTarget;

        public FollowTarget(DataFollowTarget dataBaseMove) : base(dataBaseMove)
        {
            _dataFollowTarget = dataBaseMove;
        }

        public override void F_Update(float deltaTime)
        {
            Vector3 direction = (_dataFollowTarget.MoveToTargetEntity.F_GetCurrentPos() -
                                 _dataFollowTarget.MoveController.F_GetCurrentPos()).normalized;
            Vector3 pos = _dataFollowTarget.MoveController.F_GetCurrentPos() +
                          direction * _dataFollowTarget.MoveSpeed;
            _dataFollowTarget.MoveController.F_SetCurrentPos(pos);
        }

        public override void F_Clear()
        {
            _dataFollowTarget = null;
            base.F_Clear();
        }
    }
}