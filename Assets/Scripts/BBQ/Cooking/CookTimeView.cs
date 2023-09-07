using DG.Tweening;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Cooking {
    [CreateAssetMenu(menuName = "CookTime/View")]
    public class CookTimeView : ScriptableObject {
        [SerializeField] private float timeShakeStrength;
        [SerializeField] private float timeShakeDuration;
        [SerializeField] private Color[] textColor;

        public void UpdateText(CookTime time, bool bonusMode) {
            Text timeText = time.transform.Find("Text").GetComponent<Text>();
            timeText.text = time.GetNowTime().ToString();
            timeText.color = bonusMode ? textColor[1] : textColor[0];
            Text bonusText = time.transform.Find("Bonus").GetComponent<Text>();
            if (bonusMode) bonusText.enabled = false;
            bonusText.text = "+" + time.GetBonusTime();
        }

        public void UpdateTime(CookTime time, bool bonusMode) {
            Image timeImage = time.transform.Find("TimeImage").GetComponent<Image>();
            timeImage.transform.localScale = Vector3.one * timeShakeStrength;
            timeImage.transform.DOScale(Vector3.one, timeShakeDuration).SetEase(Ease.OutElastic);
            UpdateText(time, bonusMode);
        }
    }
}
