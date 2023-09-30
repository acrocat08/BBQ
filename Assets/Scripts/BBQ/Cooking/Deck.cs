using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BBQ.Action;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using SoundMgr;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace BBQ.Cooking {
    public class Deck : MonoBehaviour, IReleasable {


        private List<(DeckFood, DeckFood)> _allFoods;
        private LinkedList<DeckFood> _foods;
        [SerializeField] private FoodObjectFactory foodFactory;
        [SerializeField] DeckView view;

        [SerializeField] private DesignParam param;

        public void Init(List<DeckFood> deckFoods) {
            _foods = new LinkedList<DeckFood>(deckFoods.OrderBy(_ => Guid.NewGuid()));
            _allFoods = deckFoods.Select(x => (x, x.Copy())).ToList();
            foreach (DeckFood deckFood in deckFoods) {
                deckFood.Releasable = this;
            }
            foreach (DeckFood food in _foods) {
                TriggerObserver.I.RegisterFood(food);
            }
            view.UpdateText(this);
        }

        public void RegisterFood(DeckFood deckFood) {
            _allFoods.Add((deckFood, deckFood.Copy()));
            TriggerObserver.I.RegisterFood(deckFood);
            view.UpdateText(this);
        }

        public List<DeckFood> SelectAll() {
            return new List<DeckFood>(_foods);
        }
        
        public List<FoodObject> TakeFood(int num) {
            List<FoodObject> taken = new List<FoodObject>();
            for (int i = 0; i < num; i++) {
                DeckFood target = _foods.First();
                _foods.RemoveFirst();
                FoodObject laneFood = foodFactory.Create(target, transform);
                taken.Add(laneFood);
            }
            view.Draw(this);
            view.UpdateText(this);
            return taken;
        }

        public async UniTask AddFoods(List<FoodObject> foods) {
            _foods.AddRange(foods.Select(x => x.deckFood));
            _foods = new LinkedList<DeckFood>(_foods.OrderBy(_ => Guid.NewGuid()));
            List<UniTask> tasks = new List<UniTask>();
            foreach (FoodObject food in foods) {
                tasks.Add(view.AddFood(this, food));
                food.deckFood.Releasable = this;
            }
            await tasks;
            foreach (FoodObject food in foods) {
                Destroy(food.gameObject);
            }
            view.UpdateText(this);
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

        public async UniTask ResetEgg(Board board) {
            List<FoodObject> egg = ReleaseFoods(new List<DeckFood> { new DeckFood(param.resetFood) });
            TriggerObserver.I.RegisterFood(egg[0].deckFood);
            SoundPlayer.I.Play("se_resetEgg");
            await board.AddFoodsRandomly(egg);
        }
    }
}
