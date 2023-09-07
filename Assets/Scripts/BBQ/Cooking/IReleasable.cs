using System.Collections.Generic;
using BBQ.PlayData;
using UnityEngine;

namespace BBQ.Cooking {
    public interface IReleasable {
        public List<LaneFood> ReleaseFoods(List<DeckFood> foods);
    }
}
