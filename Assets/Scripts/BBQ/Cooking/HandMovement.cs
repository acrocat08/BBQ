using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Cooking {
    public class HandMovement : MonoBehaviour {
        [SerializeField] private float rightBorder;
        [SerializeField] private float leftBorder;
        
        public void MoveDelta() {
            float rate = Input.mousePosition.x / (float)Screen.width;
            rate = Mathf.Clamp(rate, leftBorder, rightBorder);
            var tr = transform;
            Vector3 newPos = tr.position;
            newPos.x = Screen.width * rate;
            tr.position = newPos;
        }
        
        public bool CheckIsInnerBorder() {
            float rate = Input.mousePosition.x / (float)Screen.width;
            return rate >= leftBorder && rate <= rightBorder;
        }

    }
}