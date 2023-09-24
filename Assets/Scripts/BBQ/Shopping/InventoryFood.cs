using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace BBQ.Shopping {
    public class InventoryFood : FoodObject {
        [SerializeField] private DeckInventory inventory;

        [SerializeField] private Transform detail;
        [SerializeField] private DetailView detailView;
        [SerializeField] private Merger merger;
        [SerializeField] private int index;
        [SerializeField] private Shop shop;

        public void SetFood(DeckFood food) {
            deckFood = food;
            view.Draw(this);
        }

        public FoodData GetFoodData() {
            return deckFood.data;
        }

        public void OnPointDown() {
            if(deckFood.data != null) detailView.DrawDetail(detail, deckFood);
        }

        public async void OnPointUp(List<PointableArea> areas) {
            List<InventoryFood> target = areas.Select(x => x.transform.parent.GetComponent<InventoryFood>()).ToList();
            target.Add(this);
            await merger.Merge(target, shop);
            inventory.SortItem();
        }

        public int GetIndex() {
            return index;
        }

        public override async void LankUp() {
            deckFood.lank += 1;
            view.LankUp(this);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            view.Draw(this);
        }
    }
}
