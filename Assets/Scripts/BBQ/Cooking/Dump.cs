using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.PlayData;
using UnityEngine;

namespace BBQ.Cooking {
    public class Dump : MonoBehaviour, IReleasable {
        [SerializeField] private DumpView view;
        [SerializeField] private FoodObjectFactory foodFactory;

        private List<DeckFood> _foods;

        private List<FoodObject> _hittingFoods;

        public void Init() {
            _foods = new List<DeckFood>();
        }

        public List<DeckFood> SelectAll() {
            return new List<DeckFood>(_foods.Where(x => !x.isFrozen && !x.isFired));
        }

        public List<FoodObject> ReleaseFoods(List<DeckFood> foods) {
            List<FoodObject> ret = new List<FoodObject>();
            foreach (DeckFood food in foods) {
                _foods.Remove(food);
                FoodObject laneFood = foodFactory.Create(food, transform);
                ret.Add(laneFood);
            }
            return ret;
        }
        
        public FoodObject GetObject(DeckFood food) {
            return _hittingFoods.FirstOrDefault(x => x.deckFood == food);
        }

        public void AddFoods(List<FoodObject> foods) {
            _foods.AddRange(foods.Select(x => x.deckFood));
            foreach (FoodObject laneFood in foods) {
                laneFood.deckFood.Releasable = this;
                laneFood.Drop();
            }
        }

        public void HitFoods(List<FoodObject> foods) {
            _foods.AddRange(foods.Select(x => x.deckFood));
            _hittingFoods = foods;
            foreach (FoodObject laneFood in foods) {
                laneFood.deckFood.Releasable = this;
            }
        }

        public List<DeckFood> GetHittingFoods() {
            return _hittingFoods.Select(x => x.deckFood).ToList();
        }
        
    }
}
