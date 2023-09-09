using System;
using System.Collections.Generic;
using UnityEngine;

namespace BBQ.Action {
    
    [Serializable]
    public class FoodAction {
        [TextArea] public string[] summaries;
        public List<ActionSequence> sequences;
    }
}
