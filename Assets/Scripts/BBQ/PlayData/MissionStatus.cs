using System;
using System.Collections.Generic;
using UnityEngine;

namespace BBQ.PlayData {
    [Serializable]
    public class MissionStatus {
        public Mission mission;
        public int goal;
        public int now;
    }
}
