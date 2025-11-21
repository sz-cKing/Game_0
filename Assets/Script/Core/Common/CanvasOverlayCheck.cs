using UnityEngine;

namespace Script.Core.Common
{
    public static class CanvasOverlayCheck
    {
        public static bool F_IsUIElementOutOfScreen(RectTransform rectTransform)
        {
            // 获取UI元素的四个角点的屏幕坐标
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            // 检查所有角点是否都在屏幕外
            bool allCornersOut = true;
            foreach (Vector3 corner in corners)
            {
                if (IsScreenPointInViewport(corner))
                {
                    allCornersOut = false;
                    break;
                }
            }

            return allCornersOut;
        }

        public static bool F_IsUIElementPartiallyOutOfScreen(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            // 检查是否有任意角点在屏幕外
            foreach (Vector3 corner in corners)
            {
                if (!IsScreenPointInViewport(corner))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsScreenPointInViewport(Vector3 screenPoint)
        {
            // 对于Overlay模式，坐标已经是屏幕坐标
            return screenPoint.x >= 0 && screenPoint.x <= Screen.width &&
                   screenPoint.y >= 0 && screenPoint.y <= Screen.height;
        }
    }
}