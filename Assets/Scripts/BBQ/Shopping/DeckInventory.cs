using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Action;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.Database;
using BBQ.PlayData;
using UnityEngine;

namespace BBQ.Shopping {
    public class DeckInventory : MonoBehaviour, IReleasable {
        [SerializeField] private DeckInventoryView view;
        [SerializeField] List<InventoryFood> deckItems;
        [SerializeField] private Merger merger;

        public void Init(List<DeckFood> deckFoods) {
            for (int i = 0; i < deckFoods.Count; i++) {
                deckItems[i].SetFood(deckFoods[i]);
                TriggerObserver.I.RegisterFood(deckFoods[i]);
            }
            SetPointableArea();
        }

        public void AddItem(DeckFood food) {
            DeckFood deckFood = food;
            InventoryFood target = deckItems.FirstOrDefault(x => x.GetFoodData() == null);
            if (target == null) return;
            target.SetFood(deckFood);
            SetPointableArea();
            deckFood.Releasable = this;
            TriggerObserver.I.RegisterFood(food);
        }

        public void SortItem() {
            deckItems.ForEach(x => x.transform.Find("Image").localPosition = Vector3.zero);
            SetPointableArea();
        }
        
        public List<DeckFood> GetDeckFoods() {
            return deckItems.Select(x => x.deckFood).Where(x => x.data != null).ToList();
        }

        public bool CheckIsEmpty() {
            return deckItems.Any(x => x.GetFoodData() == null);
        }

        void SetPointableArea() {
            var group = deckItems
                .GroupBy(x => (x.deckFood.data, x.deckFood.lank));
            foreach (var g in group) {
                bool canMerge = merger.CheckCanMerge(g.Key.data, g.Key.lank, g.Count());
                foreach (InventoryFood deckItem in g) {
                    PointableArea area = deckItem.transform.Find("Merge").GetComponent<PointableArea>();
                    area.areaTag = canMerge ? (g.Key.data.foodName + g.Key.lank) : "none";
                    area.targetTag = canMerge ? (g.Key.data.foodName + g.Key.lank) : "";
                    area.canPointDown = canMerge;
                    area.isGrouped = canMerge;
                    deckItem.transform.Find("Pointable").GetComponent<PointableArea>().canPointDown = !canMerge;
                }
            }
        }


        public List<FoodObject> ReleaseFoods(List<DeckFood> foods) {
            return new List<FoodObject>();
        }

        public FoodObject GetObject(DeckFood food) {
            return deckItems.FirstOrDefault(x => x.deckFood == food);
        }
    }

}