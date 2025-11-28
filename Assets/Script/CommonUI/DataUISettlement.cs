using System;

namespace Script.CommonUI
{
    /// <summary>
    /// 结算界面数据
    /// </summary>
    public class DataUISettlement
    {
        public enResult Result;
        
        public Action OnClickRestartGame;
    }

    public enum enResult
    {
        挑战成功,
        挑战失败,
    } 
}