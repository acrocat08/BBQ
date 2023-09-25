using System;
using System.Collections.Generic;
using BBQ.Action.Play;
using BBQ.Cooking;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BBQ.Common {
    [CreateAssetMenu(menuName = "FoodObject/View")]
    public class FoodObjectView : ScriptableObject {
                [SerializeField] private GameObject hitEffectPrefab;
        [SerializeField] protected float shakeStrength;
        [SerializeField] protected float shakeDuration;

        [SerializeField] protected float fallLength;
        [SerializeField] protected float jumpLength;
        [SerializeField] protected float fallXLength;
        [SerializeField] protected float fallDuration;
        
        [SerializeField] private GameObject freezeEffectPrefab;
        [SerializeField] private Color freezeColor;
        
        [SerializeField] protected GameObject fireEffectPrefab;
        [SerializeField] protected Color fireColor;


        [SerializeField] private float effectDuration;
        [SerializeField] private float effectStrength;
        [SerializeField] Ease effectEasing;
        
        public virtual void Draw(FoodObject foodObject) {
        }

        public void DrawEffect(FoodObject foodObject) {
            FoodEffect effect = foodObject.deckFood.effect;
            if (effect == null) {
                Transform frame = foodObject.transform.Find("FoodEffect");
                frame.localScale = Vector3.zero;
            }
            else {
                foodObject.transform.Find("FoodEffect").gameObject.SetActive(true);
                Image icon = foodObject.transform.Find("FoodEffect").Find("Icon").GetComponent<Image>();
                icon.sprite = foodObject.deckFood.effect.effectImage;
            }
        }

        public void AddEffect(FoodObject foodObject) {
            FoodEffect effect = foodObject.deckFood.effect;
            if (effect == null) {
                Transform frame = foodObject.transform.Find("FoodEffect");
                frame.DOScale(Vector3.zero, effectDuration);
            }
            else {
                Image icon = foodObject.transform.Find("FoodEffect").Find("Icon").GetComponent<Image>();
                icon.sprite = foodObject.deckFood.effect.effectImage;
                Transform frame = foodObject.transform.Find("FoodEffect");
                frame.localScale = Vector3.one * effectStrength;
                frame.DOScale(Vector3.one, effectDuration).SetEase(effectEasing);
            }
        }

        public void Hit(FoodObject foodObject) {
            float rotateDelta = Random.Range(5f, 15f);
            if (Random.value >= 0.5f) rotateDelta *= -1;
            foodObject.transform.Find("Image").DOLocalRotate(new Vector3(0, 0, rotateDelta),
                0.2f).SetEase(Ease.OutExpo);
            for (int i = 0; i < 6; i++) {
                ShotEffect effect = Instantiate(hitEffectPrefab, Vector3.zero, Quaternion.identity, foodObject.transform).GetComponent<ShotEffect>();
                effect.Init(Random.Range(i * 60, i * 60 + 60));
                effect.GetComponent<Image>().color = foodObject.deckFood.data.color;
            }
        }
        
        public virtual async void Drop(FoodObject foodObject) {
            int dir = foodObject.transform.localPosition.x > 0 ? 1 : -1;
            foodObject.transform.SetParent(GameObject.Find("Canvas").transform);
            foodObject.transform.DOLocalJump(foodObject.transform.localPosition + fallLength * Vector3.down,
                jumpLength, 1, fallDuration);
            foodObject.transform.DOLocalMoveX(foodObject.transform.localPosition.x + fallXLength * dir * Random.Range(0.5f, 2f), fallDuration)
                .SetEase(Ease.Linear);
            foodObject.transform.DOLocalRotate(new Vector3(0, 0, 180), fallDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(fallDuration));
            Destroy(foodObject.gameObject);
        }

        public void Freeze(FoodObject foodObject) {
            Image image = foodObject.transform.Find("Image").GetComponent<Image>();
            image.color = freezeColor;
            FreezeEffect effect = Instantiate(freezeEffectPrefab, foodObject.transform)
                .GetComponent<FreezeEffect>();
            effect.transform.localPosition = Vector3.zero;
            effect.Draw();
        }
        
        public virtual void Fire(FoodObject foodObject) {
            Image image = foodObject.transform.Find("Image").GetComponent<Image>();
            image.color = fireColor;
            Transform effect = Instantiate(fireEffectPrefab).transform;
            effect.SetParent(foodObject.transform);
            effect.localPosition = Vector3.zero;
            effect.localScale = Vector3.one;
            effect.SetSiblingIndex(0);
        }


        public virtual void Invoke(FoodObject foodObject) {
            foodObject.transform.Find("Image").localScale = Vector3.one * shakeStrength;
            foodObject.transform.Find("Image").DOScale(Vector3.one, shakeDuration).SetEase(Ease.OutElastic);
        }

        public virtual void LankUp(FoodObject foodObject) {
        }
        
    }
}
