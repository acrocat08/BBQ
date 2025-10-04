using System;
using System.Collections.Generic;
using BBQ.Database;
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

        [SerializeField] private GameObject kushiPrefab;
 
        public async UniTask ShowResult(Transform container, List<MissionStatus> missions, int star, int gainStar,
            int life, int lostLife, bool isClear, List<Tuple<FoodData, FoodData, FoodData>> kushi) {
            Text starText = container.Find("Star").Find("Amount").GetComponent<Text>();
            starText.text = star + " / " + 10; //TODO:fix
            Text lifeText = container.Find("Life").Find("Amount").GetComponent<Text>();
            lifeText.text = life.ToString();
            
            Transform missionList = container.Find("Mission");
            missionSheetView.UpdateText(missionList, missions[0], false);
            missionList.Find("OK").GetComponent<Image>().enabled = false;
            Text nowCount = missionList.Find("NowNum").GetComponent<Text>();
            nowCount.text = "0";
            
            await UniTask.Delay(TimeSpan.FromSeconds(durationA));
            SoundPlayer.I.Play("se_resultShow");
            container.gameObject.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(durationB));
            await ShowKushiList(container, kushi, nowCount);
            await UniTask.Delay(TimeSpan.FromSeconds(durationC));

            missionSheetView.UpdateText(missionList, missions[0], true);
            /*
            foreach (MissionStatus mission in missions) {
                Transform obj = Instantiate(missionPrefab).transform;
                obj.SetParent(missionList, true);
                obj.localScale = Vector3.one * 1.5f;
                missionSheetView.UpdateText(obj, mission, true);
                //SoundPlayer.I.Play("se_resultShow");
                //await UniTask.Delay(TimeSpan.FromSeconds(durationB));
            }
            */
            Text resultText = container.Find("Text").GetComponent<Text>();
            if (isClear) {
                star += gainStar;
                resultText.text = clearText;
                SoundPlayer.I.Play("se_missionClear");
                Transform tako = container.Find("Tako");
                tako.GetComponent<Image>().sprite = takoSpriteInClear;
                TakoJump(tako);
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

        async void TakoJump(Transform tako) {
            tako.DOLocalJump(tako.transform.localPosition, 100f, 1, 0.3f).SetEase(Ease.Linear);
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
            tako.DOLocalJump(tako.transform.localPosition, 50f, 1, 0.3f).SetEase(Ease.Linear);
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
            tako.DOLocalJump(tako.transform.localPosition, 25f, 1, 0.3f).SetEase(Ease.Linear);
        }

        private async UniTask ShowKushiList(Transform container, List<Tuple<FoodData, FoodData, FoodData>> kushi, Text count) {
            Transform kushiContainer = container.Find("KushiContainer");
            float kushiDuration = 0;
            if (kushi.Count > 0) kushiDuration = 2f / kushi.Count;
            if (kushi.Count > 24) {
                float moveLength = 150f * ((kushi.Count - 25) / 4 + 1);
                kushiContainer.transform.DOLocalMoveY(kushiContainer.transform.localPosition.y + moveLength, 2f)
                    .SetEase(Ease.Linear);
            }

            int cnt = 0;
            foreach (Tuple<FoodData,FoodData,FoodData> tuple in kushi) {
                cnt++;
                count.text = cnt.ToString();
                GameObject obj = DrawKushi(tuple);
                obj.transform.SetParent(kushiContainer, false);
                SoundPlayer.I.Play("se_addHand");
                await UniTask.Delay(TimeSpan.FromSeconds(kushiDuration));
            }
        }

        private GameObject DrawKushi(Tuple<FoodData, FoodData, FoodData> tuple) {
            GameObject obj = Instantiate(kushiPrefab);
            if (tuple.Item1) obj.transform.Find("Food (1)").GetComponent<Image>().sprite = tuple.Item1.foodImage;
            else obj.transform.Find("Food (1)").GetComponent<Image>().enabled = false;
            if (tuple.Item2) obj.transform.Find("Food (2)").GetComponent<Image>().sprite = tuple.Item2.foodImage;
            else obj.transform.Find("Food (2)").GetComponent<Image>().enabled = false;
            if (tuple.Item3) obj.transform.Find("Food (3)").GetComponent<Image>().sprite = tuple.Item3.foodImage;
            else obj.transform.Find("Food (3)").GetComponent<Image>().enabled = false;
            return obj;
        }

    }
}
