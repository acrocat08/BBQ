using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace BBQ.Cooking {
    public class Lane : MonoBehaviour {
        private List<FoodObject> _foods;
        [SerializeField] private ViewParam param;
        [SerializeField] private LaneMovement movement;
        [SerializeField] private DesignParam designParam;
        void Awake() {
            _foods = new List<FoodObject>();
            for (int i = 0; i < param.foodMaxNumInLane; i++) {
                _foods.Add(null);
            }
        }

        public async UniTask AddFood(FoodObject food, int index) {
            _foods[index] = food;
            await movement.AddFood(food, index);
        }

        public async UniTask AddFoodRandomly(FoodObject food) {
            int index = Enumerable.Range(0, _foods.Count).Where(x => _foods[x] == null)
                .OrderBy(x => Guid.NewGuid()).First();
            await AddFood(food, index);
        }


        public List<FoodObject> GetFoods() {
            List<FoodObject> ret = new List<FoodObject>(_foods);
            return ret;
        }

        public FoodObject SearchNearestFood(float x) {
            for (int i = 0; i < param.foodMaxNumInLane; i++) {
                if (_foods[i] == null) continue;
                if (Mathf.Abs(_foods[i].transform.position.x - x) < param.foodCollisionSize * (Screen.width / 1920f)) {
                    return _foods[i];
                }
            }

            return null;
        }

        public void ReleaseFood(FoodObject food) {
            int index = _foods.IndexOf(food);
            _foods[index] = null;
        }
        
        public List<FoodObject> ReleaseFood(List<DeckFood> foods) {
            List<FoodObject> ret = _foods
                .Where(x => x != null)
                .Where(x => foods.Contains(x.deckFood)).ToList();
            foreach (FoodObject r in ret) {
                ReleaseFood(r);
            }
            return ret;
        }

        public int GetFoodsNum() {
            return _foods.Count(x => x != null);
        }

        public void Reset() {
            foreach (FoodObject foodObject in _foods) {
                if(foodObject != null) Destroy(foodObject.gameObject);
            }

            _foods = new List<FoodObject>();
            for (int i = 0; i < param.foodMaxNumInLane; i++) {
                _foods.Add(null);
            }
        }
    }
}