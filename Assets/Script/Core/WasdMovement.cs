using System;
using UnityEngine;

namespace Script.Core
{
    /// <summary>
    /// 方向键的侦听处理
    /// </summary>
    public class WasdMovement : MonoBehaviour
    {
        /// <summary>
        /// 移动回调
        /// </summary>
        public Action<Vector3> MovementAction;

        void FixedUpdate()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            // 重置移动向量
            var direction = Vector3.zero;

            // WASD键侦听
            if (Input.GetKey(KeyCode.W))
                direction += transform.up;
            if (Input.GetKey(KeyCode.S))
                direction -= transform.up;
            if (Input.GetKey(KeyCode.A))
                direction -= transform.right;
            if (Input.GetKey(KeyCode.D))
                direction += transform.right;

            // 标准化移动向量，防止斜向移动更快
            if (direction.magnitude > 1f)
                direction.Normalize();

            //回调事件
            MovementAction?.Invoke(direction);
        }
    }
}