using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace BBQ.Cooking {
    [CreateAssetMenu(menuName = "LaneFood/View")]
    public class LaneFoodView : FoodObjectView {
        
        [SerializeField] private List<Material> lankMaterial;
        [SerializeField] private float lankUpStrength;
        [SerializeField] private float lankUpDuration;
        [SerializeField] private ItemSet itemSet;
        
        private static readonly int Seed = Shader.PropertyToID("_seed");

        public override void Draw(FoodObject foodObject) {
            DeckFood deckFood = foodObject.deckFood;
            Image image = foodObject.transform.Find("FoodImage").GetComponent<Image>();
            image.sprite = deckFood.data.foodImage;
            Material mat = lankMaterial[foodObject.deckFood.lank - 1];
            if(mat != null) image.material = new Material(mat);
            if(image.material != null) image.material.SetFloat(Seed, Random.value);
            if(foodObject.deckFood.data.useStack) foodObject.transform.Find("Stack").gameObject.SetActive(true);
            if(foodObject.deckFood.memory != "") foodObject.transform.Find("Memory").gameObject.SetActive(true);
            if (foodObject.deckFood.isFrozen) {
                Transform effect = Instantiate(freezeEffectPrefab, foodObject.transform).transform;
                effect.localPosition = Vector3.one;
            }
            DrawEffect(foodObject);
            UpdateStack(foodObject);
            UpdateMemory(foodObject);
        }

        public override void UpdateMemory(FoodObject foodObject) {
            if (foodObject.deckFood.memory == "") {
                foodObject.transform.Find("Memory").gameObject.SetActive(false);
            }
            else {
                foodObject.transform.Find("Memory").gameObject.SetActive(true);
                var x = foodObject.transform.Find("Memory").Find("FoodImage").GetComponent<Image>();
                foodObject.transform.Find("Memory").Find("FoodImage").GetComponent<Image>().sprite
                    = itemSet.SearchFood(foodObject.deckFood.memory).foodImage;
            }
        }

        public override void UpdateStack(FoodObject foodObject) {
            if (!foodObject.deckFood.data.useStack) return;
            foodObject.transform.Find("Stack").GetComponent<Text>().text = foodObject.deckFood.stack.ToString();
        }
        
        public override async UniTask LankUp(FoodObject foodObject) {
            Draw(foodObject);
            Transform foodImage = foodObject.transform.Find("FoodImage");
            foodImage.SetParent(GameObject.Find("Canvas").transform);
            foodImage.localScale = Vector3.one * lankUpStrength;
            foodImage.DOScale(Vector3.one, lankUpDuration).SetEase(Ease.InBack);
            await UniTask.Delay(TimeSpan.FromSeconds(lankUpDuration));

            foodImage.SetParent(foodObject.transform);
        }
    }
}