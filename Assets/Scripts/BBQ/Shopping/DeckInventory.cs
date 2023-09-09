using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Database;
using BBQ.PlayData;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace BBQ.Shopping {
    public class DeckInventory : MonoBehaviour {
        [SerializeField] private DeckInventoryView view;
        [SerializeField] List<DeckItem> deckItems;

        public void Init(List<DeckFood> deckFoods) {
            for (int i = 0; i < deckFoods.Count; i++) {
                deckItems[i].SetFood(deckFoods[i]);
            }
        }

        public void AddItem(FoodData food) {
            DeckFood deckFood = new DeckFood(food);
            DeckItem target = deckItems.FirstOrDefault(x => x.GetFood() == null);
            if (target == null) return;
            target.SetFood(deckFood);
        }
        
        public List<DeckFood> GetDeckFoods() {
            return deckItems.Select(x => x.GetFood()).Where(x => x != null).ToList();
        }

        public bool CheckIsEmpty() {
            return deckItems.Any(x => x.GetFood() == null);
        }
    }

}
