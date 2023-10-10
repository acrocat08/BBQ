using BBQ.Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "Life/View")]
    public class LifeView : ScriptableObject {
        
        [SerializeField] private float shakeDuration;
        [SerializeField] private float shakeStrength;
        public void UpdateText(Life life) {
            Text coinText = life.transform.Find("Amount").GetComponent<Text>();
            coinText.text = life.GetLife().ToString();
        }

        public void AddLife(Life life) {
            Transform tr = life.transform;
            Image handImage = tr.Find("LifeImage").GetComponent<Image>();
            handImage.transform.localScale = Vector3.one * shakeStrength;
            handImage.transform.DOScale(Vector3.one, shakeDuration).SetEase(Ease.OutElastic);
            UpdateText(life);
        }

    }
}