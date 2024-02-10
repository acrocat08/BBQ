using System.Collections.Generic;
using System.Linq;
using BBQ.Database;
using BBQ.PlayData;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Shopping {
    public class ItemDetail : MonoBehaviour {
        [SerializeField] ItemDetailView view;
        [SerializeField] private List<Transform> tabs;
        [SerializeField] private List<GameObject> lankTabs;

        private ExplainableItem _nowItem;
        private DeckFood _nowFood;

        public void DrawDetail(FoodData foodData, int lank) {
            view.DrawFoodInfo(transform, foodData, lank);
            SetTab(new List<ExplainableItem>{ foodData }.Concat(foodData.subItem).ToList());
            _nowItem = foodData;
            _nowFood = null;
            SetLankTab(lank);
        }

        public void DrawDetail(DeckFood deckFood) {
            view.DrawFoodInfo(transform, deckFood.data, deckFood.lank);
            List<ExplainableItem> tabItems =
                new List<ExplainableItem> { deckFood.data }.Concat(deckFood.data.subItem).ToList();
            if(deckFood.effect != null) tabItems.Add(deckFood.effect);
            SetTab(tabItems);
            _nowItem = null;
            _nowFood = deckFood;
            SetLankTab(deckFood.lank);
        }

        public void SelectLankTab(int lank) {
            if(_nowItem != null && _nowItem is FoodData food) DrawDetail(food, lank);
            else if(_nowFood != null) DrawDetail(_nowFood.data, lank);
        }

        void SetLankTab(int lank) {
            for (int i = 0; i < 3; i++) {
                Color tabColor = lankTabs[i].GetComponent<Image>().color;
                if (i + 1 == lank) {
                    tabColor.a = 1f;
                    lankTabs[i].transform.Find("Star").GetComponent<Image>().enabled = true;
                }
                else {
                    tabColor.a = 0.2f;
                    lankTabs[i].transform.Find("Star").GetComponent<Image>().enabled = false;
                }
                lankTabs[i].GetComponent<Image>().color = tabColor;
            }
        }
        
        public void DrawDetail(ToolData toolData) {
            view.DrawToolInfo(transform, toolData);
            SetTab(new List<ExplainableItem>{ toolData }.Concat(toolData.subItem).ToList());
            _nowItem = toolData;
            _nowFood = null;
        }
        
        void DrawDetail(ExplainableItem item) {
            if(item is FoodData food) view.DrawFoodInfo(transform, food, 1);
            else if(item is ToolData tool) view.DrawToolInfo(transform, tool);
            else if(item is FoodEffect effect) view.DrawEffectInfo(transform, effect);
        }


        private void SetTab(List<ExplainableItem> subItem) {
            if (subItem.Count == 1) {
                transform.Find("Tab").GetComponent<CanvasGroup>().alpha = 0;
                return;
            }
            transform.Find("Tab").GetComponent<CanvasGroup>().alpha = 1;
            for (int i = 0; i < tabs.Count; i++) {
                if (i < subItem.Count) {
                    tabs[i].localScale = Vector3.one;
                    tabs[i].GetComponent<Image>().sprite = subItem[i].GetImage();
                }
                else tabs[i].localScale = Vector3.zero;
            }
            FocusTab(0);
        }

        void FocusTab(int index) {
            for (int i = 0; i < tabs.Count; i++) {
                tabs[i].GetComponent<Image>().color = i == index ? Color.white : new Color(1, 1, 1, 0.1f);
            }
        }
        
        public void SelectTab(int index) {
            FocusTab(index);
            if (index == 0) {
                if(_nowItem != null) DrawDetail(_nowItem);
                if(_nowFood != null) DrawDetail(_nowFood);
            }
            else {
                if(_nowItem != null) DrawDetail(_nowItem.subItem[index - 1]);
                if (_nowFood != null) {
                    if (_nowFood.effect != null && index == _nowFood.data.subItem.Count + 1) 
                        DrawDetail(_nowFood.effect);
                    else DrawDetail(_nowFood.data.subItem[index - 1]);
                }
            }
        }

    }
}