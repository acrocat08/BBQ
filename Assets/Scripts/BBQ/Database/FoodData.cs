using BBQ.Action;
using UnityEngine;

namespace BBQ.Database {
    [CreateAssetMenu(menuName = "Database/Food")]
    public class FoodData : ScriptableObject {
        public string foodName;
        public int cost;
        public Sprite foodImage;
        public Color color;
        public FoodAction action;
    }
}
