using UnityEngine;

namespace Script.Core.Entity
{
    public class Bullet : BaseEntity
    {
        private RectTransform _rectTransform;
        
        public override float F_GetMoveSpeed()
        {
            return 10f;
        }

        public void F_Clear()
        {
            for (int i = _baseMoves.Count - 1; i >= 0; i--)
            {
                F_RemoveMove(_baseMoves[i]);
            }
        }

        public RectTransform F_GetRectTransform()
        {
            if (_rectTransform == null)
            {
                _rectTransform = transform.GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }
}