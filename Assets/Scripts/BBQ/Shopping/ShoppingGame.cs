using System;
using System.Collections.Generic;
using BBQ.Common;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BBQ.Shopping {
    public class ShoppingGame : MonoBehaviour {

        [SerializeField] private ShoppingGameView view;
        [SerializeField] private DeckInventory deckInventory;
        [SerializeField] private List<DeckFood> firstFoods;
        [SerializeField] private Shop shop;
        [SerializeField] private Coin coin;
        [SerializeField] private int income;


        void Start() {
            Init();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                GameEnd();
            }
        }

        void Init() {
            view.Init(this);
            shop.Init(coin);
            LoadStatus();
            GameStart();
        }

        async void GameStart() {
            await view.OpenBG(this);
            SoundPlayer.I.Play("bgm_cooking");
        }

        async void GameEnd() {
            SaveStatus();
            await view.CloseBG(this);
            await view.ChangeColor(this);
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            SceneManager.LoadScene("Scenes/Cooking");
        }

        private void LoadStatus() {
            List<DeckFood> targetDeck = PlayerStatus.GetDeckFoods();
            if(targetDeck != null) deckInventory.Init(targetDeck);
            else deckInventory.Init(firstFoods);
            coin.Init(PlayerStatus.GetCoin() + income);
        }
        private void SaveStatus() {
            List<DeckFood> deck = deckInventory.GetDeckFoods();
            int coinNum = coin.GetCoin();
            PlayerStatus.Create(deck, coinNum);
        }

    }
}
