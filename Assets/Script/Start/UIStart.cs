using System;
using Script.Core.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Common
{
    public class UIStart : MonoBehaviour
    {
        public Button v_StartBtn;

        private void Awake()
        {
            v_StartBtn.onClick.AddListener(OnClickStart);
            //初始化
            GameManager.Instance.F_Init();
        }

        private void OnClickStart()
        {
            GameManager.Instance.LoadScene(enSceneType.Level_0);
        }
    }
}

