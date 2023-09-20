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
    public class LaneFoodView : ScriptableObject {
        [SerializeField] private GameObject hitEffectPrefab;
        [SerializeField] private float shakeStrength;
        [SerializeField] private float shakeDuration;

        [SerializeField] private float fallLength;
        [SerializeField] private float jumpLength;
        [SerializeField] private float fallXLength;
        [SerializeField] private float fallDuration;
        
        [SerializeField] private GameObject freezeEffectPrefab;
        [SerializeField] private Color freezeColor;
        
        [SerializeField] private GameObject fireEffectPrefab;
        [SerializeField] private Color fireColor;

        [SerializeField] private List<Material> lankMaterial;

        [SerializeField] private float effectDuration;
        [SerializeField] private float effectStrength;
        [SerializeField] Ease effectEasing;
        
        private static readonly int Seed = Shader.PropertyToID("_seed");

        public void Draw(FoodObject laneFood) {
            DeckFood deckFood = laneFood.deckFood;
            Image image = laneFood.transform.Find("Image").GetComponent<Image>();
            image.sprite = deckFood.data.foodImage;
            Material mat = lankMaterial[laneFood.deckFood.lank - 1];
            if(mat != null) image.material = new Material(mat);
            if(image.material != null) image.material.SetFloat(Seed, Random.value);
            DrawEffect(laneFood);
        }

        public void DrawEffect(FoodObject laneFood) {
            FoodEffect effect = laneFood.deckFood.effect;
            if (effect == null) {
                Transform frame = laneFood.transform.Find("FoodEffect");
                frame.localScale = Vector3.zero;
            }
            else {
                laneFood.transform.Find("FoodEffect").gameObject.SetActive(true);
                Image icon = laneFood.transform.Find("FoodEffect").Find("Icon").GetComponent<Image>();
                icon.sprite = laneFood.deckFood.effect.effectImage;
            }
        }

        public void AddEffect(FoodObject laneFood) {
            FoodEffect effect = laneFood.deckFood.effect;
            if (effect == null) {
                Transform frame = laneFood.transform.Find("FoodEffect");
                frame.DOScale(Vector3.zero, effectDuration);
            }
            else {
                Image icon = laneFood.transform.Find("FoodEffect").Find("Icon").GetComponent<Image>();
                icon.sprite = laneFood.deckFood.effect.effectImage;
                Transform frame = laneFood.transform.Find("FoodEffect");
                frame.localScale = Vector3.one * effectStrength;
                frame.DOScale(Vector3.one, effectDuration).SetEase(effectEasing);
            }

        }

        public void Hit(FoodObject laneFood) {
            float rotateDelta = Random.Range(5f, 15f);
            if (Random.value >= 0.5f) rotateDelta *= -1;
            laneFood.transform.Find("Image").DOLocalRotate(new Vector3(0, 0, rotateDelta),
                0.2f).SetEase(Ease.OutExpo);
            for (int i = 0; i < 6; i++) {
                ShotEffect effect = Instantiate(hitEffectPrefab, Vector3.zero, Quaternion.identity, laneFood.transform).GetComponent<ShotEffect>();
                effect.Init(Random.Range(i * 60, i * 60 + 60));
                effect.GetComponent<Image>().color = laneFood.deckFood.data.color;
            }
        }
        
        public async void Drop(FoodObject laneFood) {
            int dir = laneFood.transform.localPosition.x > 0 ? 1 : -1;
            laneFood.transform.DOLocalJump(laneFood.transform.localPosition + fallLength * Vector3.down,
                jumpLength, 1, fallDuration);
            laneFood.transform.DOLocalMoveX(laneFood.transform.localPosition.x + fallXLength * dir * Random.Range(0.5f, 2f), fallDuration)
                .SetEase(Ease.Linear);
            laneFood.transform.DOLocalRotate(new Vector3(0, 0, 180), fallDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(fallDuration));
            Destroy(laneFood.gameObject);
        }

        public void Freeze(FoodObject laneFood) {
            Image image = laneFood.transform.Find("Image").GetComponent<Image>();
            image.color = freezeColor;
            FreezeEffect effect = Instantiate(freezeEffectPrefab, laneFood.transform)
                .GetComponent<FreezeEffect>();
            effect.transform.localPosition = Vector3.zero;
            effect.Draw();
        }
        
        public void Fire(FoodObject laneFood) {
            Image image = laneFood.transform.Find("Image").GetComponent<Image>();
            image.color = fireColor;
            Transform effect = Instantiate(fireEffectPrefab).transform;
            effect.SetParent(laneFood.transform);
            effect.localPosition = Vector3.zero;
            effect.localScale = Vector3.one;
            effect.SetSiblingIndex(0);
        }


        public void Invoke(FoodObject food) {
            food.transform.Find("Image").localScale = Vector3.one * shakeStrength;
            food.transform.Find("Image").DOScale(Vector3.one, shakeDuration).SetEase(Ease.OutElastic);
        }


    }
}