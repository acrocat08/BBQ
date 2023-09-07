using System.Collections.Generic;
using BBQ.Action;
using UnityEngine;

namespace BBQ.Cooking {
    [CreateAssetMenu(menuName = "Hand/Factory")]

    public class HandFactory : ScriptableObject {
        [SerializeField] private GameObject prefab;


        public Hand Create(Board board, Dump dump, List<Lane> lanes, CookTime time) {
            Hand obj = Instantiate(prefab).GetComponent<Hand>();
            obj.Init(board, dump, lanes, time);
            return obj;
        }
    }
}
