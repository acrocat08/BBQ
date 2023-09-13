using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Action;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using SoundMgr;
using Unity.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace BBQ.Cooking {
    public class CookingGame : MonoBehaviour {

        [SerializeField] private ActionEnvironment env;
        [SerializeField] private Board board;
        [SerializeField] private List<Lane> lanes;
        [SerializeField] private CookTime cookTime;
        [SerializeField] private HandCount handCount;
        [SerializeField] private Coin coin;
        [SerializeField] private Deck deck;
        [SerializeField] private Dump dump;

        [SerializeField] private List<DeckFood> testDeck;
        
        [SerializeField] private List<ActionCommand> startCommands;
        [SerializeField] private ActionAssembly assembly;
        [SerializeField] private CookingGameView view;

        private int _day;
        private bool _isRunning;
        
        void Start() {
            Init();
        }

        public void Init() {
            _isRunning = false;
            LoadStatus();
            view.Init(this);
            cookTime.Init(60);      
            dump.Init();
            board.Init(lanes, dump, handCount, cookTime);
            env.Init(board, lanes, deck, dump, handCount, cookTime, coin);
            cookTime.Pause();
            GameStart();
        }

        private async void GameStart() {
            await view.OpenBG(this);
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            _isRunning = true;
            SoundPlayer.I.Play("bgm_cooking");
            await assembly.Run(startCommands, env, null, null);
            cookTime.Resume();
        }

        public async void GameEnd() {
            cookTime.Pause();
            board.DiscardHand();
            SoundPlayer.I.Play("se_cookingEnd");
            SaveStatus();
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            await view.CloseBG(this);
            await view.ChangeColor(this);
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            SceneManager.LoadScene("Scenes/Shopping");
        }


        private void LoadStatus() {
            List<DeckFood> targetDeck = PlayerStatus.GetDeckFoods();
            if(targetDeck == null) deck.Init(testDeck);
            else deck.Init(targetDeck);
            handCount.Init(5);
            coin.Init(PlayerStatus.GetCoin());
            _day = PlayerStatus.GetDay();
        }

        private void SaveStatus() {
            List<DeckFood> deckFoods = deck.GetUsableFoods();
            int coinNum = coin.GetCoin();
            PlayerStatus.Create(deckFoods, coinNum, _day, PlayerStatus.GetShopLevel(), PlayerStatus.GetLevelUpDiscount());
        }

        public int GetDay() {
            return _day;
        }
    }
}
