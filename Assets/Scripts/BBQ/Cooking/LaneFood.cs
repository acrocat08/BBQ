using System.Collections;
using System.Collections.Generic;
using BBQ.Action;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;

namespace BBQ.Cooking {
    public class LaneFood : FoodObject {
        
        public override async UniTask LankUp() {
            await view.LankUp(this);
        }
        
    }
    
}