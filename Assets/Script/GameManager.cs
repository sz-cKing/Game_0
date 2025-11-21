using System;
using Script.Common;
using Script.Core;
using Script.Core.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script
{
    /// <summary>
    /// 游戏管理器
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        private enSceneType _currentScene = enSceneType.Start;

        /// <summary>
        /// 加载指定的场景
        /// </summary>
        /// <param name="enSceneType"></param>
        public void LoadScene(enSceneType enSceneType)
        {
            _currentScene = enSceneType;
            SceneManager.LoadScene(enSceneType.ToString());
            Debug.Log($"加载好指定的场景:{enSceneType.ToString()}");
        }

        /// <summary>
        /// 加载下一次关卡
        /// </summary>
        public void LoadNextScene()
        {
            int nextLevel = (int)_currentScene + 1;
            int total = (int)enSceneType.All;
            nextLevel = nextLevel >= total ? 0 : nextLevel;
            LoadScene((enSceneType)nextLevel);
        }
    }
}