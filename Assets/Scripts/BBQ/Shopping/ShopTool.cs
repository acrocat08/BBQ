using System.Collections.Generic;
using BBQ.Database;
using UnityEngine;

namespace BBQ.Shopping {
    public class ShopTool : MonoBehaviour {
        [SerializeField] private ShopItemView shopView;
        [SerializeField] DetailView detailView;

        public ToolData data;
        private Transform _detailContainer;
        private Shop _shop;

        public void Init(ToolData data, Shop shop, string areaTag, Transform detail) {
            this.data = data;
            _shop = shop;
            PointableArea area = transform.Find("Image").Find("Pointable").GetComponent<PointableArea>();
            area.areaTag = areaTag;
            area.targetTag = data.targetArea;
            _detailContainer = detail;
            area.onPointDown.AddListener(OnPointDown);
            area.onPointCancel.AddListener(OnPointCancel);
            area.onPointUp.AddListener(OnPointUp);
            shopView.DrawTool(this);
        }

        public void OnPointDown() {
            detailView.DrawDetail(_detailContainer, data);
        }

        public void OnPointCancel() {
            //detailView.Clear(_detailContainer);
        }
        
        public void OnPointUp(List<PointableArea> areas) {
            
            _shop.UseTool(this);
        }

        public void Drop() {
            shopView.Drop(this);
        }

        public void Fall() {
            shopView.Fall(transform.Find("Image").transform);
        }
    }
}
