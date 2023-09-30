using System.Collections.Generic;
using BBQ.Action;
using BBQ.Common;
using BBQ.Database;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Cooking {
    public class HelpAction : MonoBehaviour {
        [SerializeField] private List<ActionCommand> addHand;
        [SerializeField] private List<ActionCommand> draw;
        [SerializeField] private ActionAssembly assembly;
        [SerializeField] private ActionEnvironment env;
        [SerializeField] private DesignParam param;
        [SerializeField] private Text handText;
        [SerializeField] private Text drawText;

        private bool _isRunning;

        public void Start() {
            addHand[0].n1 = (param.helpHandPenalty * -1).ToString();
            draw[0].n1 = (param.helpDrawPenalty * -1).ToString();
            handText.text = (param.helpHandPenalty * -1).ToString();
            drawText.text = (param.helpDrawPenalty * -1).ToString();
        }
         
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