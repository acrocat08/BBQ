using System;
using System.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Cooking {
    public class CookTime : MonoBehaviour {
        [SerializeField] CookTimeView view;
        [SerializeField] private CookingGame game;
        [SerializeField] private Board board;
        
        
        private int _nowTime;
        private int _bonusTime;
        private int _semaphore;
        private bool _bonusMode;
        
        public void Init(int maxTime) {
            _nowTime = maxTime;
            _bonusTime = 0;
            _semaphore = 0;
            _bonusMode = false;
            view.UpdateText(this, _bonusMode);
            CountDown();
        }

        private async void CountDown() {
            while (_nowTime > 0) {
                if (_semaphore > 0) {
                    await UniTask.DelayFrame(1);
                    continue;
                }
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
                _nowTime -= 1;
                if (_nowTime == 0 && !_bonusMode) {
                    _nowTime += _bonusTime;
                    _bonusMode = true;
                }
                view.UpdateText(this, _bonusMode);
            }

            while (_semaphore > 0) {
                await UniTask.DelayFrame(1);
            }
            game.GameEnd();
        }

        public void Pause() {
            _semaphore++;
            if(_semaphore > 0) board.Pause();
        }

        public void Resume() {
            _semaphore--;
            if(_semaphore == 0) board.Resume();
        }

        public int GetNowTime() {
            return _nowTime;
        }

        public int GetBonusTime() {
            return _bonusTime;
        }

        public void UseTime(int val) {
            _nowTime = Mathf.Max(_nowTime - val, 1);
            view.UpdateTime(this, _bonusMode);
        }

        public void AddTime(int val) {
            if (_bonusMode) return;
            _bonusTime += val;
            view.UpdateTime(this, _bonusMode);
        }
        
    }
}
