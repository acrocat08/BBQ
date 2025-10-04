using System;
using System.Collections.Generic;
using BBQ.Common;
using BBQ.Tutorial;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BBQ.Result {
    public class Ending : MonoBehaviour, IReceiver {

        [SerializeField] private TutorialPlayer player;
        [SerializeField] private List<TutorialParts> parts;
        [SerializeField] private Transform tako;
        [SerializeField] private SceneTransition transition;

        async void Start() {
            await transition.SceneStart();
            SoundPlayer.I.Play("se_island");
            await player.Play(parts, tako, this);
            Next();
        }
        
        public void Receive(string signal) {
            
        }

        public async void Next() {
            await SoundPlayer.I.FadeOutSound("se_island");
            await transition.SceneEnd();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            SceneManager.LoadScene("Scenes/Result");
        }
    }
}
