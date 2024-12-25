using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using BBQ.Common;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SoundMgr;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BBQ.Title {
    public class TitleMenu : MonoBehaviour {

        [SerializeField] private TitleMenuView view;
        [SerializeField] private Transform wave;
        
        [SerializeField] private List<Transform> menuText;
        [SerializeField] private Transform smogContainer;
        [SerializeField] private ItemDictionary dictionary;
        [SerializeField] private ShopPoolList lineup;
        [SerializeField] private Transform container;
        [SerializeField] private GameObject backButton;
        [SerializeField] private List<GameObject> modeButtons;
        [SerializeField] private SceneTransition transition;
        [SerializeField] private Transform basePos;

        [SerializeField] private List<GameObject> lineupList;
        [SerializeField] private List<GameObject> modeList;
        [SerializeField] private Text highScore;
        [SerializeField] private GameObject startButton;
        [SerializeField] private ExpGage expGage;

        private bool _isMoving;
        private int _modeIndex;
        private int _prevIndex;
        private bool _isSelectingMode;

        private int _nowLineup;
        private int _nowMode;
        
        private void Start() {
            _isMoving = false;
            Init();
        }

        private void Update() {
            if (_isSelectingMode || _isMoving) return;
            int index = menuText.IndexOf(menuText.OrderBy(x => Mathf.Abs(x.transform.position.y - Input.mousePosition.y - 50)).First());
            index = Mathf.Clamp(index, 0, menuText.Count - 1);
            if (index == _prevIndex) return;
            _prevIndex = index;
            SoundPlayer.I.Play("se_select2");
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
            //view.FloatLogo(this);
            view.Wave(wave);
            var cts = new CancellationTokenSource();  
            CancellationToken token = cts.Token;  
            view.Smog(transform, smogContainer, token);
            PlayerConfig.Create(PlayerConfig.GetShopPool(9), 0, PlayerConfig.GetPoolIndex(), PlayerConfig.GetGameMode());
            _modeIndex = (int)PlayerConfig.GetGameMode();

            _nowLineup = PlayerConfig.GetPoolIndex();
            _nowMode = (int)PlayerConfig.GetGameMode();
            for (int i = 0; i < lineupList.Count; i++) {
                lineupList[i].SetActive(i == _nowLineup);
            }
            for (int i = 0; i < modeList.Count; i++) {
                modeList[i].SetActive(i == _nowMode);
            }
            expGage.Init(PlayerPrefs.GetInt("expLevel", 0), PlayerPrefs.GetInt("expPoint", 0));
            UpdateHighScore();
        }
        
        public async void GotoMainGame() {
            if (_isMoving) return;
            _isMoving = true;
            _isSelectingMode = true;
            SoundPlayer.I.Play("se_select1");
            
            container.DOMoveX(basePos.position.x, 1f).SetEase(Ease.OutQuint);
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            backButton.SetActive(true);
            //_isMoving = false;

        }
        public async void BackToMenu() {
            backButton.SetActive(false);
            container.DOMoveX(Screen.width / 2f, 1f).SetEase(Ease.OutQuint);
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            _isMoving = false;
            _isSelectingMode = false;
        }

        public async void GotoGame() {
            startButton.SetActive(false);
            Debug.Log(_nowLineup);
            PlayerConfig.Create(PlayerConfig.GetShopPool(9), 0, _nowLineup, (GameMode)_nowMode);
            SoundPlayer.I.Play("se_missionClear");
            await SoundPlayer.I.FadeOutSound("bgm_title");
            view.GotoNext();
            await transition.SceneEnd();
            SceneManager.LoadScene("Scenes/Shopping");
        }

        public void ChangeLineup(int dir) {
            SoundPlayer.I.Play("se_changeMode");
            _nowLineup = (_nowLineup + dir + lineupList.Count) % lineupList.Count;
            for (int i = 0; i < lineupList.Count; i++) {
                lineupList[i].SetActive(i == _nowLineup);
            }
            UpdateHighScore();

        }
        
        public void ChangeMode(int dir) {
            SoundPlayer.I.Play("se_changeMode");
            _nowMode = (_nowMode + dir + modeList.Count) % modeList.Count;
            for (int i = 0; i < modeList.Count; i++) {
                modeList[i].SetActive(i == _nowMode);
            }
            UpdateHighScore();
        }
        
        public async void GotoTutorial() {
            if (_isMoving) return;
            _isMoving = true;
            for (int i = 1; i < menuText.Count; i++) {
                menuText[i].gameObject.SetActive(false);
            }
            SoundPlayer.I.Play("se_missionClear");
            await SoundPlayer.I.FadeOutSound("bgm_title");
            view.GotoNext();
            await transition.SceneEnd();
            await UniTask.Delay(TimeSpan.FromSeconds(1f));            
            SceneManager.LoadScene("Scenes/TutorialIntro");
        }
        
        public void OpenDictionary() {
            _isMoving = true;
            dictionary.Open(false);
        }
        
        public void CloseDictionary() {
            _isMoving = false;
            dictionary.Close();
        }
        
        public void OpenLineup() {
            _isMoving = true;
            lineup.Open();
        }
        
        public void CloseLineup() {
            _isMoving = false;
            lineup.Close();
        }

        public void ChangeMode() {
            _modeIndex = (_modeIndex + 1) % modeList.Count;
            PlayerConfig.Create(PlayerConfig.GetShopPool(9), 0, PlayerConfig.GetPoolIndex(), (GameMode)_modeIndex);
        }

        void UpdateHighScore() {
            if (_nowLineup <= 8) {
                highScore.text =
                    "Best Score： " + PlayerPrefs.GetInt("score_" + _nowMode + "_" + _nowLineup, 0); 
            }
            else {
                highScore.text = "";
            }
            
        }
        
        
        
    }
}
