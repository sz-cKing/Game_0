using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Script.Core.Common
{
    public class UpdateManager : Singleton<UpdateManager>
    {
        private List<IUpdate> _allUpdate;

        /// <summary>
        /// 是否运行
        /// </summary>
        private bool _runing;

        public override void F_Init()
        {
            base.F_Init();
            _runing = true;
            _allUpdate = new List<IUpdate>();
        }

        private void Update()
        {
            if (_runing)
            {
                List<IUpdate> temp = ListPool<IUpdate>.Get();
                temp.Clear();
                temp.AddRange(_allUpdate);
                foreach (var update in temp)
                {
                    update?.F_Update(Time.deltaTime);
                }

                ListPool<IUpdate>.Release(temp);
            }
        }

        /// <summary>
        /// 设置更新的状态
        /// </summary>
        /// <param name="stop">是否暂时</param>
        public void F_SetState(bool stop)
        {
            _runing = !stop;
        }

        public void F_AddUpdate(IUpdate update)
        {
            if (_allUpdate.Contains(update))
            {
                Debug.LogError($"已经添加过这个更新接口，怎么还想添加？update:{update}");
            }
            else
            {
                _allUpdate.Add(update);
            }
        }

        public void F_RemoveUpdate(IUpdate update)
        {
            if (_allUpdate.Contains(update))
            {
                _allUpdate.Remove(update);
            }
            else
            {
                Debug.LogError($"要移除的更新接口，已经移除过了update:{update}");
            }
        }
    }
}