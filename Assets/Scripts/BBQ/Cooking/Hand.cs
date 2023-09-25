using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Action;
using BBQ.Common;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Cooking {
    public class Hand : MonoBehaviour {
        [SerializeField] private HandView view;
        [SerializeField] private HandShot shot;
        [SerializeField] private HandMovement movement;
        private Board _board;
        private Dump _dump;
        private CookTime _time;
        private MissionSheet _missionSheet;
        

        private bool _pauseMode;
        private bool _afterShot;
        
        void Awake() {
            _pauseMode = false;
            _afterShot = false;
        }

        public void Init(Board board, Dump dump, List<Lane> lanes, CookTime time, MissionSheet missionSheet) {
            _board = board;
            _dump = dump;
            _time = time;
            _missionSheet = missionSheet;
            shot.Init(lanes);
        }

        void Update() {
            transform.localScale = Vector3.one;
            if (!CheckUsable()) return;
            movement.MoveDelta();
            
            if (Input.GetMouseButtonDown(0) && movement.CheckIsInnerBorder()) {
                OnShot();
                _afterShot = true;
            }
        }
        
        async void OnShot() {
            _time.Pause();
            List<FoodObject> hitFoods = await shot.Shot();
            List<DeckFood> deckFoods = hitFoods.Select(x => x.deckFood).Reverse().ToList();
            
            List<FoodObject> boardFoods = _board.ReleaseFoods(deckFoods);
            _dump.HitFoods(boardFoods);

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            await TriggerObserver.I.Invoke(ActionTrigger.Hit, deckFoods, true);

            
            foreach (var food in hitFoods) {
                food.transform.SetParent(transform);
            }
            
            _time.Resume();
            _board.UseHand();
            if (deckFoods.Count > 0) {
                _missionSheet.AddCount("hand", 1);
            }
            
            await view.AfterHit(this);
            Destroy(gameObject);
        }

        public void SetPauseMode(bool mode) {
            _pauseMode = mode;
        }

        private bool CheckUsable() {
            return !InputGuard.Guard() && !_afterShot;
        }
        
        
    }
}