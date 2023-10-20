using System.Collections.Generic;
using BBQ.Action;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Cooking {
    public class CopyArea : MonoBehaviour, IReleasable {
        [SerializeField] private FoodObjectFactory foodFactory;
        [SerializeField] private Deck deck;
        [SerializeField] private ItemSet itemSet;

        private List<DeckFood> _deckFoods;

        public void Init() {
            _deckFoods = new List<DeckFood>();
        }

        public void SetPosition(FoodObject laneFood) {
            if (laneFood == null) return;
            transform.position = laneFood.transform.position;
        }

        public List<DeckFood> CopyFoods(List<DeckFood> foods) {
            List<DeckFood> ret = new List<DeckFood>();
            foreach (DeckFood deckFood in foods) {
                DeckFood copied = deckFood.Copy();
                copied.Releasable = this;
                _deckFoods.Add(copied);
                ret.Add(copied);
            }
            return ret;
        }

        public List<DeckFood> MakeFood(string foodName) {
            FoodData foodData = itemSet.SearchFood(foodName);
            return new List<DeckFood>() { new DeckFood(foodData) };
        }

        public void RegisterFood(List<DeckFood> foods) {
            foreach (DeckFood deckFood in foods) {
                if (deck != null) deck.RegisterFood(deckFood);
                //else TriggerObserver.I.RegisterFood(deckFood);
            }
        }

        public List<FoodObject> ReleaseFoods(List<DeckFood> foods) {
            List<FoodObject> ret = new List<FoodObject>();
            foreach (DeckFood food in foods) {
                _deckFoods.Remove(food);
                FoodObject laneFood = foodFactory.Create(food, transform);
                laneFood.transform.localPosition = Vector3.zero;
                ret.Add(laneFood);
            }
            return ret;
        }
        
        public FoodObject GetObject(DeckFood food) {
            return null;
        }
    }
}
