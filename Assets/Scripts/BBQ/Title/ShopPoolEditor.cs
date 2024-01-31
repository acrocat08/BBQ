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
    public class ShopPoolEditor : MonoBehaviour {

        [SerializeField] private ItemSet itemSet;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private Transform container;
        [SerializeField] private ItemDetail detail;
        [SerializeField] private EventTrigger saveButton;
        [SerializeField] private List<CanvasGroup> tabs;
        [SerializeField] private ShopPoolList listWindow;
        
        private bool isMoving;
        private List<GameObject> items;
        private ShopPool _selected;
        

        public void Start() {
            items = new List<GameObject>();
            _selected = PlayerConfig.GetShopPool(0);
        }

        public async void Open(int index) {
            _selected = PlayerConfig.GetShopPool(index);   
            Draw(1);
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
            saveButton.enabled = true;
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
            saveButton.enabled = false;
        }
        
        public void Draw(int tier) {
            foreach (GameObject item in items) {
                Destroy(item);
            }

            for (int i = 0; i < tabs.Count; i++) {
                if (i == tier) tabs[i].alpha = 1f;
                else tabs[i].alpha = 0.3f;
            }
            
            items = new List<GameObject>();
            if (tier > 0) {
                foreach (FoodData food in itemSet.foods.Where(x => x.tier == tier)) {
                    GameObject obj = Instantiate(itemPrefab, container, false);
                    obj.GetComponent<Image>().sprite = food.foodImage;
                    EventTrigger ev = obj.GetComponent<EventTrigger>();
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerClick;
                    entry.callback.AddListener(x => Select(food));
                    ev.triggers.Add(entry);
                    items.Add(obj);
                    int index = itemSet.foods.IndexOf(food);
                    bool isSelected = _selected.foodsIndex.Contains(index);
                    SetIconView(obj, isSelected);
                }
            }
        }


        public void Select(FoodData food) {
            int index = itemSet.foods.IndexOf(food);
            bool isSelected = _selected.foodsIndex.Contains(index);
            if (!isSelected) {
                _selected.foodsIndex.Add(index);
            }
            else {
                _selected.foodsIndex.Remove(index);
            }
            detail.DrawDetail(food, 1);
            SetIconView(items[index % 20], !isSelected);
        }

        private void SetIconView(GameObject obj, bool isSelected) {
            if (isSelected) obj.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            else obj.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }

        public void Save() {
            List<FoodData> foods = _selected.foodsIndex.Select(x => itemSet.foods[x]).ToList();
            for (int i = 1; i <= 5; i++) {
                if (foods.Count(x => x.tier == i) != 10) return;
            }

            _selected.foodsIndex.Sort();
            
            PlayerConfig.Create(_selected, PlayerConfig.GetPoolIndex(), PlayerConfig.GetGameMode());
            listWindow.CloseEditor();
            Close();
        }
        
    }
}
