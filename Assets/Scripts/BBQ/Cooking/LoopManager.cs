using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Action;
using BBQ.Common;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Cooking {
    public class LoopManager : MonoBehaviour {
        [SerializeField] private CookTime time;
        [SerializeField] private LaneMovement[] movements;
        [SerializeField] private Lane[] lanes;
        [SerializeField] private float[] loopLines;
        private FoodObject[] _looped;
        private bool _pauseMode;
 
        void Start() {
            _looped = new FoodObject[] { null, null, null };
            MoveLanes();
        }

        async void MoveLanes() {
            while (true) {
                await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
                if (_pauseMode) continue;
                List<DeckFood> loopedFoods = new List<DeckFood>();
                for (int i = 0; i < movements.Length; i++) {
                    movements[i].Move();
                    FoodObject looped = GetLoopedFood(i);
                    if (looped != _looped[i] && looped != null) {
                        loopedFoods.Add(looped.deckFood);
                    }
                    _looped[i] = looped;
                }
                if(loopedFoods.Count > 0) await ExecuteLoop(loopedFoods);
            }
        }

        async UniTask ExecuteLoop(List<DeckFood> deckFoods) {
            time.Pause();
            await TriggerObserver.I.Invoke(ActionTrigger.Loop, deckFoods, true);
            await TriggerObserver.I.Invoke(ActionTrigger.LoopOthers, deckFoods, false);
            time.Resume();
        }

        FoodObject GetLoopedFood(int index) {
            return lanes[index]
                .GetFoods()
                .Where(x => x != null)
                .FirstOrDefault(x => Mathf.Abs(x.transform.localPosition.x - loopLines[index]) <= 50f);
        }
        
        public void SetPauseMode(bool mode) {
            _pauseMode = mode;
        }
        
    }
}
