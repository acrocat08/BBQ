using System;
using System.Collections.Generic;
using BBQ.Common;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "InventoryView/View")]
    public class InventoryFoodView : FoodObjectView {

        [SerializeField] private List<Color> lankColor;
        [SerializeField] private float lankUpStrength;
        [SerializeField] private float lankUpDuration;
        
        public override void Draw(FoodObject foodObject) {
            DeckFood deckFood = foodObject.deckFood;
            Image foodImage = foodObject.transform.Find("Image").GetComponent<Image>();
            foodImage.sprite = deckFood.data ? deckFood.data.foodImage : null;
            foodImage.enabled = deckFood.data != null;
            Image lankImage = foodObject.transform.Find("Lank").GetComponent<Image>();
            lankImage.color = deckFood.data != null ? lankColor[deckFood.lank - 1] : Color.clear;
        }

        public override async void LankUp(FoodObject foodObject) {
            Transform foodImage = foodObject.transform.Find("Image");
            foodImage.SetParent(GameObject.Find("Canvas").transform);
            foodImage.localScale = Vector3.one * lankUpStrength;
            foodImage.DOScale(Vector3.one, lankUpDuration).SetEase(Ease.InBack);
            await UniTask.Delay(TimeSpan.FromSeconds(lankUpDuration));
            foodImage.SetParent(foodObject.transform);
        }
    }
}
