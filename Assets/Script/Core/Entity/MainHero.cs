using UnityEngine;

namespace Script.Core.Entity
{
    /// <summary>
    /// 主控英雄
    /// </summary>
    public class MainHero : Hero
    {
        public override float F_GetMoveSpeed()
        {
            return 5;
        }

        public void F_Init()
        {
            WasdMovement wasdMovement = gameObject.GetComponent<WasdMovement>();
            if (wasdMovement == null)
            {
                wasdMovement = gameObject.AddComponent<WasdMovement>();
            }

            wasdMovement.MovementAction = OnMovement;
        }

        private void OnMovement(Vector3 direction)
        {
            // 使用Transform移动
            transform.Translate(direction * F_GetMoveSpeed(), Space.World);
        }
    }
}