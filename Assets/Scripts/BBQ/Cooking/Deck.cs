using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BBQ.Action;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using JetBrains.Annotations;
using SoundMgr;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace BBQ.Cooking {
    public class Deck : MonoBehaviour, IReleasable {


        private List<(DeckFood, DeckFood)> _allFoods;
        private LinkedList<DeckFood> _foods = new();
        [SerializeField] private FoodObjectFactory foodFactory;
        [SerializeField] DeckView view;

        [SerializeField] private DesignParam param;

        public void Init(List<DeckFood> deckFoods, bool doShuffle) {
            if (doShuffle) SortFoods(deckFoods);
            else _foods = new(deckFoods);
            _foods = new(_foods.Where(x => !x.isFrozen).ToList());
            _allFoods = deckFoods.Select(x => (x, x.CopyWithEffect())).ToList();
            foreach (DeckFood deckFood in deckFoods) {
                deckFood.Releasable = this;
            }

            TriggerObserver.I.Reset();
            foreach (DeckFood food in _foods) {
                TriggerObserver.I.RegisterFood(food);
            }
            view.UpdateText(this);
        }

        public void RegisterFood(DeckFood deckFood) {
            _allFoods.Add((deckFood, deckFood.CopyWithEffect()));
            TriggerObserver.I.RegisterFood(deckFood);
            view.UpdateText(this);
        }

        public List<DeckFood> SelectAll() {
            return new(_foods);
        }
        
        public List<FoodObject> TakeFood(int num) {
            List<FoodObject> taken = new();
            for (int i = 0; i < num; i++) {
                //if (_foods.All(x => x.isFrozen)) break;
                DeckFood target = _foods.FirstOrDefault(x => !x.isFrozen);
                if (target == null) break;
                _foods.Remove(target);
                FoodObject laneFood = foodFactory.Create(target, transform);
                taken.Add(laneFood);
            }
            view.Draw(this);
            view.UpdateText(this);
            return taken;
        }

        public async UniTask AddFoods(List<FoodObject> foods) {
            
            _foods.AddRange(foods.Where(x => x.deckFood.data != param.resetFood).Select(x => x.deckFood));
            SortFoods(_foods.Distinct().ToList());
            List<UniTask> tasks = new();
            foreach (FoodObject food in foods) {
                tasks.Add(view.AddFood(this, food));
                food.deckFood.Releasable = this;
            }
            view.Draw(this);
            await tasks;
            foreach (FoodObject food in foods) {
                Destroy(food.gameObject);
            }
            view.UpdateText(this);
        }

        public List<FoodObject> ReleaseFoods(List<DeckFood> foods) {
            List<FoodObject> ret = new();
            foreach (DeckFood food in foods) {
                _foods.Remove(food);
                FoodObject laneFood = foodFactory.Create(food, transform);
                ret.Add(laneFood);
            }
            view.Draw(this);
            view.UpdateText(this);
            return ret;
        }

        public void RemoveEffect(DeckFood deckFood) {
            var target = _allFoods.First(x => x.Item1 == deckFood);
            target.Item2.effect = null;
        }
        
        public FoodObject GetObject(DeckFood food) {
            return null;
        }

        public List<DeckFood> GetUsableFoods() {
            var usable = _allFoods.Where(x => CheckUsable(x.Item1)).ToList();
            foreach (var tuple in usable) {
                tuple.Item2.stack = tuple.Item1.stack;
            }
            return usable.Select(x => x.Item2).ToList();
        }
        
        public int Count() {
            return _allFoods.Count(x => CheckCountable(x.Item1));
        }

        private bool CheckCountable(DeckFood deckFood) {
            return !deckFood.isFired && !deckFood.isFrozen;
        }

        private bool CheckUsable(DeckFood deckFood) {
            return !deckFood.isFired && !deckFood.isEphemeral;
        }



        void SortFoods(List<DeckFood> target) {
            List<DeckFood> rantanFoods = target.Where(x => x.isRantan).OrderBy(_ => Guid.NewGuid()).ToList();
            List<DeckFood> others = target.Where(x => !x.isRantan).OrderBy(_ => Guid.NewGuid()).ToList();
            _foods = new(rantanFoods.Concat(others));
        }
    }
}
