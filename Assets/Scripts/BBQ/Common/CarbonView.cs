using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Common {
    [CreateAssetMenu(menuName = "Carbon/View")]
    public class CarbonView : ScriptableObject {
        
        [SerializeField] private float coinShakeDuration;
        [SerializeField] private float coinShakeStrength;
        public void UpdateText(Carbon carbon) {
            Text coinText = carbon.transform.Find("Amount").GetComponent<Text>();
            coinText.text = carbon.GetCarbon().ToString();
        }

        public void AddCarbon(Carbon carbon) {
            Transform tr = carbon.transform;
            Image handImage = tr.Find("CarbonImage").GetComponent<Image>();
            handImage.transform.localScale = Vector3.one * coinShakeStrength;
            handImage.transform.DOScale(Vector3.one, coinShakeDuration).SetEase(Ease.OutElastic);
            
            UpdateText(carbon);
        }

    }
}