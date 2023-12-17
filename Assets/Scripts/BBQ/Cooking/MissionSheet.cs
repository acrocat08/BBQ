using System.Collections.Generic;
using System.Linq;
using BBQ.PlayData;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace BBQ.Cooking {
    public class MissionSheet : MonoBehaviour {
        [SerializeField] private MissionSheetView view;

        private List<MissionStatus> _missions;

        public void Init(List<MissionStatus> missions) {
            _missions = missions;
            view.Init(this, _missions);
        }

        public void AddCount(string missionName, int num) {
            MissionStatus missionStatus = _missions.FirstOrDefault(x => x.mission.missionName == missionName);
            if (missionStatus == null) return;
            missionStatus.now += num;
            view.UpdateView(this, _missions);
        }

        public bool CheckMissionCleared() {
            return _missions.All(x => x.now >= x.goal);
        }

        public int GetScore() {
            int x = _missions.Sum(x => x.now - x.goal);
            int diff = x * (x > 0 ? 15 : 10);
            return Mathf.Clamp(100 + diff, 50, 300);
        }
    }
}
