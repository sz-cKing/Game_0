using UnityEngine;

namespace Script.Core
{
    /// <summary>
    /// 方向键的侦听处理
    /// </summary>
    public class WasdMovement : MonoBehaviour
    {
        [Header("移动设置")]
        public float moveSpeed = 5f;
        public float rotationSpeed = 180f;
    
        private Rigidbody _rb;
        private Vector3 _movement;
    
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }
    
        void Update()
        {
            HandleInput();
        }
    
        void FixedUpdate()
        {
            HandleMovement();
        }
    
        private void HandleInput()
        {
            // 重置移动向量
            _movement = Vector3.zero;
        
            // WASD键侦听
            if (Input.GetKey(KeyCode.W))
                _movement += transform.up;
            if (Input.GetKey(KeyCode.S))
                _movement -= transform.up;
            if (Input.GetKey(KeyCode.A))
                _movement -= transform.right;
            if (Input.GetKey(KeyCode.D))
                _movement += transform.right;
            
            // 标准化移动向量，防止斜向移动更快
            if (_movement.magnitude > 1f)
                _movement.Normalize();
        }
    
        private void HandleMovement()
        {
            if (_rb != null)
            {
                // 使用物理移动
                Vector3 targetVelocity = _movement * moveSpeed;
                _rb.velocity = new Vector3(targetVelocity.x, _rb.velocity.y, targetVelocity.z);
            }
            else
            {
                // 使用Transform移动
                transform.Translate(_movement * moveSpeed * Time.deltaTime, Space.World);
            }
        }
    }
}