using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class ItemDictionary : MonoBehaviour {

        [SerializeField] private ItemSet itemSet;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private Transform container;
        [SerializeField] private ItemDetail detail;
        [SerializeField] private EventTrigger backButton;
        [SerializeField] private List<CanvasGroup> tabs;
        [SerializeField] private GameObject changeCosplay;
        
        private bool isMoving;
        private List<GameObject> items;
        private bool _showPool;
        private FoodData _nowFood;
        private List<string> _unlockedSkin;
        

        public void Start() {
            items = new();
        }

        public async void Open(bool showPool) {
            SoundPlayer.I.Play("se_select1");
            _showPool = showPool;
            _unlockedSkin = PlayerPrefs.GetString("unlockedSkin", "").Split(",").Where(x => x != "").ToList();
            Draw(1);
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
                List<FoodData> targetFoods;
                if (_showPool) targetFoods = itemSet.GetFoodPool();
                else targetFoods = itemSet.foods;
                foreach (FoodData food in targetFoods.Where(x => x.tier == tier)) {
                    GameObject obj = Instantiate(itemPrefab, container, false);
                    obj.GetComponent<Image>().sprite = food.foodImage;
                    EventTrigger ev = obj.GetComponent<EventTrigger>();
                    EventTrigger.Entry entry = new();
                    entry.eventID = EventTriggerType.PointerClick;
                    entry.callback.AddListener(x => ShowDetail(food));
                    ev.triggers.Add(entry);
                    if (_unlockedSkin.Contains(food.foodName)) {
                        obj.transform.Find("Skin").GetComponent<Image>().enabled = true;
                        obj.transform.Find("Skin").GetComponent<Image>().color =
                            (PlayerConfig.CheckCosplay(food.foodName))
                                ? new Color(0.88f, 0.64f, 0.44f, 0.3f)
                                : new Color(0.13f, 0.1f, 0.08f, 0.5f);
                    }
                    items.Add(obj);
                }
                return;
            }
            
            foreach (ToolData tool in itemSet.tools) {
                GameObject obj = Instantiate(itemPrefab, container, false);
                obj.GetComponent<Image>().sprite = tool.toolImage;
                EventTrigger ev = obj.GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener(x => ShowDetail(tool));
                ev.triggers.Add(entry);
                items.Add(obj);
            }
        }


        public void ShowDetail(FoodData food) {
            SoundPlayer.I.Play("se_select3");
            detail.DrawDetail(food, 1);
            if (changeCosplay == null) return;
            if (itemSet.foods.Contains(food)) {
                changeCosplay.SetActive(true);
                _nowFood = food;
                SetCosplayName(food);
                bool unlokedSkin = _unlockedSkin.Contains(food.foodName);
                changeCosplay.GetComponent<CanvasGroup>().alpha = unlokedSkin ? 1 : 0.2f;
                changeCosplay.GetComponent<EventTrigger>().enabled = unlokedSkin;
            }
            else changeCosplay.SetActive(false);
        }
        
        public void ShowDetail(ToolData tool) {
            SoundPlayer.I.Play("se_select3");
            detail.DrawDetail(tool);
            if (changeCosplay == null) return;
            changeCosplay.SetActive(false);
        }

        public void SetCosplayState() {
            PlayerConfig.Create(PlayerConfig.GetShopPool(9), 0, PlayerConfig.GetPoolIndex(), PlayerConfig.GetGameMode(),
                PlayerConfig.GetBgmVolume(), PlayerConfig.GetSeVolume(), _nowFood.foodName);
            ShowDetail(_nowFood);
            SetCosplayName(_nowFood);
        }

        private void SetCosplayName(FoodData food) {
            if (PlayerConfig.CheckCosplay(food.foodName)) {
                changeCosplay.transform.Find("Name").GetComponent<Text>().text = "スキン：" + food.cosplayName;
            }
            else {
                changeCosplay.transform.Find("Name").GetComponent<Text>().text = "スキン：通常";
            }
        }
        
    }
}
