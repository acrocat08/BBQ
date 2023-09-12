using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace BBQ.Cooking {
    public class Deck : MonoBehaviour, IReleasable {


        private List<DeckFood> _allFoods;
        private LinkedList<DeckFood> _foods;
        [SerializeField] private LaneFoodFactory foodFactory;

        [SerializeField] DeckView view;

        public void Init(List<DeckFood> deckFoods) {
            _foods = new LinkedList<DeckFood>(deckFoods.OrderBy(x => Guid.NewGuid()));
            _allFoods = new List<DeckFood>(deckFoods);
            view.UpdateText(this);
            foreach (DeckFood deckFood in deckFoods) {
                deckFood.Releasable = this;
            }
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

        public List<DeckFood> GetAllFoods() {
            return _allFoods;
        }
    }
}
