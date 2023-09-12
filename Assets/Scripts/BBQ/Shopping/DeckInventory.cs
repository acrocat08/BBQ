using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Database;
using BBQ.PlayData;
using UnityEngine;

namespace BBQ.Shopping {
    public class DeckInventory : MonoBehaviour {
        [SerializeField] private DeckInventoryView view;
        [SerializeField] List<DeckItem> deckItems;
        [SerializeField] private Merger merger;

        public void Init(List<DeckFood> deckFoods) {
            for (int i = 0; i < deckFoods.Count; i++) {
                deckItems[i].SetFood(deckFoods[i]);
            }
            SetPointableArea();
        }

        public void AddItem(FoodData food) {
            DeckFood deckFood = new DeckFood(food);
            DeckItem target = deckItems.FirstOrDefault(x => x.GetFood() == null);
            if (target == null) return;
            target.SetFood(deckFood);
            SetPointableArea();
        }

        public void SortItem() {
            deckItems.ForEach(x => x.transform.Find("Food").localPosition = Vector3.zero);
            SetPointableArea();
        }
        
        public List<DeckFood> GetDeckFoods() {
            return deckItems.Select(x => x.GetFood()).Where(x => x != null).ToList();
        }

        public bool CheckIsEmpty() {
            return deckItems.Any(x => x.GetFood() == null);
        }

        void SetPointableArea() {
            var group = deckItems
                .Where(x => x.GetFood() != null)
                .Where(x => !x.GetFood().data.isToken)
                .GroupBy(x => (x.GetFood().data, x.GetFood().lank));
            foreach (var g in group) {
                bool canMerge = merger.CheckCanMerge(g.Key.data, g.Key.lank, g.Count());
                Debug.Log(canMerge);
                foreach (DeckItem deckItem in g) {
                    PointableArea area = deckItem.transform.Find("Merge").GetComponent<PointableArea>();
                    area.areaTag = canMerge ? (g.Key.data.foodName + g.Key.lank) : "none";
                    area.targetTag = canMerge ? (g.Key.data.foodName + g.Key.lank) : "";
                    area.canPointDown = canMerge;
                    area.isGrouped = canMerge;
                    deckItem.transform.Find("Pointable").GetComponent<PointableArea>().canPointDown = !canMerge;
                }
            }
        }
        
        
        
    }

}