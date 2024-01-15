using System;
using BBQ.Action.Play;
using BBQ.Common;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Cooking {
    [CreateAssetMenu(menuName = "Deck/View")]
    public class DeckView : ScriptableObject {

        [SerializeField] private float bagShakeDuration;
        [SerializeField] float bagShakeStrength;
        [SerializeField] private float addFoodDuration;
        [SerializeField] private Ease addFoodEasing;
        
        public void Draw(Deck deck) {
            Transform tr = deck.transform.Find("BagImage");
            tr.localScale = Vector3.one * bagShakeStrength;
            tr.DOScale(Vector3.one, bagShakeDuration).SetEase(Ease.OutElastic);
        }

        public async UniTask AddFood(Deck deck, FoodObject food) {
            Transform tr = food.transform;
            tr.SetParent(deck.transform, true);
            tr.DOLocalMove(Vector3.zero, addFoodDuration).SetEase(addFoodEasing);
            await UniTask.Delay(TimeSpan.FromSeconds(addFoodDuration));
        }

        public void UpdateText(Deck deck) {
            Text remaining = deck.transform.Find("Remaining").GetComponent<Text>();
            Text all = deck.transform.Find("All").GetComponent<Text>();
            remaining.text = deck.SelectAll().Count.ToString();
            all.text = "/ " + deck.Count();
        } 
    }
}