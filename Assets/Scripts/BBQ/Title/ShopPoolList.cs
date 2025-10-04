using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Database;
using BBQ.PlayData;
using BBQ.Shopping;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SoundMgr;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BBQ.Title {
    public class ShopPoolList : MonoBehaviour {

        [SerializeField] private ItemSet itemSet;
        [SerializeField] private EventTrigger backButton;
        [SerializeField] private ShopPoolEditor editorWindow;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private Transform container;
        [SerializeField] private List<Image> tabs;
        [SerializeField] private List<Color> selectColor;
        [SerializeField] private Text poolName;
        [SerializeField] private InputField inputField;
        
        private bool isMoving;
        private List<GameObject> items;
        private int _nowIndex;

        

        public void Start() {
            items = new();
        }

        public async void Open() {
            SoundPlayer.I.Play("se_select1");
            _nowIndex = 0;
            for (int i = 0; i < tabs.Count; i++) {
                if (i == _nowIndex) tabs[i].color = selectColor[0];
                else tabs[i].color = selectColor[1];
            }
            Draw(_nowIndex);
            isMoving = true;
            transform.localScale = Vector3.one;
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            DOTween.To(
                () => canvasGroup.alpha,
                x => canvasGroup.alpha = x,
                1f,
                0f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            isMoving = false;
            backButton.enabled = true;

        }
        
        public async void Close() {
            isMoving = true;
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            DOTween.To(
                () => canvasGroup.alpha,
                x => canvasGroup.alpha = x,
                0f,
                0f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            transform.localScale = Vector3.zero;
            isMoving = false;
            backButton.enabled = false;
        }
        
        public void Draw(int index) {
            foreach (GameObject item in items) {
                Destroy(item);
            }

            ShopPool targetPool = PlayerConfig.GetShopPool(index + 9);
            
            items = new();
            foreach (int foodIndex in targetPool.foodsIndex) {
                FoodData food = itemSet.foods[foodIndex];
                GameObject obj = Instantiate(itemPrefab, container, false);
                obj.GetComponent<Image>().sprite = food.foodImage;
                items.Add(obj);
            }

            poolName.text = (index + 1) + " : " + targetPool.poolName; 
        }

        public void ChangeTab(int index) {
            SoundPlayer.I.Play("se_select2");
            _nowIndex = index;
            for (int i = 0; i < tabs.Count; i++) {
                if (i == _nowIndex) tabs[i].color = selectColor[0];
                else tabs[i].color = selectColor[1];
            }
            Draw(index);
        }

        public void Edit() {
            editorWindow.Open(_nowIndex);
        }

        public void Select() {
            PlayerConfig.Create(PlayerConfig.GetShopPool(9), 0, _nowIndex, PlayerConfig.GetGameMode(),
                PlayerConfig.GetBgmVolume(), PlayerConfig.GetSeVolume());
            for (int i = 0; i < tabs.Count; i++) {
                if (i == _nowIndex) tabs[i].color = selectColor[0];
                else tabs[i].color = selectColor[1];
            }
        }

        public void CloseEditor() {
            Draw(0);
        }

        public void CopyPoolCode() {
            GUIUtility.systemCopyBuffer = PlayerConfig.GetShopPool(_nowIndex + 9).Encode();
        }

        public void LoadPoolCode() {
            string code = inputField.text;
            try {
                ShopPool pool = ShopPool.Decode(code);
                PlayerConfig.Create(pool, _nowIndex, PlayerConfig.GetPoolIndex(), PlayerConfig.GetGameMode(),
                    PlayerConfig.GetBgmVolume(), PlayerConfig.GetSeVolume());
                Draw(_nowIndex);
            }
            catch {
                
            }
        }
        
        
    }
}
