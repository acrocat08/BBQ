using System.Collections.Generic;
using BBQ.PlayData;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Cooking {
    [CreateAssetMenu(menuName = "MissionSheet/View")]
    public class MissionSheetView : ScriptableObject {

        [SerializeField] private GameObject prefab;

        public void Init(MissionSheet missionSheet, List<MissionStatus> missions) {
            Transform container = missionSheet.transform.Find("Container").Find("MissionList");
            foreach (MissionStatus mission in missions) {
                Transform obj = Instantiate(prefab).transform;
                obj.SetParent(container, false);
                UpdateText(obj, mission, false);
            }
        }
        
        public void UpdateView(MissionSheet missionSheet, List<MissionStatus> missions) {
            Transform container = missionSheet.transform.Find("Container").Find("MissionList");
            for (int i = 0; i < missions.Count; i++) {
                UpdateText(container.GetChild(i), missions[i], false);
            }
        }

        public void UpdateText(Transform mission, MissionStatus status, bool showFailIcon) {
            Text detail = mission.GetComponent<Text>();
            Text count = mission.Find("Count").GetComponent<Text>();
            string baseDetail = status.mission.detail;
            string[] split = baseDetail.Split("#");
            detail.text = "-　" + split[0] + status.goal + split[1];
            count.text = status.now + " / " + status.goal;
            mission.Find("Check").GetComponent<Image>().enabled = status.now >= status.goal;
            mission.Find("NG").GetComponent<Image>().enabled = status.now < status.goal && showFailIcon;
        }


    }
}
