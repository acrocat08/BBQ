using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using BBQ.Action;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BBQ.Cooking {
    public class Board : MonoBehaviour, IReleasable {

        [SerializeField] private HandFactory handFactory;
        [SerializeField] private Vector3 handInitialPos;

        private List<DeckFood> _foods;
        private Hand _hand;
        private List<LaneFood> _hittingFoods;
        
        private List<Lane> _lanes;
        private Dump _dump;
        private HandCount _handCount;
        private CookTime _time;

        public void Init(List<Lane> lanes, Dump dump, HandCount handCount, CookTime time) {
            _foods = new List<DeckFood>();
            _lanes = lanes;
            _dump = dump;
            _handCount = handCount;
            _hand = null;
            _time = time;
            StoreHand();
            Pause();
        }

        public void Pause() {
            if(_hand != null) _hand.SetPauseMode(true);
            foreach (var lane in _lanes) {
                lane.SetPauseMode(true);
            }
        }
        
        public void Resume() {
            if(_hand != null) _hand.SetPauseMode(false);
            foreach (var lane in _lanes) {
                lane.SetPauseMode(false);
            }
        }
        
        //-- TODO: 別クラスに移行　HandManager

        public void UseHand() {
            _handCount.Use(1);
            StoreHand();
        }

        public void StoreHand() {
            if (_handCount.GetHandCount() > 0) {
                CreateHand();
            }
        }

        private void CreateHand() {
            Hand hand = handFactory.Create(this, _dump, _lanes, _time);
            _hand = hand;
            _hand.transform.SetParent(transform.Find("HandContainer"));
            _hand.transform.localPosition = handInitialPos;
        }
        
        public void DiscardHand() {
            if(_hand != null) Destroy(_hand.gameObject);
        }
        
        public void SetHittingFoods(List<LaneFood> hittingFoods) {
            _hittingFoods = hittingFoods;
        }

        public List<DeckFood> GetHittingFoods() {
            return _hittingFoods.Select(x => x.deckFood).ToList();
        }
        
        //---

        public List<DeckFood> SelectAll() {
            return new List<DeckFood>(_foods);
        }

        public List<LaneFood> ReleaseFoods(List<DeckFood> foods) {
            List<LaneFood> ret = new List<LaneFood>();
            foreach (Lane lane in _lanes) {
                ret.AddRange(lane.ReleaseFood(foods));
            }
            foreach (DeckFood food in foods) {
                _foods.Remove(food);
            }
            return ret;
        }
        
        public async UniTask AddFoodsRandomly(List<LaneFood> foods) {
            _foods.AddRange(foods.Select(x => x.deckFood));
            List<UniTask> tasks = new List<UniTask>();
            foreach (LaneFood food in foods) {
                food.deckFood.Releasable = this;
                Lane target = _lanes.Where(x => x.GetFoodsNum() < 5)
                    .OrderBy(x => Guid.NewGuid()).First();
                tasks.Add(target.AddFoodRandomly(food));
            }
            await tasks;
        }

        public async UniTask AddFoodsRandomly(List<LaneFood> foods, int index) {
            _foods.AddRange(foods.Select(x => x.deckFood));
            List<UniTask> tasks = new List<UniTask>();
            foreach (LaneFood food in foods) {
                food.deckFood.Releasable = this;
                tasks.Add(_lanes[index].AddFoodRandomly(food));
            }
            await tasks;
        }
        

        public LaneFood FindLaneFood(DeckFood food) {
            return _lanes
                .SelectMany(x => x.GetFoods())
                .Concat(_hittingFoods)
                .Where(x => x != null)
                .FirstOrDefault(x => x.deckFood == food);
        }

        public int GetLaneIndex(DeckFood food) {
            Lane lane = _lanes
                .First(x => x.GetFoods().Where(x => x != null).Select(x => x.deckFood).Contains(food));
            return _lanes.IndexOf(lane);
        }


        
    }
}
