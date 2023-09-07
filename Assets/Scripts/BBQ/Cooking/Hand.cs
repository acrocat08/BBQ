using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using BBQ.Action;
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


        private bool _pauseMode;
        private bool _afterShot;
        
        void Awake() {
            _pauseMode = false;
            _afterShot = false;
        }

        public void Init(Board board, Dump dump, List<Lane> lanes, CookTime time) {
            _board = board;
            _dump = dump;
            _time = time;
            shot.Init(lanes);
        }

        void Update() {
            if (!CheckUsable()) return;
            movement.MoveDelta();
            
            if (Input.GetMouseButtonDown(0) && movement.CheckIsInnerBorder()) {
                OnShot();
                _afterShot = true;
            }
        }
        
        async void OnShot() {
            _time.Pause();
            List<LaneFood> hitFoods = await shot.Shot();
            List<DeckFood> deckFoods = hitFoods.Select(x => x.deckFood).Reverse().ToList();
            
            List<LaneFood> boardFoods = _board.ReleaseFoods(deckFoods);
            _dump.AddFoods(boardFoods);

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            
            _board.SetHittingFoods(hitFoods);
            await TriggerObserver.I.Invoke(ActionTrigger.Hit, deckFoods, true);
            _board.SetHittingFoods(new List<LaneFood>());

            
            foreach (var food in hitFoods) {
                food.transform.SetParent(transform);
            }
            
            _time.Resume();
            _board.UseHand();
            
            await view.AfterHit(this);
            Destroy(gameObject);
        }

        public void SetPauseMode(bool mode) {
            _pauseMode = mode;
        }

        private bool CheckUsable() {
            return !_pauseMode && !_afterShot;
        }
        
        
    }
}