using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Action;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.Database;
using BBQ.PlayData;
using BBQ.Shopping;
using BBQ.Test;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BBQ.Tutorial {
    public class TutorialShopping : MonoBehaviour, IReceiver {

        [SerializeField] private DeckInventory deckInventory;
        [SerializeField] private CopyArea copyArea;
        [SerializeField] private List<DeckFood> firstFoods;
        [SerializeField] private Shop shop;
        [SerializeField] private Coin coin;
        [SerializeField] private Carbon carbon;
        [SerializeField] private Life life;
        [SerializeField] private HandCount handCount;
        [SerializeField] private ActionEnvironment env;
        
        [SerializeField] private TutorialPlayer player;
        [SerializeField] private List<TutorialParts> parts;
        [SerializeField] private Transform tako;
        
        private int _day;

        private string _nowAction;
        async void Start() {
            Init();
            SoundPlayer.I.Play("bgm_tutorial");
            await player.Play(parts, tako, this);
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            await SoundPlayer.I.FadeOutSound("bgm_tutorial");
            SceneManager.LoadScene("Scenes/Shopping");
        }
        
        void Init() {
            deckInventory.Init(firstFoods);
            coin.Init(100);
            carbon.Init(1);
            handCount.Init(5);
            shop.Init(1, 0, coin, carbon, 0, this);
            copyArea.Init();
            life.Init(6);
            env.Init(shop, handCount, coin, carbon, life, deckInventory, copyArea);
        }

        public void Receive(string signal) {
            _nowAction = signal;
            if(_nowAction == "reroll") shop.SetRerollerMode(true);
            
            InputGuard.UnLock();
            
            
        }

        public void BuyFoods() {
            if (_nowAction == "buyFoods") {
                InputGuard.Lock();
                player.Send("success");
            }
        }

        public async void Reroll() {
            if (_nowAction == "reroll") {
                InputGuard.Lock();
                shop.SetRerollerMode(false);
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                player.Send("success");
            }
        }

        public void Merge() {
            if (_nowAction == "merge") {
                InputGuard.Lock();
                player.Send("success");
            }
        }
    }
}
