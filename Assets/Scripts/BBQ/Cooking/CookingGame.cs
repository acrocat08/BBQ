using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Action;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using BBQ.Test;
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
        [SerializeField] private Carbon carbon;
        [SerializeField] private Deck deck;
        [SerializeField] private Dump dump;
        [SerializeField] private CopyArea copyArea;
        [SerializeField] private HelpAction help;

        [SerializeField] private TestDeck testDeck;
        
        [SerializeField] private List<ActionCommand> startCommands;
        [SerializeField] private ActionAssembly assembly;
        [SerializeField] private CookingGameView view;

        private int _day;
        private bool _isRunning;
        private int _star;
        private int _life;
        private List<MissionStatus> _missions;
        private int _gameStatus;

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
            SoundPlayer.I.Play("se_cookingEnd");

            await UniTask.Delay(TimeSpan.FromSeconds(1));
            await view.CloseBG(this);

            bool isClear = missionSheet.CheckMissionCleared();
            int gainStar = isClear ? 1 : 0;
            //int lostLife = isClear ? 0 : ((_day - 1) / 5) + 1;
            int lostLife = isClear ? 0 : 1;
            await view.GameEnd(transform.Find("Result"), _missions, _star, gainStar, _life, lostLife, isClear);
            _star += gainStar;
            _life -= lostLife;
            _gameStatus = CheckResult();
            
            SaveStatus();
            await view.ChangeColor(this);
            GotoNextScene();
        }

        int CheckResult() {
            if (_star >= 10) return 1;  //TODO:
            if (_life <= 0) return 2;
            return 0;
        }

        async void GotoNextScene() {
            if (_gameStatus > 0) {
                await SoundPlayer.I.FadeOutSound("bgm_cooking");
                SceneManager.LoadScene("Scenes/Result");
            }
            else {
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                SceneManager.LoadScene("Scenes/Shopping");
            }
        }


        private void LoadStatus() {
            List<DeckFood> targetDeck = PlayerStatus.GetDeckFoods();
            if(targetDeck == null) deck.Init(testDeck.foods.Select(x => x.CopyWithEffect()).ToList(), true);
            else deck.Init(targetDeck, true);
            handCount.Init(PlayerStatus.GetHand());
            coin.Init(PlayerStatus.GetCoin());
            carbon.Init(0);
            _day = PlayerStatus.GetDay();
            _missions = PlayerStatus.GetNowMission();
            if (_missions.Count == 0) _missions = testMission;
            missionSheet.Init(_missions);
            _star = PlayerStatus.GetStar();
            _life = PlayerStatus.GetLife();
            view.Init(this);
            cookTime.Init(60 + PlayerStatus.GetadditionalTime());      
            dump.Init();
            copyArea.Init();
            loopManager.Init();
            help.Init(PlayerStatus.GetHelpPenaltyReduce());
            board.Init(lanes, dump, handCount, cookTime, missionSheet, env, null);
            env.Init(board, loopManager, deck, dump, copyArea, handCount, cookTime, coin, carbon, 0);
        }

        private void SaveStatus() {
            List<DeckFood> deckFoods = deck.GetUsableFoods();
            int coinNum = coin.GetCoin();
            PlayerStatus.Create(deckFoods, coinNum, 5, carbon.GetCarbon(), _day, PlayerStatus.GetShopLevel(), PlayerStatus.GetLevelUpDiscount(),
                env.rerollTicket, 0, 0, _star, _life, new List<MissionStatus>(), _gameStatus);
        }

        public int GetDay() {
            return _day;
        }
    }
}
