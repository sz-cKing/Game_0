using Script.Core.Entity;
using UnityEngine;

namespace Script.Core.Movement
{
    /// <summary>
    /// 跟踪目标移动
    /// </summary>
    public class FollowTargetMove : BaseMove
    {
        public override void F_Update(float deltaTime)
        {
            DataBaseMove dataBaseMove = F_GetData();
            Vector3 direction = (dataBaseMove.MoveToTargetEntity.F_GetCurrentPos() -
                                 dataBaseMove.MoveController.F_GetCurrentPos()).normalized;
            Vector3 pos = dataBaseMove.MoveController.F_GetCurrentPos() +
                          direction * dataBaseMove.MoveController.F_GetMoveSpeed();
            dataBaseMove.MoveController.F_SetCurrentPos(pos);
        }
    }
}