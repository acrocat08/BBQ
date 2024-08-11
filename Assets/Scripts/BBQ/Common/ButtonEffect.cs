using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Common {
    public class ButtonEffect : MonoBehaviour {
        [SerializeField] private Color clickedColor;
        
        public async void OnClicked() {
            Image image = GetComponent<Image>();
            Color prevColor = image.color;
            image.color = clickedColor;
            image.DOColor(prevColor, 0.5f).SetEase(Ease.OutQuad);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            image.color = prevColor;
        }
    }
}
