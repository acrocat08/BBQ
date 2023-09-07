using System;
using System.Collections.Generic;
using UnityEngine;

namespace BBQ.Action {
    
    [Serializable]
    public class FoodAction {
        public string summary;
        public List<ActionSequence> sequences;
    }
}
