using BBQ.Database;
using UnityEngine;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "ShopItem/Factory")]
    public class ShopItemFactory : ScriptableObject {
        [SerializeField] private GameObject foodPrefab;
        [SerializeField] private GameObject toolPrefab;

        public ShopFood CreateFood(FoodData data, Shop shop, ItemDetail detail) {
            ShopFood obj = Instantiate(foodPrefab).GetComponent<ShopFood>();
            obj.Init(data, shop, "food", "deck", detail);
            return obj;
        }

        public ShopTool CreateTool(ToolData data, Shop shop, ItemDetail detail) {
            ShopTool obj = Instantiate(toolPrefab).GetComponent<ShopTool>();
            obj.Init(data, shop, "tool", detail);
            return obj;
        }
    }
}
