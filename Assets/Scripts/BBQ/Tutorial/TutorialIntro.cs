using System;
using System.Collections.Generic;
using BBQ.Common;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SoundMgr;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BBQ.Tutorial {
    public class TutorialIntro : MonoBehaviour {
        [SerializeField] private List<TutorialParts> parts;
        [SerializeField] private Transform tako;
        [SerializeField] private TutorialPlayer player;
        [SerializeField] private GameObject introView;
        [SerializeField] private SceneTransition transition;


        private async void Start() {
            await transition.SceneStart();
            SoundPlayer.I.Play("se_island");
            await UniTask.Delay(TimeSpan.FromSeconds(3f));            
            CanvasGroup textArea = introView.transform.Find("TextArea").GetComponent<CanvasGroup>();
            DOTween.To(
                () => textArea.alpha,
                x => textArea.alpha = x,
                1f,
                0.5f);
            await UniTask.Delay(TimeSpan.FromSeconds(5f));            
            CanvasGroup canvasGroup = introView.GetComponent<CanvasGroup>();
            DOTween.To(
                () => canvasGroup.alpha,
                x => canvasGroup.alpha = x,
                0f,
                0.5f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));            
            await player.Play(parts, tako, null);
            await SoundPlayer.I.FadeOutSound("se_island");
            await transition.SceneEnd();
            SceneManager.LoadScene("Scenes/TutorialCooking");
        }
        
        
        
    }
}
