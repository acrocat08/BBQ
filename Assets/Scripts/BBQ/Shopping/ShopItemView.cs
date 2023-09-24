using System;
using System.Collections;
using BBQ.Database;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "ShopItem/View")]
    public class ShopItemView : ScriptableObject {

        [SerializeField] private Color[] colors;
        [SerializeField] private int fallPos;
        [SerializeField] private float fallDuration;
        [SerializeField] private Ease fallEasing;
        [SerializeField] private float floatLength;
        [SerializeField] private float floatSpeed;
        [SerializeField] private float dropLength;
        [SerializeField] private float jumpLength;
        [SerializeField] private float dropXLength;
        [SerializeField] private float dropDuration;

        public void DrawFood(ShopFood shopFood) {
            FoodData data = shopFood.GetFoodData();
            shopFood.transform.Find("Image").GetComponent<Image>().sprite = data.foodImage;
            shopFood.transform.Find("Cost").GetComponent<Text>().text = data.cost.ToString();
            shopFood.transform.Find("Name").GetComponent<Text>().text = data.foodName;
            shopFood.transform.Find("Line").GetComponent<Image>().color = colors[data.tier - 1];
            shopFood.transform.Find("Shadow").GetComponent<Image>().color = colors[data.tier - 1];
        }

        public void Fall(Transform tr) {
            Vector3 toPos = tr.localPosition;
            tr.localPosition = toPos + fallPos * Vector3.up;
            tr.DOLocalMove(toPos, fallDuration).SetEase(fallEasing)
                .OnComplete(() => Float(tr));
        }

        private async void Float(Transform tr) {
            Vector3 basePos = tr.localPosition;
            bool waitMode = true;
            while (tr != null) {
                Vector3 offset = Mathf.Sin(Time.time * floatSpeed) * floatLength * Vector3.up;
                if (!waitMode) tr.localPosition = basePos + offset;
                if (offset.magnitude <= 1f) waitMode = false;
                await UniTask.DelayFrame(1);
            }
        }

        public async UniTask Drop(ShopFood shopFood) {
            shopFood.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            int dir = shopFood.transform.localPosition.x > 0 ? 1 : -1;
            shopFood.transform.DOLocalJump(shopFood.transform.localPosition + dropLength * Vector3.down,
                jumpLength, 1, dropDuration);
            shopFood.transform.DOLocalMoveX(shopFood.transform.localPosition.x + dropXLength * dir * Random.Range(0.5f, 2f), dropDuration)
                .SetEase(Ease.Linear);
            shopFood.transform.DOLocalRotate(new Vector3(0, 0, 180), dropDuration);
            shopFood.transform.Find("Coin").GetComponent<Image>().enabled = false;
            shopFood.transform.Find("Cost").GetComponent<Text>().enabled = false;
            shopFood.transform.Find("Shadow").GetComponent<Image>().enabled = false;
            shopFood.transform.Find("Line").GetComponent<Image>().enabled = false;
            shopFood.transform.Find("Name").GetComponent<Text>().enabled = false;
            await UniTask.Delay(TimeSpan.FromSeconds(dropDuration));
            Destroy(shopFood.gameObject);
        }
    }
}
