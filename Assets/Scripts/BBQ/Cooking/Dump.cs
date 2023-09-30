using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using UnityEngine;

namespace BBQ.Cooking {
    public class Dump : MonoBehaviour, IReleasable {
        [SerializeField] private DumpView view;
        [SerializeField] private FoodObjectFactory foodFactory;
        [SerializeField] private DesignParam param;

        private List<DeckFood> _foods;

        private FoodObject[] _hittingFoods;

        public void Init() {
            _foods = new List<DeckFood>();
            _hittingFoods = new FoodObject[] { null, null, null };
        }

        public List<DeckFood> SelectAll() {
            return new List<DeckFood>(_foods.Where(x => !x.isFrozen && !x.isFired));
        }
        
        public List<DeckFood> SelectFrozen() {
            return new List<DeckFood>(_foods.Where(x => x.isFrozen));
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
            return _hittingFoods.Where(x => x).FirstOrDefault(x => x.deckFood == food);
        }

        public void AddFoods(List<FoodObject> foods) {
            foods = foods.Where(x => x != null && x.deckFood.data != param.resetFood).ToList();
            _foods.AddRange(foods.Select(x => x.deckFood));
            foreach (FoodObject laneFood in foods) {
                laneFood.deckFood.Releasable = this;
                laneFood.Drop();
            }
        }

        public void HitFoods(List<FoodObject> foods) {
            foods = foods.Where(x => x != null && x.deckFood.data != param.resetFood).ToList();
            _foods.AddRange(foods.Select(x => x.deckFood));
            _hittingFoods = foods.ToArray();
            foreach (FoodObject laneFood in foods) {
                if(laneFood == null) continue;
                laneFood.deckFood.Releasable = this;
            }
        }

        public List<DeckFood> GetHittingFoods() {
            return _hittingFoods.Where(x => x != null && x.deckFood.data != param.resetFood).Select(x => x.deckFood).ToList();
        }

        public int GetHittingFoodLane(DeckFood food) {
            for (int i = 0; i < 3; i++) {
                if (_hittingFoods[i] != null && _hittingFoods[i].deckFood == food)
                    return i + 1;
            }
            return 0;
        }
        
    }
}
