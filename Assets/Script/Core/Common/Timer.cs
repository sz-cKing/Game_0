using System;

namespace Script.Core.Common
{
    public class Timer : IUpdate
    {
        /// <summary>
        /// 需要计时的总时长多少秒
        /// </summary>
        private float _totalTime;

        private float _runTime;
        private bool _canRun;
        private Action<float> _timerCallback;

        /// <summary>
        /// 转换成：00：00
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string ConvertSecondsToMinutesSeconds(float seconds)
        {
            // 计算分钟和秒数
            int minutes = (int)(seconds / 60);
            int remainingSeconds = (int)(seconds % 60);

            // 格式化为两位数字，不足补零
            return $"{minutes:D2}:{remainingSeconds:D2}";
        }

        /// <summary>
        /// 倒计时多少秒
        /// </summary>
        /// <param name="totalTime"></param>
        /// <param name="timerAction">计时回调</param>
        public void F_Init(float totalTime, Action<float> timerAction)
        {
            _totalTime = totalTime;
            _canRun = true;
            _timerCallback = timerAction;
            UpdateManager.Instance.F_AddUpdate(this);
        }


        public void F_Update(float deltaTime)
        {
            if (_canRun)
            {
                _runTime += deltaTime;
                float last = _totalTime - _runTime;
                //回调当前还有多少秒
                _timerCallback?.Invoke(last);
                if (last <= 0)
                {
                    _canRun = false;
                    UpdateManager.Instance.F_RemoveUpdate(this);
                }
            }
        }

        public void F_Clear()
        {
            if (_canRun)
            {
                _canRun = false;
                UpdateManager.Instance.F_RemoveUpdate(this);
            }
        }
    }
}