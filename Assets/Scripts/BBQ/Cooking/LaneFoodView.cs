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
        
        private static readonly int Seed = Shader.PropertyToID("_seed");

        public override void Draw(FoodObject foodObject) {
            DeckFood deckFood = foodObject.deckFood;
            Image image = foodObject.transform.Find("Image").GetComponent<Image>();
            image.sprite = deckFood.data.foodImage;
            Material mat = lankMaterial[foodObject.deckFood.lank - 1];
            if(mat != null) image.material = new Material(mat);
            if(image.material != null) image.material.SetFloat(Seed, Random.value);
            DrawEffect(foodObject);
        }
        
        public override async void LankUp(FoodObject foodObject) {
            Draw(foodObject);
            Transform foodImage = foodObject.transform.Find("Image");
            foodImage.SetParent(GameObject.Find("Canvas").transform);
            foodImage.localScale = Vector3.one * lankUpStrength;
            foodImage.DOScale(Vector3.one, lankUpDuration).SetEase(Ease.InBack);
            await UniTask.Delay(TimeSpan.FromSeconds(lankUpDuration));

            foodImage.SetParent(foodObject.transform);
        }
    }
}