using UnityEngine;

namespace BBQ.Database {
    [CreateAssetMenu(menuName = "Database/SupportIcon")]
    public class SupportIcon : ScriptableObject {

        public Sprite iconImage;
        public Color backColor;
        public bool inCooking;
        public bool inShop;
        public bool inInventory;

    }
}
