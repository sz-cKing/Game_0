using System;
using Script.Common;
using Script.Core.Common;
using TMPro;
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

        [SerializeField] private TextMeshProUGUI _Time;
        [SerializeField] private Transform _Hp;
        private Transform[] _allHp;

        private void Awake()
        {
            v_ExitBtn.onClick.AddListener(OnClickExitGame);
            _allHp = new Transform[_Hp.childCount];
            for (int i = 0; i < _Hp.childCount; i++)
            {
                _allHp[i] = _Hp.transform.GetChild(i);
            }
        }

        private void OnClickExitGame()
        {
            GameManager.Instance.F_LoadScene(enSceneType.Start);
        }

        /// <summary>
        /// 倒计时
        /// </summary>
        /// <param name="time"></param>
        public void F_OnTime(float time)
        {
            _Time.text = Timer.ConvertSecondsToMinutesSeconds(time);
        }

        public void F_SetHp(int currentHp)
        {
            for (int i = 0; i < _allHp.Length; i++)
            {
                _allHp[i].gameObject.SetActive(currentHp > i);
            }
        }
    }
}