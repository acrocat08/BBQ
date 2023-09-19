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
                goal = 9 + day * 1
            };
            return new List<MissionStatus> { mission };
        }

    }
}
