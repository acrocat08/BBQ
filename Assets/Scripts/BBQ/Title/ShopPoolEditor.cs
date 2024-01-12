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
        private List<int> _selected;
        

        public void Start() {
            items = new List<GameObject>();
            _selected = new List<int>();
        }

        public async void Open(int index) {
            _selected = PlayerConfig.GetShopPool().foodsIndex;            
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
                    bool isSelected = _selected.Contains(index);
                    SetIconView(obj, isSelected);
                }
            }
        }


        public void Select(FoodData food) {
            int index = itemSet.foods.IndexOf(food);
            bool isSelected = _selected.Contains(index);
            if (!isSelected) {
                _selected.Add(index);
            }
            else {
                _selected.Remove(index);
            }
            detail.DrawDetail(food);
            SetIconView(items[index % 20], !isSelected);
        }

        private void SetIconView(GameObject obj, bool isSelected) {
            if (isSelected) obj.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            else obj.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }

        public void Save() {
            List<FoodData> foods = _selected.Select(x => itemSet.foods[x]).ToList();
            for (int i = 1; i <= 5; i++) {
                if (foods.Count(x => x.tier == i) != 10) return;
            }

            _selected.Sort();
            ShopPool newLineup = new ShopPool(_selected, "pool");
            
            PlayerConfig.Create(newLineup, PlayerConfig.GetGameMode());
            listWindow.CloseEditor();
            Close();
        }
        
    }
}
