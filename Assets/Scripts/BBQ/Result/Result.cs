using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SoundMgr;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using unityroom.Api;
using Random = UnityEngine.Random;

namespace BBQ.Result {
    
    public class Result : MonoBehaviour {
        [SerializeField] private bool isClear;
        [SerializeField] private TrueEnd trueend;
        [SerializeField] private BadEnd badend;

        [SerializeField] private List<Transform> foodList;
        [SerializeField] private ItemSet itemSet;
        [SerializeField] private List<Color> lankColor;
        [SerializeField] private Text scoreText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private SceneTransition transition;
        [SerializeField] private List<Material> lankMaterial;
        [SerializeField] private Text dayText;
        [SerializeField] private GameObject skinPrefab;
        [SerializeField] private Text scoreDetail;
        [SerializeField] private CanvasGroup detailCanvas;

        [SerializeField] private Transform expContainer;
        
        

        private static readonly int Seed = Shader.PropertyToID("_seed");


        private bool _isMoving;
        private int _nowDay;
        private Score _score;

        public async void Start() {
            DrawInventory(PlayerStatus.GetDay() - 1);

            _score = CalcScore();
            int score = _score.GetSum();
            scoreDetail.text =
                $"{_score.basePoint}\n{_score.difficulty}\n{_score.mission}\n{_score.life}\n{_score.great}\n{_score.help}\n{_score.shopping}";
            if (PlayerConfig.GetPoolIndex() <= 8) {
                UnityroomApiClient.Instance.SendScore(1, score, ScoreboardWriteMode.HighScoreDesc);
            }
            scoreText.text = "Score:    " + score;
            if (PlayerConfig.GetPoolIndex() <= 8) {
                string scoreName = "score_" + (int)PlayerConfig.GetGameMode() + "_" + PlayerConfig.GetPoolIndex();
                int nowScore = PlayerPrefs.GetInt(scoreName, 0);
                if(score > nowScore) PlayerPrefs.SetInt(scoreName, score);
            }
            
            isClear = PlayerStatus.GetGameStatus() == 1;
            if (isClear) {
                trueend.Init(transform);
            }
            else {
                badend.Init(transform);
            }
            await transition.SceneStart();
            _isMoving = true;
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            DOTween.To(
                () => canvasGroup.alpha,
                x => canvasGroup.alpha = x,
                1f,
                1f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            _isMoving = false;
        }

        public void End() {
            if (!_isMoving) {
                _isMoving = true;
                GotoTitle();
            }
        }
        
        
        private async void GotoTitle() {
            PlayerStatus.Reset();
            if (isClear) {
                await trueend.GotoTitle();
            }
            else {
                await badend.GotoTitle();
            }
            await transition.SceneEnd();
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            SceneManager.LoadScene("Scenes/Title");
        }

        void DrawInventory(int day) {
            _nowDay = day;
            if (PlayerStatus.GetDeckFoods().Count == 0) return;
            List<DeckFood> deckFoods = PlayerStatus.GetDeckFoods()[2 * _nowDay - 2];
            if (deckFoods == null) return;
            deckFoods = deckFoods.Take(foodList.Count).OrderBy(x => itemSet.GetFoodIndex(x.data)).ToList();
            DeckFood empty = new(null);
            for (int i = 0; i < foodList.Count; i++) {
                DrawFood(foodList[i], empty);
            }
            for (int i = 0; i < deckFoods.Count; i++) {
                DrawFood(foodList[i], deckFoods[i]);
            }
            dayText.text = "Day " + _nowDay;
        }

        public void ShowNextInventory(int dir) {
            SoundPlayer.I.Play("se_changeMode");
            int maxDay = PlayerStatus.GetDay() - 1;
            DrawInventory((_nowDay + dir - 1 + maxDay) % maxDay + 1);
        }


        private void DrawFood(Transform container, DeckFood deckFood) {
            Image foodImage = container.Find("FoodImage").GetComponent<Image>();
            foodImage.sprite = deckFood.data ? deckFood.data.foodImage : null;
            foodImage.enabled = deckFood.data != null;
            foodImage.color = Color.white;
            Image lankImage = container.Find("Lank").GetComponent<Image>();
            lankImage.color = deckFood.data != null ? lankColor[deckFood.lank - 1] : Color.clear;
            FoodEffect effect = deckFood.effect;
            if (effect == null) {
                Transform frame = container.Find("Effect");
                frame.localScale = Vector3.zero;
            }
            else {
                Transform frame = container.Find("Effect");
                frame.localScale = Vector3.one;
                Image icon = container.Find("Effect").Find("Icon").GetComponent<Image>();
                icon.sprite = deckFood.effect.effectImage;
            }
            SetMaterial(foodImage, deckFood.lank);
        }
        
        void SetMaterial(Image foodImage, int lank) {
            Material mat = lankMaterial[lank - 1];
            if(mat != null) foodImage.material = new(mat);
            if(foodImage.material != null) foodImage.material.SetFloat(Seed, Random.value);
        }

        public void ShowExpContainer() {
            AddExp();
        }

        async void AddExp() {
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            canvasGroup.transform.localScale = Vector3.zero;
            expContainer.GetComponent<CanvasGroup>().alpha = 1;
            ExpGage exp = expContainer.Find("Exp").GetComponent<ExpGage>();
            exp.Init(PlayerPrefs.GetInt("expLevel", 0), PlayerPrefs.GetInt("expPoint", 0));
            expContainer.Find("Score").GetComponent<Text>().text = "Exp +" + _score.GetSum();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            int prevLevel = exp.GetLevel();
            await exp.GainExp(_score.GetSum());
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            int nowLevel = exp.GetLevel();
            if (nowLevel - prevLevel > 0) {
                expContainer.Find("NewSkin").GetComponent<CanvasGroup>().alpha = 1;
                SoundPlayer.I.Play("se_merge");
                for (int i = 0; i < nowLevel - prevLevel; i++) {
                    FindNewSkin();
                }
            }
            expContainer.Find("End").localScale = Vector3.one;
            PlayerPrefs.SetInt("expLevel", exp.GetLevel());
            PlayerPrefs.SetInt("expPoint", exp.GetPoint());
            UnityroomApiClient.Instance.SendScore(2, nowLevel, ScoreboardWriteMode.HighScoreDesc);

        }

        void FindNewSkin() {
            Transform container = expContainer.Find("NewSkin").Find("Skins");
            int targetIndex = UnlockSkin();
            GameObject skin = Instantiate(skinPrefab, container);
            Image skinImage = skin.transform.Find("Image").GetComponent<Image>();
            skinImage.sprite = itemSet.foods[targetIndex].cosplayImage;
            skinImage.transform.localScale = Vector3.one * 1.5f;
            skinImage.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InBack);
        }

