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
            MissionStatus mission = new MissionStatus {
                mission = hand,
                goal = MakeDifficulty(day, failed)
            };
            return new List<MissionStatus> { mission };
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
            return 6 + (day - failed) * 2 + failed;
        }

    }
}



