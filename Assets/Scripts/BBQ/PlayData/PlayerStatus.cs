using System.Collections.Generic;
using BBQ.Cooking;
using UnityEngine;

namespace BBQ.PlayData {
    public class PlayerStatus {
        private static PlayerStatus _saveData;
        private List<DeckFood> _deckFoods;
        private int _coin;
        private int _hand;
        private int _carbon;
        private int _day;
        private int _shopLevel;
        private int _levelUpDiscount;
        private int _rerollTicket;
        private int _star;
        private int _life;
        private List<MissionStatus> _nowMission;
        private int _gameStatus;
        
        public static void Create(List<DeckFood> deckFoods, int coin, int hand, int carbon, int day, int shopLevel, 
            int levelUpDiscount, int rerollTicket, int star, int life, List<MissionStatus> nowMission, int gameStatus) {
            _saveData = new PlayerStatus();
            _saveData._deckFoods = deckFoods;
            _saveData._coin = coin;
            _saveData._hand = hand;
            _saveData._carbon = carbon;
            _saveData._day = day;
            _saveData._shopLevel = shopLevel;
            _saveData._levelUpDiscount = levelUpDiscount;
            _saveData._rerollTicket = rerollTicket;
            _saveData._star = star;
            _saveData._life = life;
            _saveData._nowMission = nowMission;
            _saveData._gameStatus = gameStatus;
        }

        public static void Reset() {
            _saveData = null;
        }

        public static List<DeckFood> GetDeckFoods() {
            if (_saveData == null) return null;
            return _saveData._deckFoods;
        }

        public static int GetCoin() {
            if (_saveData == null) return 0;
            return _saveData._coin;
        }
        
        public static int GetHand() {
            if (_saveData == null) return 5;
            return _saveData._hand;
        }
        
        public static int GetCarbon() {
            if (_saveData == null) return 0;
            return _saveData._carbon;
        }


        public static int GetDay() {
            if (_saveData == null) return 1;
            return _saveData._day;
        }

        public static int GetShopLevel() {
            if (_saveData == null) return 5;
            return _saveData._shopLevel;
        }

        public static int GetLevelUpDiscount() {
            if (_saveData == null) return 0;
            return _saveData._levelUpDiscount;
        }

        public static int GetRerollTicket() {
            if (_saveData == null) return 0;
            return _saveData._rerollTicket;
        }
        
        public static int GetStar() {
            if (_saveData == null) return 0;
            return _saveData._star;
        }
        
        public static int GetLife() {
            if (_saveData == null) return 10; //TODO:fix
            return _saveData._life;
        }

        public static int GetGameStatus() {
            if (_saveData == null) return 0;
            return _saveData._gameStatus;
        }
        
        public static List<MissionStatus> GetNowMission() {
            if (_saveData == null) return new List<MissionStatus>(); //TODO:fix
            return _saveData._nowMission;
        }

    }
}
