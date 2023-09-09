using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Shopping {
    public class ArrowDrawer : Graphic {
        public Vector2 _topPoint;
        public Vector2 _bottomPoint;
        [SerializeField] float triangleWidth;
        [SerializeField] float triangleHeight;
        [SerializeField] private float weight;

        protected override void OnPopulateMesh(VertexHelper vh) {
            vh.Clear();

            Vector2 dir = (_topPoint - _bottomPoint).normalized;
            Vector2 normal = Quaternion.Euler(0, 0, 90) * (_topPoint - _bottomPoint).normalized;
            
            float h = Mathf.Min(triangleHeight, (_topPoint - _bottomPoint).magnitude);
            AddVert(vh, _topPoint - dir * h + normal * weight);
            AddVert(vh, _topPoint - dir * h - normal * weight);
            AddVert(vh, _bottomPoint + normal * weight);
            AddVert(vh, _bottomPoint - normal * weight);
            AddVert(vh, _topPoint);
            AddVert(vh, _topPoint - dir * h + normal * triangleWidth);
            AddVert(vh, _topPoint - dir * h - normal * triangleWidth);

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(1, 2, 3);
            vh.AddTriangle(4, 5, 6);
        }
        private void AddVert(VertexHelper vh, Vector2 pos)
        {
            var vert = UIVertex.simpleVert;
            vert.position = pos;
            vert.color = color;
            vh.AddVert(vert);
        }

        public void SetPos(Vector2 from, Vector2 to) {
            _topPoint = to;
            _bottomPoint = from;
            SetVerticesDirty();
        }
        
    }
}