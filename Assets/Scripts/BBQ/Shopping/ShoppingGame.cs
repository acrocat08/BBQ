using System;
using Cysharp.Threading.Tasks;
using SoundMgr;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BBQ.Shopping {
    public class ShoppingGame : MonoBehaviour {

        [SerializeField] private ShoppingGameView view;

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
            await view.OpenBG(this);
            SoundPlayer.I.Play("bgm_cooking");
        }

        async void GameEnd() {
            await view.CloseBG(this);
            await view.ChangeColor(this);
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            SceneManager.LoadScene("Scenes/Cooking");
            
        }

    }
}
