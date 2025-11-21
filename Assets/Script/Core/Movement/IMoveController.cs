

using UnityEngine;

namespace Script.Core.Movement
{
    /// <summary>
    /// 移动实例的控制器
    /// </summary>
    public interface IMoveController
    {
        /// <summary>
        /// 更新当前的位置
        /// </summary>
        /// <param name="vector3"></param>
        void F_SetCurrentPos(Vector3 vector3);

        /// <summary>
        /// 获取当前的位置
        /// </summary>
        /// <returns></returns>
        Vector3 F_GetCurrentPos();

        /// <summary>
        /// 获取当前实体的移动速度
        /// </summary>
        /// <returns></returns>
        float F_GetMoveSpeed();
    }
}