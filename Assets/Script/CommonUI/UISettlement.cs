using System;
using Script.Core.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.CommonUI
{
    /// <summary>
    /// 结算界面 
    /// </summary>
    public class UISettlement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _Title;
        [SerializeField] private Button _ReStartGame;

        [SerializeField] private Button _ExitGameBtn;
    
        [SerializeField] private Button _NextLevelBtn;

        private DataUISettlement _dataUISettlement;
    

        void Start()
        {
            _ReStartGame.onClick.AddListener(OnClickReStartGame);
            _ExitGameBtn.onClick.AddListener(OnClickExitGame);
            _NextLevelBtn.onClick.AddListener(OnClickNextLevel);
        }

        private void OnClickNextLevel()
        {
            gameObject.SetActive(false);
            GameManager.Instance.F_LoadNextScene();
           
        }

        private void OnClickExitGame()
        {
            gameObject.SetActive(false);
            GameManager.Instance.F_LoadScene(enSceneType.Start);
        }

        private void OnClickReStartGame()
        {
            gameObject.SetActive(false);
            _dataUISettlement.OnClickRestartGame?.Invoke();
        }

        public void F_SetData(DataUISettlement dataUISettlement)
        {
            gameObject.SetActive(true);
            _dataUISettlement = dataUISettlement;
            switch (dataUISettlement.Result)
            {
                case enResult.挑战成功:
                    _Title.text = "挑战成功";
                    break;
                case enResult.挑战失败:
                    _Title.text = "挑战失败";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}