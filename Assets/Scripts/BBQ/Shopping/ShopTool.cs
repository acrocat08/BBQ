using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace BBQ.Shopping {
    public class ShopTool : MonoBehaviour {
        [SerializeField] private ShopItemView shopView;
        private ItemDetail itemDetail;

        public ToolData data;
        private Shop _shop;

        public void Init(ToolData data, Shop shop, string areaTag, ItemDetail detail) {
            this.data = data;
            _shop = shop;
            itemDetail = detail;
            PointableArea area = transform.Find("Image").Find("Pointable").GetComponent<PointableArea>();
            area.areaTag = areaTag;
            area.targetTag = data.targetArea;
            area.onPointDown.AddListener(OnPointDown);
            area.onPointCancel.AddListener(OnPointCancel);
            area.onPointUp.AddListener(OnPointUp);
            shopView.DrawTool(this);
        }

        public void OnPointDown() {
            itemDetail.DrawDetail(data);
        }

        public void OnPointCancel() {
            //detailView.Clear(_detailContainer);
        }

        public void OnPointUp(List<PointableArea> areas) {
            if (InputGuard.Guard()) return;
            List<DeckFood> target = new List<DeckFood>();
            if (data.targetArea == "deckItem") {
                target = areas.Select(x => x.transform.parent.GetComponent<InventoryFood>().deckFood).ToList();
            }
            _shop.UseTool(this, target);
        }

        public void Drop() {
            shopView.Drop(this);
        }

        public void Fall() {
            shopView.Fall(transform.Find("Image").transform);
        }
    }
}
