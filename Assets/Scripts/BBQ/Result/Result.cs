using System;
using System.Collections.Generic;
using System.Linq;
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


        private bool _isMoving;

        public async void Start() {
            DrawInventory();
            int score = PlayerStatus.GetScore() / PlayerStatus.GetDay();
            UnityroomApiClient.Instance.SendScore(1, score, ScoreboardWriteMode.HighScoreDesc);
            scoreText.text = "Score:    " + score;
            isClear = PlayerStatus.GetGameStatus() == 1;
            if (isClear) {
                trueend.Init(transform);
            }
            else {
                badend.Init(transform);
            }
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

        public void Update() {
            if (!_isMoving && Input.GetMouseButtonDown(0)) {
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
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            SceneManager.LoadScene("Scenes/Title");
        }

        void DrawInventory() {
            List<DeckFood> deckFoods = PlayerStatus.GetDeckFoods();
            if (deckFoods == null) return;
            deckFoods = deckFoods.Take(foodList.Count).OrderBy(x => itemSet.GetFoodIndex(x.data)).ToList();
            for (int i = 0; i < deckFoods.Count; i++) {
                DrawFood(foodList[i], deckFoods[i]);
            }
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
        }
    }
}