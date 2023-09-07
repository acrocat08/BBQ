using System.Collections.Generic;
using UnityEngine;

namespace BBQ.Database {
    [CreateAssetMenu(menuName = "Database/Cooking")]
    public class CookingParam : ScriptableObject {
        public Vector2 laneWindowSize;
        public List<int> laneSpeed;
        public int foodMaxNumInLane;
        public int foodSize;
        public int foodMargin;
        public int foodCollisionSize;


    }
}