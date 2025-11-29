using UnityEngine;

namespace Script.Core.Movement
{
    /// <summary>
    /// 直线方向移动
    /// </summary>
    public class DirectionMove : BaseMove
    {
        public DirectionMove(DataBaseMove dataBaseMove) : base(dataBaseMove)
        {
        }

        public override void F_Update(float deltaTime)
        {
            DataBaseMove dataBaseMove = F_GetData();
            Vector3 currentPos = dataBaseMove.MoveController.F_GetCurrentPos();
            currentPos += dataBaseMove.MoveDirection * dataBaseMove.MoveSpeed;
            dataBaseMove.MoveController.F_SetCurrentPos(currentPos);
            base.F_Update(deltaTime);
        }
    }
}