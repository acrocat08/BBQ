using System;
using System.Collections.Generic;
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

        async void Start() {
            SoundPlayer.I.Play("se_island");
            await player.Play(parts, tako, this);
            Next();
        }
        
        public void Receive(string signal) {
            
        }

        public async void Next() {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            await SoundPlayer.I.FadeOutSound("se_island");
            SceneManager.LoadScene("Scenes/Result");
        }
    }
}
