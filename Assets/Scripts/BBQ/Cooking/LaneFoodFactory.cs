using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using UnityEngine;

namespace BBQ.Cooking {
    [CreateAssetMenu(menuName = "LaneFood/Factory")]
    public class LaneFoodFactory : ScriptableObject {
        [SerializeField] private GameObject prefab;

        public FoodObject Create(DeckFood deckFood, Transform parent) {
            FoodObject obj = Instantiate(prefab).GetComponent<FoodObject>();
            obj.Init(deckFood);
            Transform tr = obj.transform;
            tr.SetParent(parent, false);
            tr.localPosition = Vector3.zero;
            tr.localScale = Vector3.one;
            return obj;
        }
    }
}
