using System.Collections.Generic;
using BBQ.PlayData;
using UnityEngine;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "MissionMaker")]
    public class MissionMaker : ScriptableObject {

        [SerializeField] private Mission hand;

        public List<MissionStatus> Create(int day) {
            MissionStatus mission = new MissionStatus {
                mission = hand,
                goal = MakeDifficulty(day)
            };
            return new List<MissionStatus> { mission };
        }

        int MakeDifficulty(int day) {
            int x = day - 1;
            int ret = 10;
            ret += x;
            ret += Mathf.Max(0, x - 4);
            ret += Mathf.Max(0, x - 9);
            return ret;
        }

    }
}