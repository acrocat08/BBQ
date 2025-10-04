using System;
using System.Collections;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "ShopItem/View")]
    public class ShopItemView : ScriptableObject {

        [SerializeField] private int fallPos;
        [SerializeField] private float fallDuration;
        [SerializeField] private Ease fallEasing;
        [SerializeField] private float floatLength;
        [SerializeField] private float floatSpeed;
        [SerializeField] private float leanLength;
        [SerializeField] private float leanSpeed;
        [SerializeField] private float dropLength;
        [SerializeField] private float jumpLength;
        [SerializeField] private float dropXLength;
        [SerializeField] private float dropDuration;
        [SerializeField] private ViewParam param;
        [SerializeField] private Color discountColor;
        [SerializeField] protected GameObject freezeEffectPrefab;
        [SerializeField] protected Color freezeColor;
        [SerializeField] private SupportIconView iconView;

        
        public void DrawFood(ShopFood shopFood) {
            FoodData data = shopFood.GetFoodData();
            shopFood.transform.Find("FoodImage").transform.Find("Image").GetComponent<Image>().sprite = PlayerConfig.CheckCosplay(data.foodName) ? data.cosplayImage :data.foodImage;
            shopFood.transform.Find("Cost").GetComponent<Text>().text = shopFood.GetCost().ToString();
            if (shopFood.GetCost() < shopFood.GetFoodData().cost)
                shopFood.transform.Find("Cost").GetComponent<Text>().color = discountColor;
            shopFood.transform.Find("Name").GetComponent<Text>().text = data.foodName;
            shopFood.transform.Find("Line").GetComponent<Image>().color = param.tierColors[data.tier];
            shopFood.transform.Find("Shadow").GetComponent<Image>().color = param.tierColors[data.tier];
            if(shopFood.deckFood.data.iconA != null && shopFood.deckFood.data.iconA.inShop)
                iconView.Draw(shopFood.transform.Find("FoodImage").Find("SupportIcon"), shopFood.deckFood.data.iconA);
            else if(shopFood.deckFood.data.iconB != null && shopFood.deckFood.data.iconB.inShop)
                iconView.Draw(shopFood.transform.Find("FoodImage").Find("SupportIcon"), shopFood.deckFood.data.iconB);
            else iconView.Hide(shopFood.transform.Find("FoodImage").Find("SupportIcon"));
        }
        
        public void DrawTool(ShopTool shopTool) {
            ToolData data = shopTool.data;
            shopTool.transform.Find("ToolImage").transform.Find("Image").GetComponent<Image>().sprite = data.toolImage;
            shopTool.transform.Find("Cost").GetComponent<Text>().text = data.cost.ToString();
            shopTool.transform.Find("Name").GetComponent<Text>().text = data.toolName;
            shopTool.transform.Find("Line").GetComponent<Image>().color = param.toolColor;
            shopTool.transform.Find("Shadow").GetComponent<Image>().color = param.toolColor;
        }

        public async void Fall(Transform tr) {
            Vector3 toPos = tr.localPosition;
            tr.localPosition = toPos + fallPos * Vector3.up;
            tr.DOLocalMove(toPos, fallDuration).SetEase(fallEasing)
                .OnComplete(() => Float(tr));
            float angleDiff = Random.Range(0, Mathf.PI * 2);
            Transform imageTr = tr.Find("Image");
            while (tr != null) {
                imageTr.localRotation = Quaternion.Euler(0, 0, leanLength * Mathf.Sin(Time.time * leanSpeed + angleDiff));
                await UniTask.DelayFrame(1);
            }
        }

        private async void Float(Transform tr) {
            bool waitMode = true;
            float posDiff = Random.Range(0, Mathf.PI * 2);
            Transform imageTr = tr.Find("Image");
            Vector3 basePos = imageTr.localPosition;
            while (tr != null) {
                Vector3 offset = Mathf.Sin(Time.time * floatSpeed + posDiff) * floatLength * Vector3.up;
                if (!waitMode) imageTr.localPosition = basePos + offset;
                if (offset.magnitude <= 1f) waitMode = false;
                await UniTask.DelayFrame(1);
            }
        }

        public async UniTask Drop(ShopFood shopFood) {
            shopFood.GetComponent<RectTransform>().pivot = new(0.5f, 0.5f);
            int dir = shopFood.transform.localPosition.x > 0 ? 1 : -1;
            shopFood.transform.DOLocalJump(shopFood.transform.localPosition + dropLength * Vector3.down,
                jumpLength, 1, dropDuration);
            shopFood.transform.DOLocalMoveX(shopFood.transform.localPosition.x + dropXLength * dir * Random.Range(0.5f, 2f), dropDuration)
                .SetEase(Ease.Linear);
            shopFood.transform.DOLocalRotate(new(0, 0, 180), dropDuration);
            shopFood.transform.Find("Coin").GetComponent<Image>().enabled = false;
            shopFood.transform.Find("Cost").GetComponent<Text>().enabled = false;
            shopFood.transform.Find("Shadow").GetComponent<Image>().enabled = false;
            shopFood.transform.Find("Line").GetComponent<Image>().enabled = false;
            shopFood.transform.Find("Name").GetComponent<Text>().enabled = false;
            await UniTask.Delay(TimeSpan.FromSeconds(dropDuration));
            Destroy(shopFood.gameObject);
        }

        public async UniTask Drop(ShopTool shopTool) {
            shopTool.GetComponent<RectTransform>().pivot = new(0.5f, 0.5f);
            int dir = shopTool.transform.localPosition.x > 0 ? 1 : -1;
            shopTool.transform.DOLocalJump(shopTool.transform.localPosition + dropLength * Vector3.down,
                jumpLength, 1, dropDuration);
            shopTool.transform.DOLocalMoveX(shopTool.transform.localPosition.x + dropXLength * dir * Random.Range(0.5f, 2f), dropDuration)
                .SetEase(Ease.Linear);
            shopTool.transform.DOLocalRotate(new(0, 0, 180), dropDuration);
            shopTool.transform.Find("Carbon").GetComponent<Image>().enabled = false;
            shopTool.transform.Find("Cost").GetComponent<Text>().enabled = false;
            shopTool.transform.Find("Shadow").GetComponent<Image>().enabled = false;
            shopTool.transform.Find("Line").GetComponent<Image>().enabled = false;
            shopTool.transform.Find("Name").GetComponent<Text>().enabled = false;
            await UniTask.Delay(TimeSpan.FromSeconds(dropDuration));
            Destroy(shopTool.gameObject);
        }

        public void Discount(ShopFood shopFood) {
            Transform tag = shopFood.transform.Find("FoodImage").Find("Image").Find("Tag");
            tag.GetComponent<Image>().enabled = true;
            tag.localScale = Vector3.zero;
            tag.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutQuad);
        }
        
        
        public virtual void Freeze(ShopFood shopFood) {
            Image image = shopFood.transform.Find("FoodImage").Find("Image").GetComponent<Image>();
            image.color = freezeColor;
            FreezeEffect effect = Instantiate(freezeEffectPrefab, shopFood.transform)
                .GetComponent<FreezeEffect>();
            effect.transform.localPosition = Vector3.zero;
            effect.Freeze(1f);
            effect.transform.SetParent(image.transform);
        }
    }
}
