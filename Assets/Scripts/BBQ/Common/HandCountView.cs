using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Common {
    [CreateAssetMenu(menuName = "HandCount/View")]
    public class HandCountView : ScriptableObject {
        [SerializeField] private float handShakeDuration;
        [SerializeField] private float handShakeStrength;

        public void UpdateText(HandCount handCount) {
            Text handText = handCount.transform.Find("Remaining").GetComponent<Text>();
            handText.text = handCount.GetHandCount().ToString();
        }

        public void AddHand(HandCount handCount) {
            Transform tr = handCount.transform;
            Image handImage = tr.Find("HandImage").GetComponent<Image>();
            handImage.transform.localScale = Vector3.one * handShakeStrength;
            handImage.transform.DOScale(Vector3.one, handShakeDuration).SetEase(Ease.OutElastic);
            
            UpdateText(handCount);
        }

    }
}
