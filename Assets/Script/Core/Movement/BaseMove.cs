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

        /// <summary>
        /// 设置当前运动的数据
        /// </summary>
        /// <param name="dataBaseMove"></param>
        public void F_SetData(DataBaseMove dataBaseMove)
        {
            _dataBaseMove = dataBaseMove;
        }

        public DataBaseMove F_GetData()
        {
            return _dataBaseMove;
        }

        public abstract void F_Update(float deltaTime);
    }
}