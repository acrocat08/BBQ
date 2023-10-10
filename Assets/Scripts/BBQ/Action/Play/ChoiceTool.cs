using System;
using System.Collections.Generic;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.Database;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/ChoiceTool")]
    public class ChoiceTool : PlayAction {

        [SerializeField] private ItemSet itemSet;

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            int level = Mathf.Min(v.GetNum(v.n1), 5);
            ToolData tool = itemSet.GetRandomTool(level, level);
            v.s1 = tool.toolName;
        }
    }
}