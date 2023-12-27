using System;
using System.Collections.Generic;

namespace BBQ.Action {
    
    [Serializable]
    public class ActionSequence {
        public ActionTrigger trigger;
        public List<ActionCommand> condition;
        public List<ActionCommand> commands;
        public int priority;
    }

    public enum ActionTrigger {
        Hit,
        HitOthers,
        Loop,
        LoopOthers,
        Freeze,
        FreezeOthers,
        Fire,
        FireOthers,
        Draw,
        DrawOthers,
        BeforeReset,
        AfterReset,
        Buy,
        BuyOthers,
        BeforeReroll,
        AfterReroll,
        LankUp,
        LankUpOthers,
        UseTool,
        AfterHit,
        Drop,
        DropOthers,
        Placed,
        PlacedOthers,
        StartCooking,
        StartShopping,
        BonusTime,
        GainCoin,
        GainEffect,
    }
    
    [Serializable]
    public class ActionCommand {
        public PlayAction action;
        public string n1;
        public string n2;
    }
    

}
