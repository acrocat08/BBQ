using System;
using System.Collections.Generic;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using SoundMgr;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BBQ.Shopping {
    public class ShoppingGame : MonoBehaviour {

        [SerializeField] private ShoppingGameView view;
        [SerializeField] private DeckInventory deckInventory;
        [SerializeField] private List<DeckFood> firstFoods;


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
            GameStart();
        }

        async void GameStart() {
            List<DeckFood> targetDeck = PlayerStatus.GetDeckFoods();
            if(targetDeck != null) deckInventory.OnShopStart(targetDeck);
            else deckInventory.OnShopStart(firstFoods);
            
            await view.OpenBG(this);
            SoundPlayer.I.Play("bgm_cooking");
        }

        async void GameEnd() {
            deckInventory.OnShopEnd();
            await view.CloseBG(this);
            await view.ChangeColor(this);
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            SceneManager.LoadScene("Scenes/Cooking");
            
        }

    }
}
