using System;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using SoundMgr;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BBQ.Result {
    
    public class Result : MonoBehaviour {
        [SerializeField] private bool isClear;
        [SerializeField] private TrueEnd trueend;
        [SerializeField] private BadEnd badend;


        private bool _isMoving;

        public void Start() {
            isClear = PlayerStatus.GetGameStatus() == 1;
            if (isClear) {
                trueend.Init(transform);
            }
            else {
                badend.Init(transform);
            }
        }

        public void Update() {
            if (!_isMoving && Input.GetMouseButtonDown(0)) {
                _isMoving = true;
                GotoTitle();
            }
        }
        
        private async void GotoTitle() {
            PlayerStatus.Reset();
            if (isClear) {
                await trueend.GotoTitle();
            }
            else {
                await badend.GotoTitle();
            }
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            SceneManager.LoadScene("Scenes/Title");
        }
    }
}