using Script.Core.Common;

namespace Script.Core.Movement
{
    /// <summary>
    /// 移动的基类
    /// </summary>
    public abstract class BaseMove : IUpdate
    {
        /// <summary>
        /// 当前的运动数据
        /// </summary>
        private DataBaseMove _dataBaseMove;

        public BaseMove(DataBaseMove dataBaseMove)
        {
            _dataBaseMove = dataBaseMove;
            UpdateManager.Instance.F_AddUpdate(this);
        }

        public DataBaseMove F_GetData()
        {
            return _dataBaseMove;
        }

        public void F_SetMoveController(IMoveController moveController)
        {
            _dataBaseMove.MoveController = moveController;
        }

        public abstract void F_Update(float deltaTime);

        public virtual void F_Clear()
        {
            if (_dataBaseMove != null)
            {
                _dataBaseMove = null;
                UpdateManager.Instance.F_RemoveUpdate(this);
            }
        }
    }
}