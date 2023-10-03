using System;
using System.Collections.Generic;
using System.Data;
using BBQ.Action;
using BBQ.Common;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Cooking {
    public class CookTime : MonoBehaviour {
        [SerializeField] CookTimeView view;
        [SerializeField] private CookingGame game;
        [SerializeField] private Board board;
        
        
        private int _nowTime;
        private int _bonusTime;
        private bool _bonusMode;
        
        public void Init(int maxTime) {
            _nowTime = maxTime;
            _bonusTime = 0;
            _bonusMode = false;
            view.UpdateText(this, _bonusMode);
            CountDown();
        }

        private async void CountDown() {
            while (_nowTime >= 0) {
                if (InputGuard.Guard()) {
                    await UniTask.DelayFrame(1);
                    continue;
                }
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
                _nowTime -= 1;
                if (_nowTime <= 0 && !_bonusMode) {
                    _nowTime += _bonusTime;
                    _bonusMode = true;
                    Pause();
                    await TriggerObserver.I.Invoke(ActionTrigger.BonusTime, new List<DeckFood>(), false);
                    Resume();
                }
                if(_nowTime >= 0) view.UpdateText(this, _bonusMode);
            }

            while (InputGuard.Guard()) {
                await UniTask.DelayFrame(1);
            }
            game.GameEnd();
        }

        public void Pause() {
            InputGuard.Lock();
            if(InputGuard.Guard()) board.Pause();
        }

        public void Resume() {
            InputGuard.UnLock();
            if(!InputGuard.Guard()) board.Resume();
        }

        public int GetNowTime() {
            return _nowTime;
        }

        public int GetBonusTime() {
            return _bonusTime;
        }

        public void UseTime(int val) {
            _bonusTime = Mathf.Min(_bonusTime, Mathf.Max(_bonusTime - val + _nowTime, 0));
            _nowTime = Mathf.Max(_nowTime - val, 0);
            view.UpdateTime(this, _bonusMode);
        }

        public void AddTime(int val) {
            if (_bonusMode) return;
            _bonusTime += val;
            view.UpdateTime(this, _bonusMode);
        }

        public bool IsBonusMode() {
            return _bonusMode;
        }
        
    }
}
