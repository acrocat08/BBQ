using System;
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

        private bool isMoving;

        public void Start() {
            Init();
        }

        public async void Open() {
            isMoving = true;
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
            isMoving = false;
            backButton.enabled = false;
        }
        
        public void Init() {
            foreach (FoodData food in itemSet.foods) {
                GameObject obj = Instantiate(itemPrefab, container, false);
                obj.GetComponent<Image>().sprite = food.foodImage;
                EventTrigger ev = obj.GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener(x => ShowDetail(food));
                ev.triggers.Add(entry);
            }
            foreach (ToolData tool in itemSet.tools) {
                GameObject obj = Instantiate(itemPrefab, container, false);
                obj.GetComponent<Image>().sprite = tool.toolImage;
                EventTrigger ev = obj.GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener(x => ShowDetail(tool));
                ev.triggers.Add(entry);
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
