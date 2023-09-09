using System.Collections.Generic;
using BBQ.Cooking;
using UnityEngine;

namespace BBQ.PlayData {
    public class PlayerStatus {
        private static PlayerStatus _saveData;
        private List<DeckFood> _deckFoods;
        private int _coin;
        
        public static void Create(List<DeckFood> deckFoods, int coin) {
            _saveData = new PlayerStatus();
            _saveData._deckFoods = deckFoods;
            _saveData._coin = coin;
        }

        public static List<DeckFood> GetDeckFoods() {
            if (_saveData == null) return null;
            return _saveData._deckFoods;
        }

        public static int GetCoin() {
            if (_saveData == null) return 0;
            return _saveData._coin;
        }

    }
}
