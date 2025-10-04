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
    public class ShopPoolEditor : MonoBehaviour {

        [SerializeField] private ItemSet itemSet;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private Transform container;
        [SerializeField] private ItemDetail detail;
        [SerializeField] private EventTrigger saveButton;
        [SerializeField] private List<CanvasGroup> tabs;
        [SerializeField] private ShopPoolList listWindow;
        [SerializeField] private InputField inputField;
        
        private bool isMoving;
        private List<GameObject> items;
        private ShopPool _selected;
        private int poolIndex;

        public void Start() {
            items = new();
            _selected = PlayerConfig.GetShopPool(9);
        }

        public async void Open(int index) {
            poolIndex = index + 9;
            _selected = PlayerConfig.GetShopPool(poolIndex);   
            Draw(1);
            inputField.text = PlayerConfig.GetShopPool(poolIndex).poolName;
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
            saveButton.enabled = true;
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
            saveButton.enabled = false;
        }
        
        public void Draw(int tier) {
            SoundPlayer.I.Play("se_select2");
            foreach (GameObject item in items) {
                Destroy(item);
            }

            for (int i = 0; i < tabs.Count; i++) {
                if (i == tier) tabs[i].alpha = 1f;
                else tabs[i].alpha = 0.3f;
            }
            
            items = new();
            if (tier > 0) {
                foreach (FoodData food in itemSet.foods.Where(x => x.tier == tier)) {
                    GameObject obj = Instantiate(itemPrefab, container, false);
                    obj.GetComponent<Image>().sprite = food.foodImage;
                    EventTrigger ev = obj.GetComponent<EventTrigger>();
                    EventTrigger.Entry entry = new();
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
            SoundPlayer.I.Play("se_select3");
            int index = itemSet.foods.IndexOf(food);
            bool isSelected = _selected.foodsIndex.Contains(index);
            if (!isSelected) {
                _selected.foodsIndex.Add(index);
            }
            else {
                _selected.foodsIndex.Remove(index);
            }
            detail.DrawDetail(food, 1);
            SetIconView(items[index % 30], !isSelected);
        }

        private void SetIconView(GameObject obj, bool isSelected) {
            if (isSelected) obj.GetComponent<Image>().color = new(1, 1, 1, 1);
            else obj.GetComponent<Image>().color = new(0.5f, 0.5f, 0.5f, 0.5f);
        }

        public void Save() {
            List<FoodData> foods = _selected.foodsIndex.Select(x => itemSet.foods[x]).ToList();
            
            for (int i = 1; i <= 5; i++) {
                if (foods.Count(x => x.tier == i) != 10) return;
            }

            _selected.foodsIndex.Sort();
            _selected.poolName = inputField.text;

            PlayerConfig.Create(_selected, poolIndex - 9, PlayerConfig.GetPoolIndex(), PlayerConfig.GetGameMode(),
                PlayerConfig.GetBgmVolume(), PlayerConfig.GetSeVolume());
            listWindow.CloseEditor();
            Close();
        }
        
    }
}
