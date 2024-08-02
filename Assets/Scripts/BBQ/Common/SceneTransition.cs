using System;
using BBQ.Database;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BBQ.Common {
    public class SceneTransition : MonoBehaviour {
        [SerializeField] private Image image;
        [SerializeField] private float duration;
        [SerializeField] private ItemSet itemSet;
        private static readonly int Transition = Shader.PropertyToID("_Transition");
        private static readonly int Rule = Shader.PropertyToID("_Rule");

        public async UniTask SceneEnd() {
            transform.localScale = Vector3.one;
            image.material.SetFloat(Transition, 1f);
            //image.material.SetTexture(Rule, itemSet.foods[Random.Range(0, itemSet.foods.Count)].foodImage.texture);
            DOTween.To(
                () => image.material.GetFloat(Transition),
                x => { image.material.SetFloat(Transition, x); },
                0f, duration).SetEase(Ease.InCubic);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
        
        public async UniTask SceneStart() {
            transform.localScale = Vector3.one;
            image.material.SetFloat(Transition, 0f);
            //image.material.SetTexture(Rule, itemSet.foods[Random.Range(0, itemSet.foods.Count)].foodImage.texture);
            DOTween.To(
                () => image.material.GetFloat(Transition),
                x => { image.material.SetFloat(Transition, x); },
                1f, duration).SetEase(Ease.OutCubic);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            transform.localScale = Vector3.zero;
        }
    }
}
