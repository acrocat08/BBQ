using System.Collections.Generic;
using UnityEngine;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "Shop/View")]
    public class ShopView : ScriptableObject {

        [SerializeField] private int itemWidth;
        [SerializeField] private int margin;
        public void PlaceItem(List<ShopItem> items, Transform container) {
            for (int i = 0; i < items.Count; i++) {
                items[i].transform.SetParent(container);
                items[i].GetComponent<RectTransform>().anchoredPosition
                     = (itemWidth + margin) * i * Vector3.right;
                items[i].Fall();
            }
        }
    }
}
