namespace Script.Core.CheckCollider
{
    public struct CheckResult
    {
        public bool IsCollider;

        /// <summary>
        /// 检测者
        /// </summary>
        public IColliderInstance Checker;

        /// <summary>
        /// 被碰撞者
        /// </summary>
        public IColliderInstance Other;
    }
}