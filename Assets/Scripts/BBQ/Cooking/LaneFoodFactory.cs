using BBQ.Database;
using BBQ.PlayData;
using UnityEngine;

namespace BBQ.Cooking {
    [CreateAssetMenu(menuName = "LaneFood/Factory")]
    public class LaneFoodFactory : ScriptableObject {
        [SerializeField] private GameObject prefab;

        public LaneFood Create(DeckFood deckFood, Transform parent) {
            LaneFood obj = Instantiate(prefab).GetComponent<LaneFood>();
            obj.Init(deckFood);
            Transform tr = obj.transform;
            tr.SetParent(parent);
            tr.localPosition = Vector3.zero;
            return obj;
        }
    }
}
