using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Cooking {
    public class HandMovement : MonoBehaviour {
        [SerializeField] private float rightBorder;
        [SerializeField] private float leftBorder;
        
        public void MoveDelta() {
            float mousePos = Input.mousePosition.x;
            mousePos = Mathf.Clamp(mousePos, leftBorder, rightBorder);
            var tr = transform;
            Vector3 newPos = tr.position;
            newPos.x = mousePos;
            tr.position = newPos;
        }
        
        public bool CheckIsInnerBorder() {
            float mousePos = Input.mousePosition.x;
            return mousePos >= leftBorder && mousePos <= rightBorder;
        }

    }
}