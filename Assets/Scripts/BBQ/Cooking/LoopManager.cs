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
        [SerializeField] private float[] speedPerSecond;
        private FoodObject[] _looped;
        private bool _pauseMode = true;
        private int[] speedLevel;
        
 
        public void Init() {
            _pauseMode = false;
            _looped = new FoodObject[] { null, null, null };
            speedLevel = new [] { 0, 0, 0 };
            for (int i = 0; i < 3; i++) {
                float speed = i == 1 ? -speedPerSecond[0] : speedPerSecond[0];
                movements[i].SetSpeed(speed / (PlayerConfig.GetGameMode() == GameMode.easy ? 1.25f : 1f));
            }
        }
        
        private async void FixedUpdate() {
            if (_pauseMode) return;
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
        
        public async void SetSpeed(int index, int offset, int limit=1000) {
            speedLevel[index] = Mathf.Min(speedPerSecond.Length - 1, speedLevel[index] + offset);
            float speed = speedPerSecond[speedLevel[index]];
            if (index == 1) speed *= -1;
            movements[index].SetSpeed(speed / (PlayerConfig.GetGameMode() == GameMode.easy ? 1.25f : 1f));
            await UniTask.Delay(TimeSpan.FromSeconds(limit));
            speedLevel[index] = Mathf.Max(0, speedLevel[index] - offset);
            speed = speedPerSecond[speedLevel[index]];
            if (index == 1) speed *= -1;
            movements[index].SetSpeed(speed / (PlayerConfig.GetGameMode() == GameMode.easy ? 1.25f : 1f));
        }
        
    }
}
