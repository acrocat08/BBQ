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
            await merger.Merge(target, shop, tutorial != null);
            inventory.SortItem();
            if (tutorial != null) tutorial.Merge();
        }

        public int GetIndex() {
            return index;
        }

        public override async UniTask LankUp() {
            await view.LankUp(this);
            view.Draw(this);
            inventory.SortItem();
        }
        
        public override void Hit() {
            view.Hit(this);
        }
        
        public override async UniTask Drop() {
            deckFood.data = null;
            deckFood.effect = null;
            DeckFood emptyFood = new(null);
            SetFood(emptyFood);
            view.Drop(this);  
        }
        public async UniTask ForkDrop() {
            FoodData prevData = deckFood.data;
            int prevLank = deckFood.lank;
            deckFood.data = null;
            deckFood.effect = null;
            DeckFood emptyFood = new(null);
            SetFood(emptyFood);
            await ((InventoryFoodView)view).ForkDrop(this, prevData, prevLank);
        }

    }
}
