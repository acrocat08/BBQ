using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using BBQ.Tutorial;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace BBQ.Shopping {
    public class InventoryFood : FoodObject {
        [SerializeField] private DeckInventory inventory;

        [SerializeField] private ItemDetail itemDetail;
        [SerializeField] private Merger merger;
        [SerializeField] private int index;
        [SerializeField] private Shop shop;
        [SerializeField] private TutorialShopping tutorial;

        public void SetFood(DeckFood food) {
            deckFood = food;
            view.Draw(this);
        }

        public FoodData GetFoodData() {
            return deckFood.data;
        }

        public void OnPointDown() {
            if(deckFood.data != null) itemDetail.DrawDetail(deckFood);
        }

        public async void OnPointUp(List<PointableArea> areas) {
            if (InputGuard.Guard()) return;
            List<InventoryFood> target = areas.Select(x => x.transform.parent.GetComponent<InventoryFood>()).ToList();
            target.Add(this);
            await merger.Merge(target, shop);
            inventory.SortItem();
            if (tutorial != null) tutorial.Merge();
        }

        public int GetIndex() {
            return index;
        }

        public override async void LankUp() {
            await view.LankUp(this);
            view.Draw(this);
        }
        
        public override void Drop() {
            deckFood.data = null;
            view.Drop(this);
        }

    }
}
