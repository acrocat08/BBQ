using System;
using System.Collections.Generic;
using BBQ.Common;
using BBQ.Database;
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
        [SerializeField] private GameObject lankUpPrefab;
        [SerializeField] private List<Material> lankMaterial;
        [SerializeField] private SupportIconView iconView;

        
        private static readonly int Seed = Shader.PropertyToID("_seed");

        
        public override void DrawEffect(FoodObject foodObject) {
            FoodEffect effect = foodObject.deckFood.effect;
            if (effect == null) {
                Transform frame = foodObject.transform.Find("Object").Find("FoodEffect");
                frame.localScale = Vector3.zero;
            }
            else {
                Transform frame = foodObject.transform.Find("Object").Find("FoodEffect");
                frame.localScale = Vector3.one;
                Image icon = foodObject.transform.Find("Object").Find("FoodEffect").Find("Icon").GetComponent<Image>();
                icon.sprite = foodObject.deckFood.effect.effectImage;
            }
        }

        public override void AddEffect(FoodObject foodObject) {
            FoodEffect effect = foodObject.deckFood.effect;
            if (effect == null) {
                Transform frame = foodObject.transform.Find("Object").Find("FoodEffect");
                frame.DOScale(Vector3.zero, effectDuration);
            }
            else {
                Image icon = foodObject.transform.Find("Object").Find("FoodEffect").Find("Icon").GetComponent<Image>();
                icon.sprite = foodObject.deckFood.effect.effectImage;
                Transform frame = foodObject.transform.Find("Object").Find("FoodEffect");
                frame.localScale = Vector3.one * effectStrength;
                frame.DOScale(Vector3.one, effectDuration).SetEase(effectEasing);
            }
        }
        
        public override void Draw(FoodObject foodObject) {
            DeckFood deckFood = foodObject.deckFood;
            Image foodImage = foodObject.transform.Find("Object").Find("Image").GetComponent<Image>();
            foodImage.sprite = deckFood.data ? (PlayerConfig.CheckCosplay(deckFood.data.foodName)
                ? deckFood.data.cosplayImage
                : deckFood.data.foodImage) : null;
            foodImage.enabled = deckFood.data != null;
            foodImage.color = Color.white;
            SetMaterial(foodImage, foodObject.deckFood.lank);
            Image lankImage = foodObject.transform.Find("Object").Find("Lank").GetComponent<Image>();
            lankImage.color = deckFood.data != null ? lankColor[deckFood.lank - 1] : Color.clear;
            if(foodObject.transform.Find("Object").Find("FireEffect(Clone)")) 
                Destroy(foodObject.transform.Find("Object").Find("FireEffect(Clone)").gameObject);
            if(foodObject.transform.Find("FreezeEffect(Clone)")) 
                Destroy(foodObject.transform.Find("FreezeEffect(Clone)").gameObject);
            if(foodObject.deckFood.data && foodObject.deckFood.data.useStack) foodObject.transform.Find("Object").Find("Stack").gameObject.SetActive(true);
            else foodObject.transform.Find("Object").Find("Stack").gameObject.SetActive(false);
            DrawEffect(foodObject);
            UpdateStack(foodObject);
            
            iconView.Hide(foodObject.transform.transform.Find("Object").Find("SupportIcon_A"));
            iconView.Hide(foodObject.transform.transform.Find("Object").Find("SupportIcon_B"));
            if (foodObject.deckFood.data) {
                if(foodObject.deckFood.data.iconA != null && foodObject.deckFood.data.iconA.inInventory)
                    iconView.Draw(foodObject.transform.transform.Find("Object").Find("SupportIcon_A"), foodObject.deckFood.data.iconA);
                if(foodObject.deckFood.data.iconB != null && foodObject.deckFood.data.iconB.inInventory)
                    iconView.Draw(foodObject.transform.transform.Find("Object").Find(foodObject.deckFood.data.iconA.inInventory ? "SupportIcon_B" : "SupportIcon_A"), foodObject.deckFood.data.iconB);
            }
        }
        
        public override void UpdateStack(FoodObject foodObject) {
            if (foodObject.deckFood.data == null) return;
            if (!foodObject.deckFood.data.useStack) return;
            foodObject.transform.Find("Object").Find("Stack").GetComponent<Text>().text = foodObject.deckFood.stack.ToString();
        }
        
        public override async void Drop(FoodObject foodObject) {
            int dir = foodObject.transform.localPosition.x > 0 ? 1 : -1;
            Vector2 prevPos = foodObject.transform.Find("Object").Find("Fired").localPosition;
            Transform image = foodObject.transform.Find("Object").Find("Fired");
            image.transform.DOLocalJump(image.transform.localPosition + fallLength * Vector3.down,
                jumpLength, 1, fallDuration);
            image.transform.DOLocalMoveX(image.transform.localPosition.x + fallXLength * dir * Random.Range(0.5f, 2f), fallDuration)
                .SetEase(Ease.Linear);
            image.transform.DOLocalRotate(new(0, 0, 180), fallDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(fallDuration));
            image.gameObject.SetActive(false);
            image.transform.localPosition = prevPos;
            image.transform.localRotation = Quaternion.Euler(0, 0, 0);
            Draw(foodObject);
        }
        
        public async UniTask ForkDrop(FoodObject foodObject, FoodData prevData) {
            float duration = fallDuration * 1.2f;
            Vector2 prevPos = foodObject.transform.Find("Object").Find("Image").localPosition;
            Transform image = foodObject.transform.Find("Object").Find("Image");
            Image foodImage = image.GetComponent<Image>();
            foodImage.enabled = true;
            foodImage.sprite = PlayerConfig.CheckCosplay(prevData.foodName)
                ? prevData.cosplayImage
                : prevData.foodImage;
            int dir = foodObject.transform.localPosition.x > 0 ? 1 : -1;
            image.transform.DOLocalJump(image.transform.localPosition + fallLength * Vector3.down,
                jumpLength, 1, duration);
            image.transform.DOLocalMoveX(image.transform.localPosition.x + fallXLength * dir * Random.Range(0.5f, 2f), duration)
                .SetEase(Ease.Linear);
            image.transform.DOLocalRotate(new(0, 0, 180), duration);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            image.transform.localPosition = prevPos;
            image.transform.localRotation = Quaternion.Euler(0, 0, 0);
            Draw(foodObject);
        }
        
        
        
        public override void Fire(FoodObject foodObject) {
            foodObject.transform.Find("Object").Find("Fired").gameObject.SetActive(true);
            Image image = foodObject.transform.Find("Object").Find("Fired").Find("Image").GetComponent<Image>();
            image.sprite = PlayerConfig.CheckCosplay(foodObject.deckFood.data.foodName) ? foodObject.deckFood.data.cosplayImage :foodObject.deckFood.data.foodImage;
        }
        
        public override void Freeze(FoodObject foodObject) {
            Image image = foodObject.transform.Find("Object").transform.Find("Image").GetComponent<Image>();
            image.color = freezeColor;
            FreezeEffect effect = Instantiate(freezeEffectPrefab, foodObject.transform)
                .GetComponent<FreezeEffect>();
            effect.transform.localPosition = Vector3.zero;
            effect.GetComponent<RectTransform>().sizeDelta = new(100, 100); 
            effect.Freeze(0.3f);
        }

        public override async UniTask LankUp(FoodObject foodObject) {
            Transform foodImage = foodObject.transform.Find("Object").Find("Image");
            SetMaterial(foodImage.GetComponent<Image>(), foodObject.deckFood.lank);
            foodImage.SetParent(GameObject.Find("Canvas").transform);
            foodImage.localScale = Vector3.one * lankUpStrength;
            foodImage.DOScale(Vector3.one, lankUpDuration).SetEase(Ease.InBack);
            GameObject star = Instantiate(lankUpPrefab, foodObject.transform, true);
            star.transform.localPosition = Vector3.zero;
            star.transform.SetParent(foodObject.transform.parent.Find("Star"));
            star.GetComponent<Image>().color = foodObject.deckFood.data.color;
            star.transform.DOScale(Vector3.one * 5, lankUpDuration * 2f).SetEase(Ease.OutQuart);
            star.GetComponent<Image>().DOFade(0f, lankUpDuration * 2f).SetEase(Ease.InQuart)
                .OnComplete(() => Destroy(star));
            await UniTask.Delay(TimeSpan.FromSeconds(lankUpDuration));
            foodImage.SetParent(foodObject.transform.Find("Object"));
            foodImage.transform.SetSiblingIndex(0);
        }
        
        public override void Invoke(FoodObject foodObject) {
            Debug.Log(foodObject.gameObject.name);
            foodObject.transform.Find("Object").Find("Image").localScale = Vector3.one * shakeStrength;
            foodObject.transform.Find("Object").Find("Image").DOScale(Vector3.one, shakeDuration).SetEase(Ease.OutElastic);
        }

        void SetMaterial(Image foodImage, int lank) {
            Material mat = lankMaterial[lank - 1];
            if(mat != null) foodImage.material = new(mat);
            if(foodImage.material != null) foodImage.material.SetFloat(Seed, Random.value);
        }
    }
}
