using UnityEngine;

namespace Script.Core.Movement
{
    /// <summary>
    /// 正常的移动
    /// </summary>
    public class Move : BaseMove
    {
        public Move(DataBaseMove dataBaseMove) : base(dataBaseMove)
        {
        }

        public override void F_Update(float deltaTime)
        {
            DataBaseMove dataBaseMove = F_GetData();
            Vector3 currentPos = dataBaseMove.MoveController.F_GetCurrentPos();
            currentPos += dataBaseMove.MoveDirection * dataBaseMove.MoveSpeed;
            dataBaseMove.MoveController.F_SetCurrentPos(currentPos);
        }
    }
}