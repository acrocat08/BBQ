using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Common {
    [CreateAssetMenu(menuName = "Coin/View")]
    public class CoinView : ScriptableObject {
        
        [SerializeField] private float coinShakeDuration;
        [SerializeField] private float coinShakeStrength;
        public void UpdateText(Coin coin) {
            Text coinText = coin.transform.Find("Amount").GetComponent<Text>();
            coinText.text = coin.GetCoin().ToString();
        }

        public void AddCoin(Coin coin) {
            Transform tr = coin.transform;
            Image handImage = tr.Find("CoinImage").GetComponent<Image>();
            handImage.transform.localScale = Vector3.one * coinShakeStrength;
            handImage.transform.DOScale(Vector3.one, coinShakeDuration).SetEase(Ease.OutElastic);
            
            UpdateText(coin);
        }

    }
}
