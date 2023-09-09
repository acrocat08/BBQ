using BBQ.Database;
using UnityEngine;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "ShopItem/Factory")]
    public class ShopItemFactory : ScriptableObject {
        [SerializeField] private GameObject prefab;

        public ShopItem CreateFood(FoodData data, Shop shop, Transform detailContainer) {
            ShopItem obj = Instantiate(prefab).GetComponent<ShopItem>();
            obj.Init(data, shop, "food", "deck", detailContainer);
            return obj;
        }
    }
}
