using UnityEngine;

namespace Script.Core.Movement
{
    /// <summary>
    /// 正常的移动
    /// </summary>
    public class Move : BaseMove
    {
        public override void F_Update(float deltaTime)
        {
            DataBaseMove dataBaseMove = F_GetData();
            Vector3 currentPos = dataBaseMove.MoveController.F_GetCurrentPos();
            currentPos += dataBaseMove.MoveDirection * dataBaseMove.MoveController.F_GetMoveSpeed();
            dataBaseMove.MoveController.F_SetCurrentPos(currentPos);
        }
    }
}