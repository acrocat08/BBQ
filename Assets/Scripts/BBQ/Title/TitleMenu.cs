using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using SoundMgr;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BBQ.Title {
    public class TitleMenu : MonoBehaviour {

        [SerializeField] private TitleMenuView view;
        [SerializeField] private Transform wave;
        
        [SerializeField] private List<Text> menuText;
        [SerializeField] private Transform smogContainer;
        [SerializeField] private ItemDictionary dictionary;
        [SerializeField] private ShopPoolList lineup;


        private bool _isMoving;
        
        private void Start() {
            _isMoving = false;
            Init();
        }

        private void Update() {
            int index = menuText.IndexOf(menuText.OrderBy(x => Mathf.Abs(x.transform.position.y - Input.mousePosition.y)).First());
            
            index = Mathf.Clamp(index, 0, menuText.Count - 1);
            view.UpdateText(menuText, index);
            if (Input.GetMouseButtonDown(0) && !_isMoving) {
                _isMoving = true;
                if (index == 0) GotoTutorial();
                if (index == 1) GotoMainGame();
                if (index == 2) OpenDictionary();
                if (index == 3) OpenLineup();
            }
        }




        void Init() {
            SoundPlayer.I.Play("bgm_title");
            view.FloatLogo(this);
            view.Wave(wave);
            view.Smog(transform, smogContainer);
        }
        
        private async void GotoMainGame() {
            SoundPlayer.I.Play("se_select1");
            await SoundPlayer.I.FadeOutSound("bgm_title");
            view.GotoNext();
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            SceneManager.LoadScene("Scenes/Shopping");
        }
        
        private async void GotoTutorial() {
            SoundPlayer.I.Play("se_select1");
            await SoundPlayer.I.FadeOutSound("bgm_title");
            view.GotoNext();
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            SceneManager.LoadScene("Scenes/TutorialIntro");
        }
        
        private void OpenDictionary() {
            _isMoving = true;
            dictionary.Open();
        }
        
        public void CloseDictionary() {
            _isMoving = false;
            dictionary.Close();
        }
        
        private void OpenLineup() {
            _isMoving = true;
            lineup.Open();
        }
        
        public void CloseLineup() {
            _isMoving = false;
            lineup.Close();
        }
        
        
        
    }
}
