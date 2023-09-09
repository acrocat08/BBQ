using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BBQ.Shopping {
    public class PointableArea : MonoBehaviour {
        private RectTransform _tr;
        private Image _image;

        public UnityEvent onPointDown;
        public UnityEvent<PointableArea> onPointUp;
        public UnityEvent onPointCancel;
        public string areaTag;
        public string targetTag;

        public bool canPointDown;        

        private void Start() {
            Hide();
        }

        public bool CheckPointed(Vector2 pointedPos) {
            if(_tr == null) _tr = GetComponent<RectTransform>();
            Vector3[] corners = new Vector3[4];
            _tr.GetWorldCorners(corners);
            return pointedPos.x >= corners[0].x && pointedPos.x <= corners[2].x
                && pointedPos.y >= corners[0].y && pointedPos.y <= corners[2].y;
        }

        public void Hide() {
            if(_image == null) _image = transform.Find("Frame").GetComponent<Image>();
            _image.enabled = false;
        }
        public void Show() {
            if(_image == null) _image = transform.Find("Frame").GetComponent<Image>();
            _image.enabled = true;
        }
    }
}
