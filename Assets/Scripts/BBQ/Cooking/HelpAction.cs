using System.Collections.Generic;
using BBQ.Action;
using BBQ.Common;
using UnityEngine;

namespace BBQ.Cooking {
    public class HelpAction : MonoBehaviour {
        [SerializeField] private List<ActionCommand> addHand;
        [SerializeField] private List<ActionCommand> draw;
        [SerializeField] private ActionAssembly assembly;
        [SerializeField] private ActionEnvironment env;

        private bool _isRunning;
         
        public async void OnAddHand() {
            if (env.time.GetNowTime() == 0) return;
            if (InputGuard.Guard()) return;
            _isRunning = true;
            env.time.Pause();
            await assembly.Run(addHand, env, null, null);
            env.time.Resume();
            _isRunning = false;
        }
        
        public async void OnDraw() {
            if (env.time.GetNowTime() == 0) return;
            if (InputGuard.Guard()) return;
            _isRunning = true;
            env.time.Pause();
            await assembly.Run(draw, env, null, null);
            env.time.Resume();
            _isRunning = false;
        }

    }
}