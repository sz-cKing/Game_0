using UnityEngine;

namespace Script.Core.Common
{
   
    /// <summary>
    /// 视窗检测
    /// </summary>
    public static class ViewportCheck
    {
        public static bool F_IsWorldPosOutOfScreen(Vector3 worldPosition, Camera checkCamera)
        {
            // 将世界坐标转换为视口坐标
            Vector3 viewportPos = checkCamera.WorldToViewportPoint(worldPosition);

            // 判断是否在视口范围(0,0)到(1,1)之外
            bool isOut = viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1;
            // 如果需要，还可以考虑z坐标（物体在相机后方的情况）
            // isOut = isOut || viewportPos.z < 0;
            return isOut;
        }
    }
}