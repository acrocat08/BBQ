using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BBQ.Tutorial {
    public class TutorialIntro : MonoBehaviour {
        [SerializeField] private List<TutorialParts> parts;
        [SerializeField] private Transform tako;
        [SerializeField] private TutorialPlayer player;


        private async void Start() {
            SoundPlayer.I.Play("se_island");
            await player.Play(parts, tako, null);
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            await SoundPlayer.I.FadeOutSound("se_island");
            SceneManager.LoadScene("Scenes/TutorialCooking");
        }
        
        
        
    }
}
