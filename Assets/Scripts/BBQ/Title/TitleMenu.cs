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
        [SerializeField] private List<string> modeList;
        [Multiline][SerializeField] private List<string> modeExplainList;
        [SerializeField] private Text modeText;
        [SerializeField] private Text modeExplain;

        private bool _isMoving;
        private int _modeIndex;
        
        
        private void Start() {
            _isMoving = false;
            Init();
        }

        private void Update() {
            int index = menuText.IndexOf(menuText.OrderBy(x => Mathf.Abs(x.transform.position.y - Input.mousePosition.y)).First());
            
            index = Mathf.Clamp(index, 0, menuText.Count - 1);
            view.UpdateText(menuText, index);
            //if (Input.GetMouseButtonDown(0)) {
                //_isMoving = true;
                // if (index == 0) GotoTutorial();
                // if (index == 1) GotoMainGame();
                // if (index == 2) OpenDictionary();
                // if (index == 3) OpenLineup();
            //}
        }




        void Init() {
            SoundPlayer.I.Play("bgm_title");
            view.FloatLogo(this);
            view.Wave(wave);
            view.Smog(transform, smogContainer);
            PlayerConfig.Create(PlayerConfig.GetShopPool(0), PlayerConfig.GetPoolIndex(), PlayerConfig.GetGameMode());
            _modeIndex = (int)PlayerConfig.GetGameMode();
            modeText.text = modeList[_modeIndex];
            modeExplain.text = modeExplainList[_modeIndex];
        }
        
        public async void GotoMainGame() {
            if (_isMoving) return;
            _isMoving = true;
            SoundPlayer.I.Play("se_select1");
            await SoundPlayer.I.FadeOutSound("bgm_title");
            view.GotoNext();
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            SceneManager.LoadScene("Scenes/Shopping");
        }
        
        public async void GotoTutorial() {
            if (_isMoving) return;
            _isMoving = true;
            SoundPlayer.I.Play("se_select1");
            await SoundPlayer.I.FadeOutSound("bgm_title");
            view.GotoNext();
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            SceneManager.LoadScene("Scenes/TutorialIntro");
        }
        
        public void OpenDictionary() {
            //_isMoving = true;
            dictionary.Open(false);
        }
        
        public void CloseDictionary() {
            //_isMoving = false;
            dictionary.Close();
        }
        
        public void OpenLineup() {
            //_isMoving = true;
            lineup.Open();
        }
        
        public void CloseLineup() {
            //_isMoving = false;
            lineup.Close();
        }

        public void ChangeMode() {
            _modeIndex = (_modeIndex + 1) % modeList.Count;
            modeText.text = modeList[_modeIndex];
            modeExplain.text = modeExplainList[_modeIndex];
            PlayerConfig.Create(PlayerConfig.GetShopPool(0), PlayerConfig.GetPoolIndex(), (GameMode)_modeIndex);

        }
        
        
        
    }
}
