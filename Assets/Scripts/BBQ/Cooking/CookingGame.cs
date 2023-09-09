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
        [SerializeField] private ActionRegister actionRegister;
        
        [SerializeField] private List<ActionCommand> startCommands;
        [SerializeField] private ActionAssembly assembly;
        [SerializeField] private CookingGameView view;
        
        private bool _isRunning;
        
        void Start() {
            Init();
        }

        public void Init() {
            _isRunning = false;
            cookTime.Init(60);      
            handCount.Init(5);
            coin.Init(0);

            List<DeckFood> targetDeck = PlayerStatus.GetDeckFoods();
            if(targetDeck == null) deck.Init(testDeck);
            else deck.Init(targetDeck);
            
            dump.Init();
            board.Init(lanes, dump, handCount, cookTime);
            foreach (DeckFood food in targetDeck) {
                actionRegister.Add(food);
            }
            env.Init(board, lanes, deck, dump, handCount, cookTime, coin);
            cookTime.Pause();
            view.Init(this);
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
            await view.ChangeColor(this);
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            SceneManager.LoadScene("Scenes/Shopping");
        }
    }
}
