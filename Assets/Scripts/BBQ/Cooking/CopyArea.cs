using System.Collections.Generic;
using BBQ.Action;
using BBQ.Database;
using BBQ.PlayData;
using UnityEngine;

namespace BBQ.Cooking {
    public class CopyArea : MonoBehaviour, IReleasable {
        [SerializeField] private LaneFoodFactory foodFactory;
        [SerializeField] private Deck deck;
        [SerializeField] private ItemSet itemSet;

        private List<DeckFood> _deckFoods;

        public void Init() {
            _deckFoods = new List<DeckFood>();
        }

        public void SetPosition(LaneFood laneFood) {
            transform.position = laneFood.transform.position;
        }

        public List<DeckFood> CopyFoods(List<DeckFood> foods) {
            List<DeckFood> ret = new List<DeckFood>();
            foreach (DeckFood deckFood in foods) {
                DeckFood copied = deckFood.Copy();
                deck.RegisterFood(copied);
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

        public List<LaneFood> ReleaseFoods(List<DeckFood> foods) {
            List<LaneFood> ret = new List<LaneFood>();
            foreach (DeckFood food in foods) {
                _deckFoods.Remove(food);
                LaneFood laneFood = foodFactory.Create(food, transform);
                laneFood.transform.localPosition = Vector3.zero;
                ret.Add(laneFood);
            }
            return ret;
        }
    }
}
