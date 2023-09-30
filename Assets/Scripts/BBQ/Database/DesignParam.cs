using System.Collections.Generic;
using UnityEngine;

namespace BBQ.Database {
    [CreateAssetMenu(menuName = "Database/DesignParam")]
    public class DesignParam : ScriptableObject {
        public int helpDrawPenalty;
        public int helpHandPenalty;
        public FoodData resetFood;
        public int initialMissionDifficulty;
        public int firstMissionPoint;
        public int secondMissionPoint;


    }
}