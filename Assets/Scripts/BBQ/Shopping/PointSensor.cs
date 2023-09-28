using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Shopping {
    public class PointSensor : MonoBehaviour {
        private List<PointableArea> _areas;

        private bool _isDragging;
        private PointableArea _fromArea;
        private List<PointableArea> _toAreas;

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
                    _toAreas = null;
                    _fromArea = _areas.Where(x => x.canPointDown).FirstOrDefault(x => x.CheckPointed(pointed));
                    if (_fromArea != null) {
                        _fromArea.Show();
                        _fromArea.onPointDown.Invoke();
                        _isDragging = true;
                    }
                } 
            }
            else {
                Color color = _fromArea.transform.Find("Frame").GetComponent<Image>().color;
                if(_fromArea.canDrag) arrow.SetPos(_fromArea.transform.position, pointed, color);
                PointableArea toArea = _areas
                    .Where(x => x != _fromArea)
                    .Where(x => x.areaTag == _fromArea.targetTag)
                    .FirstOrDefault(x => x.CheckPointed(pointed));

                List<PointableArea> toAreas = null;
                if (toArea != null && toArea.isGrouped) {
                    toAreas = _areas
                        .Where(x => x != _fromArea)
                        .Where(x => x.areaTag == toArea.areaTag).ToList();
                }
                else if (toArea != null) toAreas = new List<PointableArea> { toArea };
                
                if (_toAreas == null || !_toAreas.Contains(toArea)) {
                    if(toAreas != null) toAreas.ForEach(x => x.Show());
                    if(_toAreas != null) _toAreas.ForEach(x => x.Hide());
                    _toAreas = toAreas;
                }

                if (Input.GetMouseButtonUp(0)) {
                    _fromArea.Hide();
                    arrow.SetPos(Vector2.zero, Vector2.zero, Color.white);
                    if (_toAreas != null) {
                        _toAreas.ForEach(x => x.Hide());
                        _fromArea.onPointUp.Invoke(_toAreas);
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
