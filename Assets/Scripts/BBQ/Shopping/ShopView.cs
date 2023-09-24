using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "Shop/View")]
    public class ShopView : ScriptableObject {

        [SerializeField] private int itemWidth;
        [SerializeField] private int margin;
        [SerializeField] private float moveDuration;

        public void PlaceItem(ShopFood item, int index, Transform container) {
            item.transform.SetParent(container);
            item.GetComponent<RectTransform>().anchoredPosition
                = (itemWidth + margin) * index * Vector3.right;
            item.Fall();
        }
        
        public void MoveItem(ShopFood item, int index, Transform container) {
            item.transform.SetParent(container);
            Vector3 toPos = (itemWidth + margin) * index * Vector3.right;
            item.GetComponent<RectTransform>().DOLocalMove(toPos, moveDuration);
        }

        public void UpdateText(Shop shop, int cost) {
            Text levelText = shop.transform.Find("Level").Find("Text").GetComponent<Text>();
            levelText.text = "Level " + shop.GetShopLevel();
            Text buttonText = shop.transform.Find("LevelUp").Find("Cost").GetComponent<Text>();
            buttonText.text = cost.ToString();
        }


    }
}