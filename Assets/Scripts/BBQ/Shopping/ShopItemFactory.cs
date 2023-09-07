using BBQ.Database;
using UnityEngine;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "ShopItem/Factory")]
    public class ShopItemFactory : ScriptableObject {
        [SerializeField] private GameObject prefab;


        public ShopItem Create(FoodData data) {
            ShopItem obj = Instantiate(prefab).GetComponent<ShopItem>();
            obj.Init(data);
            return obj;
        }
    }
}
