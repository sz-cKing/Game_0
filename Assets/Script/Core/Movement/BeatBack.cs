using Script.Core.Common;
using UnityEngine;

namespace Script.Core.Movement
{
    /// <summary>
    /// 击退
    /// </summary>
    public class BeatBack : BaseMove
    {
        private DataBeatBack _dataBeatBack;
        private float _runTime;
        private bool _canRun;


        public BeatBack(DataBeatBack dataBaseMove) : base(dataBaseMove)
        {
            _dataBeatBack = dataBaseMove;
            _runTime = 0;
            _canRun = true;
        }

        public override void F_Update(float deltaTime)
        {
            if (_canRun)
            {
                DataBaseMove dataBaseMove = F_GetData();
                Vector3 currentPos = dataBaseMove.MoveController.F_GetCurrentPos();
                currentPos += dataBaseMove.MoveDirection * dataBaseMove.MoveSpeed;
                dataBaseMove.MoveController.F_SetCurrentPos(currentPos);
                //
                _runTime += deltaTime;
                base.F_Update(deltaTime);
                if (_runTime >= _dataBeatBack.EffectTime)
                {
                    _canRun = false;
                    F_Clear();
                }
            }
        }

        public override void F_Clear()
        {
            _dataBeatBack = null;
            _canRun = false;
            base.F_Clear();
        }
    }
}