using System.Collections.Generic;
using UnityEngine;

namespace BBQ.Database {
    [CreateAssetMenu(menuName = "Database/Cooking")]
    public class ViewParam : ScriptableObject {
        public int foodMaxNumInLane;
        public int foodSize;
        public int foodMargin;
        public int foodCollisionSize;
        public List<Color> tierColors;
        public Color toolColor;
        public Color effectColor;


    }
}