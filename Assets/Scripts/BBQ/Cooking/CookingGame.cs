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
        [SerializeField] private LoopManager loopManager;
        [SerializeField] private CookTime cookTime;
        [SerializeField] private HandCount handCount;
        [SerializeField] private MissionSheet missionSheet;
        [SerializeField] private Coin coin;
        [SerializeField] private Deck deck;
        [SerializeField] private Dump dump;
        [SerializeField] private CopyArea copyArea;

        [SerializeField] private List<DeckFood> testDeck;
        
        [SerializeField] private List<ActionCommand> startCommands;
        [SerializeField] private ActionAssembly assembly;
        [SerializeField] private CookingGameView view;

        private int _day;
        private bool _isRunning;
        private int _star;
        private int _life;
        private List<MissionStatus> _missions;

        [SerializeField] private List<MissionStatus> testMission;
        
        void Start() {
            Init();
        }

        public void Init() {
            _isRunning = false;
            LoadStatus();

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

            await UniTask.Delay(TimeSpan.FromSeconds(1));
            await view.CloseBG(this);

            bool isClear = missionSheet.CheckMissionCleared();
            int gainStar = isClear ? 1 : 0;
            int lostLife = isClear ? 0 : ((_day - 1) / 5) + 1;
            await view.GameEnd(transform.Find("Result"), _missions, _star, gainStar, _life, lostLife, isClear);
            _star += gainStar;
            _life -= lostLife;
            SaveStatus();
            await view.ChangeColor(this);
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            SceneManager.LoadScene("Scenes/Shopping");
        }


        private void LoadStatus() {
            List<DeckFood> targetDeck = PlayerStatus.GetDeckFoods();
            if(targetDeck == null) deck.Init(testDeck);
            else deck.Init(targetDeck);
            handCount.Init(PlayerStatus.GetHand());
            coin.Init(PlayerStatus.GetCoin());
            _day = PlayerStatus.GetDay();
            _missions = PlayerStatus.GetNowMission();
            if (_missions.Count == 0) _missions = testMission;
            missionSheet.Init(_missions);
            _star = PlayerStatus.GetStar();
            _life = PlayerStatus.GetLife();
            view.Init(this);
            cookTime.Init(60);      
            dump.Init();
            copyArea.Init();
            loopManager.Init();
            board.Init(lanes, dump, handCount, cookTime, missionSheet);
            env.Init(board, loopManager, deck, dump, copyArea, handCount, cookTime, coin, 0);
        }

        private void SaveStatus() {
            List<DeckFood> deckFoods = deck.GetUsableFoods();
            int coinNum = coin.GetCoin();
            PlayerStatus.Create(deckFoods, coinNum, 5, _day, PlayerStatus.GetShopLevel(), PlayerStatus.GetLevelUpDiscount(),
                env.rerollTicket, _star, _life, new List<MissionStatus>());
        }

        public int GetDay() {
            return _day;
        }
    }
}
