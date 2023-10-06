using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BBQ.Action;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.PlayData;
using BBQ.Test;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;

namespace BBQ.Tutorial {
    public class TutorialCooking : MonoBehaviour, IReceiver {
        [SerializeField] private TutorialPlayer player;
        [SerializeField] private List<TutorialParts> parts;
        
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
        [SerializeField] private List<MissionStatus> testMission;
        [SerializeField] private ActionAssembly assembly;


        [SerializeField] private Transform tako;

        [SerializeField] private List<TestDeck> decks;
        [SerializeField] private List<int> hands;
        [SerializeField] private List<int> draws;
        [SerializeField] private List<string> deckKeys;
        [SerializeField] private List<ActionCommand> startCommands;


        private bool _isRunning;
        private List<MissionStatus> _missions;
        private int _gameStatus;


        private string _nowPractice;
        private int _nowHandNum;


        async void Start() {
            Init();
            SoundPlayer.I.Play("bgm_tutorial");
            await player.Play(parts, tako, this);
        }

        void Init() {
            handCount.Init(5);
            coin.Init(0);
            carbon.Init(0);
            _missions = testMission;
            missionSheet.Init(_missions);
            cookTime.Init(1000);      
            dump.Init();
            copyArea.Init();
            loopManager.Init();
            help.Init(0);
            board.Init(lanes, dump, handCount, cookTime, missionSheet, env, this);
            env.Init(board, loopManager, deck, dump, copyArea, handCount, cookTime, coin, carbon, 0);
        }

        public async void Receive(string signal) {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            List<DeckFood> deckFoods = decks[deckKeys.IndexOf(signal)].foods;
            int hand = hands[deckKeys.IndexOf(signal)];
            int draw = draws[deckKeys.IndexOf(signal)];
            _nowPractice = signal;
            Practice(deckFoods, hand, draw);
        }

        async void Practice(List<DeckFood> foods, int handNum, int drawNum) {
            board.DiscardHand();
            deck.Init(foods, false);
            board.Reset();
            dump.Init();
            handCount.Init(handNum);
            coin.Init(0);
            InputGuard.UnLock();
            startCommands[0].n1 = drawNum.ToString();
            board.Resume();
            cookTime.Pause();
            _nowHandNum = 0;
            await assembly.Run(startCommands, env, null, null);
            cookTime.Resume();
        }

        public void SendHittingFoods(List<DeckFood> deckFoods) {
            if (deckFoods.Count > 0) _nowHandNum++;
            
            int status = 0;
            if (_nowPractice == "hitPractice") {
                status = HitPractice(deckFoods);
            }
            if (_nowPractice == "cornPractice") {
                status = CornPractice(deckFoods);
            }

            if (_nowPractice == "bekonPractice") {
                status = BekonPractice(deckFoods);
            }
            
            if (_nowPractice == "cabbagePractice") {
                status = CabbagePractice(deckFoods);
            }
            
            if (_nowPractice == "finalPractice") {
                status = FinalPractice(deckFoods);
            }

            if (status == 1) {
                player.Send("success");
                InputGuard.Lock();
                board.Pause();
            }
            
            if (status == 2) {
                player.Send("failed");
                InputGuard.Lock();
                board.Pause();
            }
        }

        private int HitPractice(List<DeckFood> deckFoods) {
            if (deckFoods.Any(x => x.data.foodName == "タコ足")) return 1;
            if (board.HasNoHand()) return 2;
            return 0;
        }
        
        private int CornPractice(List<DeckFood> deckFoods) {
            if (board.SelectAll().Count == 0) return 1;
            if (board.HasNoHand()) return 2;
            return 0;
        }
        
        private int BekonPractice(List<DeckFood> deckFoods) {
            if (_nowHandNum >= 3) return 1;
            if (board.HasNoHand()) return 2;
            return 0;
        }
        
        private int CabbagePractice(List<DeckFood> deckFoods) {
            if (_nowHandNum >= 1 && deckFoods.Count == 1 && deckFoods[0].data.foodName == "キャベツ") return 1;
            if (board.HasNoHand()) return 2;
            return 0;
        }
        
        private int FinalPractice(List<DeckFood> deckFoods) {
            if (_nowHandNum >= 4 && coin.GetCoin() >= 10) return 1;
            if (board.HasNoHand()) return 2;
            return 0;
        }
    }
}
