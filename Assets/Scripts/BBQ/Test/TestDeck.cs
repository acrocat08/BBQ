using System.Collections.Generic;
using BBQ.PlayData;
using UnityEngine;

namespace BBQ.Test {
    [CreateAssetMenu(menuName = "TestDeck")]
    public class TestDeck : ScriptableObject {
        public List<DeckFood> foods;
    }
}
