using UnityEngine;

namespace BBQ.PlayData {
    [CreateAssetMenu(menuName = "Database/Mission")]
    public class Mission: ScriptableObject {
        public string missionName;
        public string detail;
    }
}
