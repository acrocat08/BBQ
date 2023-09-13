using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Cooking {
    public class Lane : MonoBehaviour {
        private List<LaneFood> _foods;
        [SerializeField] private CookingParam param;
        [SerializeField] private LaneMovement movement;
        void Awake() {
            _foods = new List<LaneFood>();
            for (int i = 0; i < param.foodMaxNumInLane; i++) {
                _foods.Add(null);
            }
        }

        public async UniTask AddFood(LaneFood food, int index) {
            _foods[index] = food;
            await movement.AddFood(food, index);
        }

        public async UniTask AddFoodRandomly(LaneFood food) {
            int index = Enumerable.Range(0, _foods.Count).Where(x => _foods[x] == null)
                .OrderBy(x => Guid.NewGuid()).First();
            await AddFood(food, index);
        }


        public List<LaneFood> GetFoods() {
            List<LaneFood> ret = new List<LaneFood>(_foods);
            return ret;
        }

        public LaneFood SearchNearestFood(float x) {
            for (int i = 0; i < param.foodMaxNumInLane; i++) {
                if (_foods[i] == null) continue;
                if (Mathf.Abs(_foods[i].transform.position.x - x) < param.foodCollisionSize) {
                    return _foods[i];
                }
            }

            return null;
        }

        public void ReleaseFood(LaneFood food) {
            int index = _foods.IndexOf(food);
            _foods[index] = null;
        }
        
        public List<LaneFood> ReleaseFood(List<DeckFood> foods) {
            List<LaneFood> ret = _foods
                .Where(x => x != null)
                .Where(x => foods.Contains(x.deckFood)).ToList();
            foreach (LaneFood r in ret) {
                ReleaseFood(r);
            }
            return ret;
        }

        public int GetFoodsNum() {
            return _foods.Count(x => x != null);
        }
        
    }
}