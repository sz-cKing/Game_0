using System;
using Script.Common;
using Script.Core.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Level_0
{
    /// <summary>
    /// 场景0的界面管理器
    /// </summary>
    public class UILevel0 : MonoBehaviour
    {
        public Button v_ExitBtn;
        public Image v_BulletImage;
        public Image v_MonsterImage;
        /// <summary>
        /// 主控英雄的实例
        /// </summary>
        public Image v_HeroImage;

        private void Awake()
        {
            v_ExitBtn.onClick.AddListener(OnClickExitGame);
        }

        private void OnClickExitGame()
        {
            GameManager.Instance.LoadScene(enSceneType.Start);
        }
    }
}