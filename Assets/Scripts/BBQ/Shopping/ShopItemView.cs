using System;
using System.Collections;
using BBQ.Database;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "ShopItem/View")]
    public class ShopItemView : ScriptableObject {

        [SerializeField] private Color[] colors;
        [SerializeField] private int fallPos;
        [SerializeField] private float fallDuration;
        [SerializeField] private Ease fallEasing;
        [SerializeField] private float floatLength;
        [SerializeField] private float floatSpeed;

        public void Draw(ShopItem shopItem) {
            FoodData data = shopItem.GetFoodData();
            shopItem.transform.Find("Food").GetComponent<Image>().sprite = data.foodImage;
            shopItem.transform.Find("Cost").GetComponent<Text>().text = data.cost.ToString();
            shopItem.transform.Find("Name").GetComponent<Text>().text = data.foodName;
            shopItem.transform.Find("Line").GetComponent<Image>().color = colors[data.tier - 1];
            shopItem.transform.Find("Shadow").GetComponent<Image>().color = colors[data.tier - 1];
        }

        public void Fall(ShopItem shopItem) {
            Transform tr = shopItem.transform.Find("Food").transform;
            Vector3 toPos = tr.localPosition;
            tr.localPosition = toPos + fallPos * Vector3.up;
            tr.DOLocalMove(toPos, fallDuration).SetEase(fallEasing)
                .OnComplete(() => Float(shopItem));
        }

        private async void Float(ShopItem shopItem) {
            Transform tr = shopItem.transform.Find("Food").transform;
            Vector3 basePos = tr.localPosition;
            bool waitMode = true;
            while (shopItem != null) {
                Vector3 offset = Mathf.Sin(Time.time * floatSpeed) * floatLength * Vector3.up;
                if (!waitMode) tr.localPosition = basePos + offset;
                if (offset.magnitude <= 1f) waitMode = false;
                await UniTask.DelayFrame(1);
            }
        }
    }
}
