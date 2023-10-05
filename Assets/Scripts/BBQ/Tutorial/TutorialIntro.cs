using System;
using System.Collections.Generic;
using SoundMgr;
using UnityEngine;

namespace BBQ.Tutorial {
    public class TutorialIntro : MonoBehaviour {
        [SerializeField] private List<TutorialParts> parts;
        [SerializeField] private Transform tako;
        [SerializeField] private TutorialPlayer player;


        private async void Start() {
            SoundPlayer.I.Play("se_island");
            await player.Play(parts, tako);
        }
        
        
        
    }
}
