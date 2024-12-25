namespace BBQ.Common {
    public class Score {
        public int basePoint;
        public int difficulty;
        public int mission;
        public int life;
        public int great;
        public int help;
        public int shopping;


        public int GetSum() {
            return basePoint + difficulty + mission + life + great + help + shopping;
        }
    }
}
