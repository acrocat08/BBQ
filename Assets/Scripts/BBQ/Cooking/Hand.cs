using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Action;
using BBQ.Common;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BBQ.Cooking {
    public class Hand : MonoBehaviour {
        [SerializeField] private HandView view;
        [SerializeField] private ParamUpEffectFactory param; 
        [SerializeField] private HandShot shot;
        [SerializeField] private HandMovement movement;
        [SerializeField] private ActionAssembly assembly;
        [SerializeField] private List<ActionSequence> bonus;
        private Board _board;
        private Dump _dump;
        private CookTime _time;
        private MissionSheet _missionSheet;
        private ActionEnvironment _env;
        

        private bool _afterShot;
        private bool _isGolden;
        
        
        void Awake() {
            _afterShot = false;
            _isGolden = true;
        }

        public void Init(Board board, Dump dump, List<Lane> lanes, CookTime time, MissionSheet missionSheet, ActionEnvironment env, bool isGolden) {
            _board = board;
            _dump = dump;
            _time = time;
            _missionSheet = missionSheet;
            _env = env;
            _isGolden = isGolden;
            shot.Init(lanes);
            view.Golden(this, isGolden);
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

            if (_isGolden) {
                if (deckFoods.Count == 3) {
                    List<ActionCommand> _bonus = bonus[Random.Range(0, bonus.Count)].commands;
                    SoundPlayer.I.Play("se_goldenBonus");    
                    param.Create("Bonus!!", null);
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                    await assembly.Run(_bonus, _env, null, new List<DeckFood>());
                }
            }
            
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

        private bool CheckUsable() {
            return !InputGuard.Guard() && !_afterShot;
        }
        
        
    }
}