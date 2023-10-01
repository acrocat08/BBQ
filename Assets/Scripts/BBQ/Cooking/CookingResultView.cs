using System;
using System.Collections.Generic;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SoundMgr;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Cooking {
    [CreateAssetMenu(menuName = "CookingResult/View")]
    public class CookingResultView : ScriptableObject {

        [SerializeField] private GameObject missionPrefab;
        [SerializeField] private MissionSheetView missionSheetView;
        [SerializeField] private float durationA;
        [SerializeField] private float durationB;
        [SerializeField] private float durationC;
        [SerializeField] private float durationD;

        [SerializeField] private string clearText;
        [SerializeField] private string failedText;
        [SerializeField] private float shakeStrength;
        [SerializeField] private float shakeDuration;

        [SerializeField] private Sprite takoSpriteInClear;
        [SerializeField] private Sprite takoSpriteInFailed;

        public async UniTask ShowResult(Transform container, List<MissionStatus> missions, int star, int gainStar, int life, int lostLife, bool isClear) {
            Text starText = container.Find("Star").Find("Amount").GetComponent<Text>();
            starText.text = star + " / " + 10; //TODO:fix
            Text lifeText = container.Find("Life").Find("Amount").GetComponent<Text>();
            lifeText.text = life.ToString();
            await UniTask.Delay(TimeSpan.FromSeconds(durationA));
            SoundPlayer.I.Play("se_resultShow");
            container.gameObject.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(durationB));
            Transform missionList = container.Find("Mission").Find("Container");
            foreach (MissionStatus mission in missions) {
                Transform obj = Instantiate(missionPrefab).transform;
                obj.SetParent(missionList, false);
                obj.localScale = Vector3.one * 1.5f;
                missionSheetView.UpdateText(obj, mission, true);
                SoundPlayer.I.Play("se_resultShow");
                await UniTask.Delay(TimeSpan.FromSeconds(durationB));
            }
            await UniTask.Delay(TimeSpan.FromSeconds(durationC));
            Text resultText = container.Find("Text").GetComponent<Text>();
            if (isClear) {
                star += gainStar;
                resultText.text = clearText;
                SoundPlayer.I.Play("se_missionClear");
                container.Find("Tako").GetComponent<Image>().sprite = takoSpriteInClear;
            }
            else {
                life -= lostLife;
                resultText.text = failedText;
                SoundPlayer.I.Play("se_missionFailed");
                container.Find("Tako").GetComponent<Image>().sprite = takoSpriteInFailed;
            }
            resultText.enabled = true;
            await UniTask.Delay(TimeSpan.FromSeconds(durationD));
            starText.text = star + " / " + 10; //TODO:fix
            lifeText.text = life.ToString();
            SoundPlayer.I.Play(isClear ? "se_gainStar" : "se_lostLife");
            Image targetImage = container.Find(isClear ? "Star" : "Life")
                .Find(isClear ? "StarImage" : "LifeImage").GetComponent<Image>();
            targetImage.transform.localScale = Vector3.one * shakeStrength;
            targetImage.transform.DOScale(Vector3.one, shakeDuration).SetEase(Ease.OutElastic);
        }

    }
}
