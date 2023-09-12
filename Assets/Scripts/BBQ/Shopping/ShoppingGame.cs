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

        private int _day;

        void Start() {
            Init();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                GameEnd();
            }
        }

        void Init() {
            LoadStatus();
            view.Init(this);
            GameStart();
        }

        async void GameStart() {
            await view.OpenBG(this);
            SoundPlayer.I.Play("bgm_cooking");
        }

        async void GameEnd() {
            _day += 1;
            SaveStatus();
            await view.CloseBG(this);
            await view.ChangeColor(this);
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            SceneManager.LoadScene("Scenes/Cooking");
        }

        private void LoadStatus() {
            _day = PlayerStatus.GetDay();
            List<DeckFood> targetDeck = PlayerStatus.GetDeckFoods();
            firstFoods.ForEach(x => Debug.Log(x.lank));

            if(targetDeck != null) deckInventory.Init(targetDeck);
            else deckInventory.Init(firstFoods);

            int shopLevel = PlayerStatus.GetShopLevel();
            int nowIncome = Mathf.Max(0, income - (_day - 1) * 10); 
            
            coin.Init(PlayerStatus.GetCoin() + nowIncome);
            shop.Init(shopLevel,PlayerStatus.GetLevelUpDiscount(), coin);
        }
        private void SaveStatus() {
            List<DeckFood> deck = deckInventory.GetDeckFoods();
            int coinNum = coin.GetCoin();
            PlayerStatus.Create(deck, coinNum, _day, shop.GetShopLevel(), shop.GetLevelUpDiscount() + 10);
        }

        public int GetDay() {
            return _day;
        }

    }
}
