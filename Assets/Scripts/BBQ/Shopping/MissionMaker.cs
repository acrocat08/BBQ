using System.Collections.Generic;
using BBQ.PlayData;
using UnityEngine;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "MissionMaker")]
    public class MissionMaker : ScriptableObject {

        [SerializeField] private Mission hand;
        [SerializeField] private int initialHand;
        [SerializeField] private int firstPoint;
        [SerializeField] private int secondPoint;

        public List<MissionStatus> Create(int day, int failed) {
            MissionStatus mission = new() {
                mission = hand,
                goal = MakeDifficulty(day, failed)
            };
            return new() { mission };
        }

        int MakeDifficulty(int day, int failed) {
            /*
            int x = day - 1;
            int ret = initialHand;
            ret += x;
            ret += Mathf.Max(0, x - firstPoint);
            ret += Mathf.Max(0, x - secondPoint);
            return ret;
            */
            int count = day - failed;
            
            if (PlayerConfig.GetGameMode() == GameMode.easy) {
                return 4 + count * 2;
            }
            if (PlayerConfig.GetGameMode() == GameMode.normal) {
                if (count <= 5) return 5 + count * 2;
                return count * 3;
            }
            if (PlayerConfig.GetGameMode() == GameMode.hard) {
                if (count <= 3) return 6 + count * 2;
                if (count <= 7) return 3 + count * 3;
                return -4 + count * 4;
            }
            return 0;
        }

    }
}



