using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Action;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.Database;
using BBQ.PlayData;
using BBQ.Test;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BBQ.Shopping {
    public class ShoppingGame : MonoBehaviour {

        [SerializeField] private ShoppingGameView view;
        [SerializeField] private DeckInventory deckInventory;
        [SerializeField] private CopyArea copyArea;
        [SerializeField] private List<DeckFood> firstFoods;
        [SerializeField] private Shop shop;
        [SerializeField] private Coin coin;
        [SerializeField] private Carbon carbon;
        [SerializeField] private Life life;
        [SerializeField] private HandCount handCount;
        [SerializeField] private int income;
        [SerializeField] private MissionMaker missionMaker;
        [SerializeField] private ActionEnvironment env;
        [SerializeField] private DesignParam param;
        [SerializeField] private TestDeck testDeck;
        
        private int _day;
        private List<MissionStatus> _nowMission;
        private bool _isEnd;
        void Start() {
            Init();
            _isEnd = false;
        }
        
        void Init() {
            LoadStatus();
            env.Init(shop, handCount, coin, carbon, life, deckInventory, copyArea);
            view.Init(this);
            _nowMission = missionMaker.Create(_day, PlayerStatus.GetFailed());
            view.UpdateMission(this, _nowMission);
            GameStart();
        }

        async void GameStart() {
            await view.OpenBG(this);
            await TriggerObserver.I.Invoke(ActionTrigger.StartShopping, new List<DeckFood>(), false);
            SoundPlayer.I.Play("bgm_cooking");
        }

        public async void GameEnd() {
            if (_isEnd) return;
            _day += 1;
            _isEnd = true;
            SaveStatus();
            await view.CloseBG(this);
            SoundPlayer.I.Play("se_morning");
            await view.ChangeColor(this);
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            SceneManager.LoadScene("Scenes/Cooking");
        }

        private void LoadStatus() {
            _day = PlayerStatus.GetDay();
            List<DeckFood> targetDeck = PlayerStatus.GetDeckFoods();
            if(targetDeck != null) deckInventory.Init(targetDeck);
            else if(param.isDebugMode) deckInventory.Init(testDeck.foods.Select(x => x.CopyWithEffect()).ToList());
            else deckInventory.Init(firstFoods);
            int shopLevel = param.isDebugMode ?  1 : PlayerStatus.GetShopLevel();
            int nowIncome = Mathf.Max(0, GetDayIncome()); 
            coin.Init(param.isDebugMode ?  10000 : PlayerStatus.GetCoin() + nowIncome);
            carbon.Init(param.isDebugMode ?  100 : PlayerStatus.GetCarbon() + (_day - 1) / 5 + 1);
            handCount.Init(5);
            shop.Init(shopLevel,PlayerStatus.GetLevelUpDiscount(), coin, carbon, PlayerStatus.GetRerollTicket(), null);
            copyArea.Init();
            int star = PlayerStatus.GetStar();
            life.Init(PlayerStatus.GetLife());
            view.SetStatus(this, star);
        }
        private void SaveStatus() {
            List<DeckFood> deck = deckInventory.GetDeckFoods();
            int coinNum = coin.GetCoin();
            int hand = handCount.GetHandCount();
            PlayerStatus.Create(deck, coinNum, hand, 0, _day, shop.GetShopLevel(), shop.GetLevelUpDiscount() + 10, 0,
                deckInventory.GetAdditionalTime(), deckInventory.GetHelpPenaltyReduce(), PlayerStatus.GetStar(), life.GetLife(), _nowMission, PlayerStatus.GetFailed(), 0, PlayerStatus.GetScore());
        }

        public int GetDay() {
            return _day;
        }

        public int GetDayIncome() {
            int dayIncome = income;
            //if (_day > 1) dayIncome /= 2;
            return dayIncome;
        }

    }
}
