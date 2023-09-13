using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Action;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace BBQ.Cooking {
    public class Deck : MonoBehaviour, IReleasable {


        private List<(DeckFood, DeckFood)> _allFoods;
        private LinkedList<DeckFood> _foods;
        [SerializeField] private LaneFoodFactory foodFactory;

        [SerializeField] DeckView view;

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

        public List<DeckFood> SelectAll() {
            return new List<DeckFood>(_foods);
        }
        
        public List<LaneFood> TakeFood(int num) {
            List<LaneFood> taken = new List<LaneFood>();
            for (int i = 0; i < num; i++) {
                DeckFood target = _foods.First();
                _foods.RemoveFirst();
                LaneFood laneFood = foodFactory.Create(target, transform);
                taken.Add(laneFood);
            }
            view.Draw(this);
            view.UpdateText(this);
            return taken;
        }

        public async UniTask AddFoods(List<LaneFood> foods) {
            _foods.AddRange(foods.Select(x => x.deckFood));
            List<UniTask> tasks = new List<UniTask>();
            foreach (LaneFood food in foods) {
                tasks.Add(view.AddFood(this, food));
                food.deckFood.Releasable = this;
            }
            await tasks;
            foreach (LaneFood food in foods) {
                Destroy(food.gameObject);
            }
            view.UpdateText(this);
        }

        public List<LaneFood> ReleaseFoods(List<DeckFood> foods) {
            return new List<LaneFood>();
        }

        public List<DeckFood> GetUsableFoods() {
            return _allFoods.Where(x => CheckUsable(x.Item1)).Select(x => x.Item2).ToList();
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
    }
}
