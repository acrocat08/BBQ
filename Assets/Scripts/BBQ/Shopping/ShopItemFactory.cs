using BBQ.Database;
using UnityEngine;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "ShopItem/Factory")]
    public class ShopItemFactory : ScriptableObject {
        [SerializeField] private GameObject prefab;

        public ShopFood CreateFood(FoodData data, Shop shop, Transform detailContainer) {
            ShopFood obj = Instantiate(prefab).GetComponent<ShopFood>();
            obj.Init(data, shop, "food", "deck", detailContainer);
            return obj;
        }
    }
}
