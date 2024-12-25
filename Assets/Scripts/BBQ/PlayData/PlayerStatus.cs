using System.Collections.Generic;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.Database;
using UnityEngine;

namespace BBQ.PlayData {
    public class PlayerStatus {
        private static PlayerStatus _saveData;
        private List<List<DeckFood>> _deckFoods;
        private int _coin;
        private int _hand;
        private int _carbon;
        private int _day;
        private int _shopLevel;
        private int _levelUpDiscount;
        private int _rerollTicket;
        private bool _pigFlag;
        private int _additionalTime;
        private int _helpPenaltyReduce;
        private bool _rantanFlag;
        private int _star;
        private int _life;
        private List<MissionStatus> _nowMission;
        private int _failed;
        private int _gameStatus;
        private Score _score;
        private List<FoodData> _frozen;
        
        public static void Create(List<List<DeckFood>> deckFoods, int coin, int hand, int carbon, int day, int shopLevel, 
            int levelUpDiscount, int rerollTicket, bool pigFlag, int additionalTime, int helpPenaltyReduce, bool rantanFlag,
            int star, int life, List<MissionStatus> nowMission, int failed, int gameStatus, Score score, List<FoodData> frozen) {
            _saveData = new();
            _saveData._deckFoods = deckFoods;
            _saveData._coin = coin;
            _saveData._hand = hand;
            _saveData._carbon = carbon;
            _saveData._day = day;
            _saveData._shopLevel = shopLevel;
            _saveData._levelUpDiscount = levelUpDiscount;
            _saveData._rerollTicket = rerollTicket;
            _saveData._pigFlag = pigFlag;
            _saveData._additionalTime = additionalTime;
            _saveData._helpPenaltyReduce = helpPenaltyReduce;
            _saveData._rantanFlag = rantanFlag;
            _saveData._star = star;
            _saveData._life = life;
            _saveData._nowMission = nowMission;
            _saveData._failed = failed;
            _saveData._gameStatus = gameStatus;
            _saveData._score = score;
            _saveData._frozen = frozen;
        }

        public static void Reset() {
            _saveData = null;
        }

        public static List<List<DeckFood>> GetDeckFoods() {
            if (_saveData == null) return new();
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
            if (_saveData == null) return 1;
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

        public static bool GetPigFlag() {
            if (_saveData == null) return false;
            return _saveData._pigFlag;
        }
        

        public static int GetAdditionalTime() {
            if (_saveData == null) return 0;
            return _saveData._additionalTime;
        }

        public static int GetHelpPenaltyReduce() {
            if (_saveData == null) return 0;
            return _saveData._helpPenaltyReduce;
        }
        
        public static bool GetRantanFlag() {
            if (_saveData == null) return false;
            return _saveData._rantanFlag;
        }
        
        public static int GetStar() {
            if (_saveData == null) return 0;
            return _saveData._star;
        }
        
        public static int GetLife() {
            if (_saveData == null) return 5; //TODO:fix
            return _saveData._life;
        }

        public static int GetGameStatus() {
            if (_saveData == null) return 0;
            return _saveData._gameStatus;
        }
        
        public static List<MissionStatus> GetNowMission() {
            if (_saveData == null) return new(); //TODO:fix
            return _saveData._nowMission;
        }

        public static int GetFailed() {
            if (_saveData == null) return 0;
            return _saveData._failed;
        }

        public static Score GetScore() {
            if (_saveData == null) return new();
            return _saveData._score;
        }
        
        public static List<FoodData> GetFrozen() {
            if (_saveData == null) return new();
            return _saveData._frozen;
        }
    }
    
}
