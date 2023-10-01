using System;
using System.Collections.Generic;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.Database;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;

namespace BBQ.Action.Play {
    [CreateAssetMenu(menuName = "Action/AddShopTool")]
    public class AddShopTool : PlayAction {

        [SerializeField] private ItemSet itemSet;

        public override async UniTask Execute(ActionEnvironment env, ActionVariable v) {
            ToolData tool = itemSet.SearchTool(v.GetString(v.n1));
            SoundPlayer.I.Play("se_addShopFood");
            await env.shop.AddTool(tool);
        }
    }
}