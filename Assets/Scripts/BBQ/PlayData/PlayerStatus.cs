using System.Collections.Generic;
using BBQ.Cooking;
using UnityEngine;

namespace BBQ.PlayData {
    public class PlayerStatus {
        private static PlayerStatus _saveData;
        private List<DeckFood> _deckFoods;
        private int _coin;
        private int _day;
        private int _shopLevel;
        private int _levelUpDiscount;
        
        public static void Create(List<DeckFood> deckFoods, int coin, int day, int shopLevel, int levelUpDiscount) {
            _saveData = new PlayerStatus();
            _saveData._deckFoods = deckFoods;
            _saveData._coin = coin;
            _saveData._day = day;
            _saveData._shopLevel = shopLevel;
            _saveData._levelUpDiscount = levelUpDiscount;
        }

        public static List<DeckFood> GetDeckFoods() {
            if (_saveData == null) return null;
            return _saveData._deckFoods;
        }

        public static int GetCoin() {
            if (_saveData == null) return 0;
            return _saveData._coin;
        }

        public static int GetDay() {
            if (_saveData == null) return 1;
            return _saveData._day;
        }

        public static int GetShopLevel() {
            if (_saveData == null) return 1;
            return _saveData._shopLevel;
        }

        public static int GetLevelUpDiscount() {
            if (_saveData == null) return 0;
            return _saveData._levelUpDiscount;
        }

    }
}
