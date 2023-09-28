using System;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BBQ.Title {
    public class TitleMenu : MonoBehaviour {

        [SerializeField] private TitleMenuView view;
        [SerializeField] private Transform wave;
        

        private bool _isMoving;
        
        private void Start() {
            _isMoving = false;
            Init();
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0) && !_isMoving) {
                _isMoving = true;
                GotoMainGame();
            }
        }



        void Init() {
            SoundPlayer.I.Play("bgm_title");
            view.FloatLogo(this);
            view.Wave(wave);
            view.Smog(transform);
        }
        
        private async void GotoMainGame() {
            SoundPlayer.I.Play("se_select1");
            await SoundPlayer.I.FadeOutSound("bgm_title");
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            view.GotoNext();
            SceneManager.LoadScene("Scenes/Shopping");
        }
        
    }
}
