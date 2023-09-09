using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BBQ.Shopping {
    public class PointSensor : MonoBehaviour {
        private List<PointableArea> _areas;

        private bool _isDragging;
        private PointableArea _fromArea;
        private PointableArea _toArea;

        [SerializeField] private ArrowDrawer arrow;

        private void Start() {
            UpdateArea();
        }

        public void UpdateArea() {
            _isDragging = false;
            GameObject[] gameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
            _areas = new List<PointableArea>(gameObjects.Select(x => x.GetComponent<PointableArea>())
                .Where(x => x != null));
        }

        void Update() {
            Vector2 pointed = Input.mousePosition;
            if (!_isDragging) {
                if (Input.GetMouseButtonDown(0)) {
                    _toArea = null;
                    _fromArea = _areas.Where(x => x.canPointDown).FirstOrDefault(x => x.CheckPointed(pointed));
                    if (_fromArea != null) {
                        _fromArea.Show();
                        _fromArea.onPointDown.Invoke();
                        _isDragging = true;
                    }
                } 
            }
            else {
                arrow.SetPos(_fromArea.transform.position, pointed);
                PointableArea toArea = _areas
                    .Where(x => x != _fromArea)
                    .Where(x => x.areaTag == _fromArea.targetTag)
                    .FirstOrDefault(x => x.CheckPointed(pointed));
                if (toArea != _toArea) {
                    if(toArea != null) toArea.Show();
                    if(_toArea != null) _toArea.Hide();
                    _toArea = toArea;
                }

                if (Input.GetMouseButtonUp(0)) {
                    _fromArea.Hide();
                    arrow.SetPos(Vector2.zero, Vector2.zero);
                    if (_toArea != null) {
                        _toArea.Hide();
                        _fromArea.onPointUp.Invoke(_toArea);
                    }
                    else {
                        _fromArea.onPointCancel.Invoke();
                    }
                    _isDragging = false;
                }
            }
            
            
            
        }

    }
}
