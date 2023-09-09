using System.Collections.Generic;
using BBQ.Cooking;
using UnityEngine;

namespace BBQ.PlayData {
    public class PlayerStatus {
        private static PlayerStatus _saveData;
        private List<DeckFood> _deckFoods;
        
        public static void Create(List<DeckFood> deckFoods) {
            _saveData = new PlayerStatus();
            _saveData._deckFoods = deckFoods;
        }

        public static List<DeckFood> GetDeckFoods() {
            if (_saveData == null) return null;
            return _saveData._deckFoods;
        }

    }
}
