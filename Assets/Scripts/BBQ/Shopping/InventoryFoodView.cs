using System;
using System.Collections.Generic;
using BBQ.Common;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "InventoryView/View")]
    public class InventoryFoodView : FoodObjectView {

        [SerializeField] private List<Color> lankColor;
        [SerializeField] private float lankUpStrength;
        [SerializeField] private float lankUpDuration;
        
        public override void Draw(FoodObject foodObject) {
            DeckFood deckFood = foodObject.deckFood;
            Image foodImage = foodObject.transform.Find("Object").Find("Image").GetComponent<Image>();
            foodImage.sprite = deckFood.data ? deckFood.data.foodImage : null;
            foodImage.enabled = deckFood.data != null;
            foodImage.color = Color.white;
            Image lankImage = foodObject.transform.Find("Object").Find("Lank").GetComponent<Image>();
            lankImage.color = deckFood.data != null ? lankColor[deckFood.lank - 1] : Color.clear;
            if(foodObject.transform.Find("Object").Find("FireEffect(Clone)")) 
                Destroy(foodObject.transform.Find("Object").Find("FireEffect(Clone)").gameObject);
            DrawEffect(foodObject);
        }
        
        public override async void Drop(FoodObject foodObject) {
            int dir = foodObject.transform.localPosition.x > 0 ? 1 : -1;
            Vector2 prevPos = foodObject.transform.Find("Object").localPosition;
            Transform image = foodObject.transform.Find("Object");
            image.transform.DOLocalJump(image.transform.localPosition + fallLength * Vector3.down,
                jumpLength, 1, fallDuration);
            image.transform.DOLocalMoveX(image.transform.localPosition.x + fallXLength * dir * Random.Range(0.5f, 2f), fallDuration)
                .SetEase(Ease.Linear);
            image.transform.DOLocalRotate(new Vector3(0, 0, 180), fallDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(fallDuration));
            image.transform.localPosition = prevPos;
            image.transform.localRotation = Quaternion.Euler(0, 0, 0);
            foodObject.deckFood.data = null;
            Draw(foodObject);
        }
        
        public override void Fire(FoodObject foodObject) {
            Image image = foodObject.transform.Find("Object").Find("Image").GetComponent<Image>();
            image.color = fireColor;
            Transform effect = Instantiate(fireEffectPrefab).transform;
            effect.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100); 
            effect.Find("Effect").GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150); 
            effect.SetParent(foodObject.transform.Find("Object"));
            effect.localPosition = Vector3.zero;
            effect.Find("Effect").localPosition = Vector3.zero + Vector3.up * 30;
            effect.localScale = Vector3.one;
            effect.SetSiblingIndex(0);
        }

        public override async void LankUp(FoodObject foodObject) {
            Transform foodImage = foodObject.transform.Find("Object").Find("Image");
            foodImage.SetParent(GameObject.Find("Canvas").transform);
            foodImage.localScale = Vector3.one * lankUpStrength;
            foodImage.DOScale(Vector3.one, lankUpDuration).SetEase(Ease.InBack);
            await UniTask.Delay(TimeSpan.FromSeconds(lankUpDuration));
            foodImage.SetParent(foodObject.transform.Find("Object"));
        }
        
        public override void Invoke(FoodObject foodObject) {
            foodObject.transform.Find("Object").Find("Image").localScale = Vector3.one * shakeStrength;
            foodObject.transform.Find("Object").Find("Image").DOScale(Vector3.one, shakeDuration).SetEase(Ease.OutElastic);
        }
    }
}