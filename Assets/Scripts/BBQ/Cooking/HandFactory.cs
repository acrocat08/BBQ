using System.Collections.Generic;
using BBQ.Action;
using UnityEngine;

namespace BBQ.Cooking {
    [CreateAssetMenu(menuName = "Hand/Factory")]

    public class HandFactory : ScriptableObject {
        [SerializeField] private GameObject prefab;

        [SerializeField] private Vector3 pos;

        public Hand Create(Board board, Dump dump, List<Lane> lanes, CookTime time, MissionSheet missionSheet, ActionEnvironment env, bool isGolden) {
            Hand obj = Instantiate(prefab).GetComponent<Hand>();
            obj.Init(board, dump, lanes, time, missionSheet, env, isGolden);
            obj.transform.localPosition = pos;
            obj.transform.localScale = Vector3.one;
            return obj;
        }
    }
}
