using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Shopping {
    public class ArrowDrawer : Graphic {
        public Vector2 _topPoint;
        public Vector2 _bottomPoint;
        [SerializeField] float triangleWidth;
        [SerializeField] float triangleHeight;
        [SerializeField] private float weight;
        [SerializeField] private Transform BasePosX;
        [SerializeField] private Transform BasePosY;

        protected override void OnPopulateMesh(VertexHelper vh) {
            vh.Clear();

            Vector2 dir = (_topPoint - _bottomPoint).normalized;
            Vector2 normal = Quaternion.Euler(0, 0, 90) * (_topPoint - _bottomPoint).normalized;
            
            float h = Mathf.Min(triangleHeight, (_topPoint - _bottomPoint).magnitude);
            AddVert(vh, _topPoint - dir * h + normal * weight);
            AddVert(vh, _topPoint - dir * h - normal * weight);
            AddVert(vh, _bottomPoint);
            AddVert(vh, _topPoint);
            AddVert(vh, _topPoint - dir * h + normal * triangleWidth);
            AddVert(vh, _topPoint - dir * h - normal * triangleWidth);

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(3, 4, 5);
        }
        private void AddVert(VertexHelper vh, Vector2 pos)
        {
            var vert = UIVertex.simpleVert;
            
            vert.position = pos;
           // vert.position = pos + new Vector2(BasePosX.position.x, BasePosY.position.y);
            vert.color = color;
            vh.AddVert(vert);
        }

        public void SetPos(Vector2 from, Vector2 to, Color color) {
            float ratio = Mathf.Max((1920f / Screen.width), (1080f / Screen.height));
            _topPoint = to * ratio;
            _bottomPoint = from * ratio;
            this.color = color;
            SetVerticesDirty();
        }
        
    }
}