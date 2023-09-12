using System.Collections.Generic;
using System.Linq;
using BBQ.PlayData;
using UnityEngine;

namespace BBQ.Cooking {
    public class Dump : MonoBehaviour, IReleasable {
        [SerializeField] private DumpView view;
        [SerializeField] private LaneFoodFactory foodFactory;

        private List<DeckFood> _foods;

        public void Init() {
            _foods = new List<DeckFood>();
        }

        public List<DeckFood> SelectAll() {
            return new List<DeckFood>(_foods.Where(x => !x.isFrozen && !x.isFired));
        }

        public List<LaneFood> ReleaseFoods(List<DeckFood> foods) {
            List<LaneFood> ret = new List<LaneFood>();
            foreach (DeckFood food in foods) {
                _foods.Remove(food);
                LaneFood laneFood = foodFactory.Create(food, transform);
                ret.Add(laneFood);
            }
            return ret;
        }

        public void AddFoods(List<LaneFood> foods) {
            _foods.AddRange(foods.Select(x => x.deckFood));
            foreach (LaneFood laneFood in foods) {
                laneFood.deckFood.Releasable = this;
            }
        }
        
        
        
    }
}