        int UnlockSkin() {
            string skinList = PlayerPrefs.GetString("unlockedSkin", "");
            List<string> unlocked = skinList.Split(",").Where(x => x != "").ToList();
            if (unlocked.Count > 0 && unlocked[0].All(char.IsDigit)) {
                unlocked = unlocked.Select(int.Parse).Select(x => (x / 25) * 30 + (x % 25))
                    .Select(x => itemSet.foods[x].foodName).ToList();
                PlayerPrefs.SetString("unlockedSkin", string.Join(",", unlocked) + ",");
                skinList = PlayerPrefs.GetString("unlockedSkin", "");
            }
            List<int> locked = new();
            for (int i = 0; i < itemSet.foods.Count; i++) {
                if(!unlocked.Contains(itemSet.foods[i].foodName)) locked.Add(i);
            }
            int target = locked[Random.Range(0, locked.Count)];
            PlayerPrefs.SetString("unlockedSkin", skinList + itemSet.foods[target].foodName + ",");
            return target;
        }

        private Score CalcScore() {
            Score score = PlayerStatus.GetScore();
            score.basePoint = 50;
            if (PlayerConfig.GetGameMode() == GameMode.normal) score.difficulty = 25;
            if (PlayerConfig.GetGameMode() == GameMode.hard) score.difficulty = 50;
            score.mission = PlayerStatus.GetStar() * 5;
            score.life = PlayerStatus.GetLife() * 10;
            score.great = Mathf.Min(50, score.great);
            score.help = -Mathf.Min(50, score.help / 3);
            score.shopping = (PlayerStatus.GetShopLevel() - 1) * 5;
            List<DeckFood> deckFoods = PlayerStatus.GetDeckFoods()[PlayerStatus.GetDeckFoods().Count - 1];
            int count = deckFoods.Where(x => x.data).Select(x => x.data).Distinct().Count();
            score.shopping += Mathf.Min(30, count * 2);
            return score;
        }

        public void ShowDetail() {
            detailCanvas.alpha = (detailCanvas.alpha + 1) % 2;
        }
    }
}