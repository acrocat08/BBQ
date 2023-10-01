using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using BBQ.Action;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using SoundMgr;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BBQ.Cooking {
    public class Board : MonoBehaviour, IReleasable {

        [SerializeField] private HandFactory handFactory;
        [SerializeField] private Vector3 handInitialPos;
        [SerializeField] private LoopManager loop;
        [SerializeField] private DesignParam param;

        private List<DeckFood> _foods;
        private Hand _hand;
        
        private List<Lane> _lanes;
        private Dump _dump;
        private HandCount _handCount;
        private CookTime _time;
        private MissionSheet _missionSheet;
        private ActionEnvironment _env;
        private bool _nextGold;
        private bool _nextDouble;

        public void Init(List<Lane> lanes, Dump dump, HandCount handCount, CookTime time, MissionSheet missionSheet, ActionEnvironment env) {
            _foods = new List<DeckFood>();
            _lanes = lanes;
            _dump = dump;
            _handCount = handCount;
            _hand = null;
            _time = time;
            _missionSheet = missionSheet;
            _env = env;
            _nextGold = false;
            
            StoreHand();
            Pause();
        }

        public void Pause() {
            loop.SetPauseMode(true);
        }
        
        public void Resume() {
            loop.SetPauseMode(false);
        }
        
        //-- TODO: 別クラスに移行　HandManager
        
        public void StoreHand() {
            if (_handCount.GetHandCount() > 0 && _hand == null) {
                CreateHand();
            }
        }

        private void CreateHand() {
            Hand hand = handFactory.Create(this, _dump, _lanes, _time, _missionSheet, _env, _nextGold, _nextDouble);
            if (_nextGold) {
                _nextGold = false;
                SoundPlayer.I.Play("se_goldenHand");                
            }
            if (_nextDouble && _handCount.GetHandCount() >= 2) {
                _handCount.Use(2);
                _nextDouble = false;
                SoundPlayer.I.Play("se_doubleHand");                
            }
            else _handCount.Use(1);
            _hand = hand;
            _hand.transform.SetParent(transform.Find("HandContainer"));
            _hand.transform.localPosition = handInitialPos;
        }

        public void UseHand() {
            _hand = null;
        }
        
        //---

        public List<DeckFood> SelectAll() {
            return new List<DeckFood>(_foods.Where(x => x.data != param.resetFood));
        }

        public List<DeckFood> SelectLane(int index) {
            if (index == 0) return new List<DeckFood>();
            return _lanes[index - 1].GetFoods().Where(x => x != null && x.deckFood.data != param.resetFood)
                .Select(x => x.deckFood).ToList();
        }

        public int GetFoodNum(int index) {
            if (index == 0) return 0;
            return _lanes[index - 1].GetFoods().Count(x => x != null);
        }

        public List<FoodObject> ReleaseFoods(List<DeckFood> foods) {
            List<FoodObject> ret = new List<FoodObject>();
            foreach (Lane lane in _lanes) {
                ret.AddRange(lane.ReleaseFood(foods));
            }
            foreach (DeckFood food in foods) {
                _foods.Remove(food);
            }
            return ret;
        }

        public FoodObject GetObject(DeckFood food) {
            return _lanes
                .SelectMany(x => x.GetFoods())
                .Where(x => x != null)
                .FirstOrDefault(x => x.deckFood == food);
        }

        public async UniTask AddFoodsRandomly(List<FoodObject> foods) {
            _foods.AddRange(foods.Select(x => x.deckFood));
            List<UniTask> tasks = new List<UniTask>();
            foreach (FoodObject food in foods) {
                food.deckFood.Releasable = this;
                Lane target = _lanes.Where(x => x.GetFoodsNum() < 5)
                    .OrderBy(x => Guid.NewGuid()).First();
                tasks.Add(target.AddFoodRandomly(food));
            }
            await tasks;
        }

        public async UniTask AddFoodsRandomly(List<FoodObject> foods, int index) {
            _foods.AddRange(foods.Select(x => x.deckFood));
            List<UniTask> tasks = new List<UniTask>();
            foreach (FoodObject food in foods) {
                food.deckFood.Releasable = this;
                tasks.Add(_lanes[index - 1].AddFoodRandomly(food));
            }
            await tasks;
        }

        public int GetLaneIndex(DeckFood food) {
            Lane lane = _lanes
                .FirstOrDefault(x => x.GetFoods().Where(x => x != null).Select(x => x.deckFood).Contains(food));
            if (lane == null) return 0;
            return _lanes.IndexOf(lane) + 1;
        }

        public void SetGold() {
            _nextGold = true;
        }
        
        public void SetDouble() {
            _nextDouble = true;
        }
        
    }
}
