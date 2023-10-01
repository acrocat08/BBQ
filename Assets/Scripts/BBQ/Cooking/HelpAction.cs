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
        
        public void Init(int reduce) {
            int handPenalty = param.helpHandPenalty - reduce;
            int drawPenalty = param.helpDrawPenalty - reduce;

            addHand[0].n1 = (handPenalty * -1).ToString();
            draw[0].n1 = (drawPenalty * -1).ToString();
            handText.text = (handPenalty * -1).ToString();
            drawText.text = (drawPenalty * -1).ToString();
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