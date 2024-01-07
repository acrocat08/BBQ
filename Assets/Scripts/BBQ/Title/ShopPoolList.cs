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
        
        private bool isMoving;
        private List<GameObject> items;

        

        public void Start() {
            items = new List<GameObject>();
        }

        public async void Open() {
            Draw(0);
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
            foreach (int foodIndex in PlayerConfig.GetShopPool().foodsIndex) {
                FoodData food = itemSet.foods[foodIndex];
                GameObject obj = Instantiate(itemPrefab, container, false);
                obj.GetComponent<Image>().sprite = food.foodImage;
                items.Add(obj);
            }
        }


        public void Select(int index) {
            editorWindow.Open(index);
        }

        public void CloseEditor() {
            Draw(0);
        }
        
        
    }
}
