using UnityEngine;

namespace BBQ.Common {
    public class Score {
        public int difficulty;
        public int mission;
        public int life;
        public int great;
        public int help;
        public int shopping;


        public int GetSum() {
            return Mathf.Min(300, difficulty + mission + life + great + help + shopping);
        }
    }
}
