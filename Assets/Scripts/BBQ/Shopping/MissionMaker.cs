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

        public List<MissionStatus> Create(int day) {
            MissionStatus mission = new MissionStatus {
                mission = hand,
                goal = MakeDifficulty(day)
            };
            return new List<MissionStatus> { mission };
        }

        int MakeDifficulty(int day) {
            /*
            int x = day - 1;
            int ret = initialHand;
            ret += x;
            ret += Mathf.Max(0, x - firstPoint);
            ret += Mathf.Max(0, x - secondPoint);
            return ret;
            */
            return 6 + day * 2;
        }

    }
}



