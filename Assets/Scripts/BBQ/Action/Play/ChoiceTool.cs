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
            ToolData tool = itemSet.GetRandomTool(env.shop.GetShopLevel());
            v.s1 = tool.toolName;
        }
    }
}