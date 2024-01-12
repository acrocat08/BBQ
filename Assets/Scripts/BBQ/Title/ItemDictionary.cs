using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Database;
using BBQ.Shopping;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        
        private bool isMoving;
        private List<GameObject> items;
        

        public void Start() {
            items = new List<GameObject>();
        }

        public async void Open() {
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
                    entry.callback.AddListener(x => ShowDetail(food));
                    ev.triggers.Add(entry);
                    items.Add(obj);
                }
                return;
            }
            
            foreach (ToolData tool in itemSet.tools) {
                GameObject obj = Instantiate(itemPrefab, container, false);
                obj.GetComponent<Image>().sprite = tool.toolImage;
                EventTrigger ev = obj.GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener(x => ShowDetail(tool));
                ev.triggers.Add(entry);
                items.Add(obj);
            }
        }


        public void ShowDetail(FoodData food) {
            detail.DrawDetail(food);
        }
        
        public void ShowDetail(ToolData tool) {
            detail.DrawDetail(tool);
        }
    }
}
