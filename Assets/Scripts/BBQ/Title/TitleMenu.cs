using System;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BBQ.Title {
    public class TitleMenu : MonoBehaviour {

        [SerializeField] private TitleMenuView view;

        private void Start() {
            Init();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                GotoMainGame();
            }
        }



        void Init() {
            SoundPlayer.I.Play("bgm_title");
            view.FloatLogo(this);
        }
        
        private async void GotoMainGame() {
            SoundPlayer.I.Play("se_select1");
            await SoundPlayer.I.FadeOutSound("bgm_title");
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            SceneManager.LoadScene("Scenes/Shopping");
        }
        
    }
}
