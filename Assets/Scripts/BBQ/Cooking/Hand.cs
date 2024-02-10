using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Action;
using BBQ.Common;
using BBQ.PlayData;
using BBQ.Tutorial;
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
        [SerializeField] private List<ActionCommand> reset;
        private Board _board;
        private Dump _dump;
        private CookTime _time;
        private MissionSheet _missionSheet;
        private ActionEnvironment _env;
        private TutorialCooking _tutorial;
        

        private bool _afterShot;
        private bool _isGolden;
        private bool _isDouble;
        
        
        void Awake() {
            _afterShot = false;
            _isGolden = true;
        }

        public void Init(Board board, Dump dump, List<Lane> lanes, CookTime time, MissionSheet missionSheet, ActionEnvironment env,
            bool isGolden, bool isDouble, TutorialCooking tutorial) {
            _board = board;
            _dump = dump;
            _time = time;
            _missionSheet = missionSheet;
            _env = env;
            _isGolden = isGolden;
            _isDouble = isDouble;
            _tutorial = tutorial;
            shot.Init(lanes);
            view.Double(this, isDouble);
            view.Golden(this, isGolden, isDouble);
        }

        void Update() {
            transform.localScale = Vector3.one;
            if (!CheckUsable()) return;
            movement.MoveDelta();
            
            if (Input.GetMouseButtonUp(0) && movement.CheckIsInnerBorder()) {
                OnShot();
                _afterShot = true;
            }
        }
        
        async void OnShot() {
            _time.Pause();
            List<FoodObject> hitFoods = await shot.Shot(_isDouble);
            List<DeckFood> deckFoods = hitFoods.Where(x => x).Select(x => x.deckFood).ToList();
            
            _board.ReleaseFoods(deckFoods);
            _board.UseHand();
            _dump.HitFoods(hitFoods);

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            await TriggerObserver.I.Invoke(ActionTrigger.Hit, deckFoods, true);
            await TriggerObserver.I.Invoke(ActionTrigger.HitOthers, deckFoods, false);

            if (_isGolden) {
                if (deckFoods.Count == 3) {
                    SoundPlayer.I.Play("se_goldenBonus");    
                    param.Create("Bonus!!", null);
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                    List<ActionCommand> _bonus = bonus[Random.Range(0, bonus.Count)].commands;
                    await assembly.Run(_bonus, _env, null, new List<DeckFood>());
                    if (_isDouble) {
                        List<ActionCommand> _bonus2 = bonus[Random.Range(0, bonus.Count)].commands;
                        await assembly.Run(_bonus2, _env, null, new List<DeckFood>());
                    }
                }
            }
            await TriggerObserver.I.Invoke(ActionTrigger.AfterHit, deckFoods.Where(x => !x.isFired && !x.isFrozen).ToList(), true);

            if (_env.resetFlag) {
                _env.resetFlag = false;
                await assembly.Run(reset, _env, null, new List<DeckFood>());
            }
            
            
            if (!_env.isShopping && _env.deck.SelectAll().Count == 0 
                                && _env.board.SelectAll().Count < 15 && !_env.board.HasResetEgg() && !_env.dump.HasResetEgg()) {
                await _env.board.ResetEgg();
            }
            
            foreach (var food in hitFoods) {
                if(food == null) continue;
                food.transform.SetParent(transform, true);
            }
            _time.Resume();
            
            if (deckFoods.Count > 0 && _missionSheet != null) {
                _missionSheet.AddCount("hand", _isDouble ? 2 : 1);
                _missionSheet.AddKushi(hitFoods[0]?.deckFood.data, hitFoods[1]?.deckFood.data, hitFoods[2]?.deckFood.data);
            }

            if (_tutorial != null) {
                _tutorial.SendHittingFoods(deckFoods);
            }
            
            await view.AfterHit(this);
            //if(_tutorial != null) _tutorial
            Destroy(gameObject);
        }

        private bool CheckUsable() {
            return !InputGuard.Guard() && !_afterShot;
        }
        
        
    }
}