using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Database;
using BBQ.PlayData;
using BBQ.Shopping;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        
        private bool isMoving;
        private List<GameObject> items;
        private int _nowIndex;

        

        public void Start() {
            items = new List<GameObject>();
        }

        public async void Open() {
            _nowIndex = PlayerConfig.GetPoolIndex();
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
                0.2f);
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
                0.2f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            transform.localScale = Vector3.zero;
            isMoving = false;
            backButton.enabled = false;
        }
        
        public void Draw(int index) {
            foreach (GameObject item in items) {
                Destroy(item);
            }
            
            items = new List<GameObject>();
            foreach (int foodIndex in PlayerConfig.GetShopPool(index).foodsIndex) {
                FoodData food = itemSet.foods[foodIndex];
                GameObject obj = Instantiate(itemPrefab, container, false);
                obj.GetComponent<Image>().sprite = food.foodImage;
                items.Add(obj);
            }

            poolName.text = "Lineup " + (index + 1);
        }

        public void ChangeTab(int index) {
            _nowIndex = index;
            Draw(index);
        }

        public void Edit() {
            editorWindow.Open(_nowIndex);
        }

        public void Select() {
            PlayerConfig.Create(PlayerConfig.GetShopPool(0), _nowIndex, PlayerConfig.GetGameMode());
            for (int i = 0; i < tabs.Count; i++) {
                if (i == _nowIndex) tabs[i].color = selectColor[0];
                else tabs[i].color = selectColor[1];
            }
        }

        public void CloseEditor() {
            Draw(0);
        }
        
        
    }
}
