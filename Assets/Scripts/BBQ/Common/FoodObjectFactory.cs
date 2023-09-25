using BBQ.Cooking;
using BBQ.PlayData;
using UnityEngine;

namespace BBQ.Common {
    [CreateAssetMenu(menuName = "FoodObject/Factory")]
    public class FoodObjectFactory : ScriptableObject {
        [SerializeField] private GameObject prefab;

        public FoodObject Create(DeckFood deckFood, Transform parent) {
            FoodObject obj = Instantiate(prefab).GetComponent<FoodObject>();
            obj.Init(deckFood);
            Transform tr = obj.transform;
            tr.SetParent(parent);
            tr.localPosition = Vector3.zero;
            tr.localScale = Vector3.one;
            return obj;
        }
    }
}
