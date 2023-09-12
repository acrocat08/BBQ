using System.Collections.Generic;
using System.Linq;
using BBQ.PlayData;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace BBQ.Shopping {
    public class DeckItem : MonoBehaviour {
        [SerializeField] private DeckItemView view;
        [SerializeField] private DeckInventory inventory;
        private DeckFood _deckFood;

        [SerializeField] private Transform detail;
        [SerializeField] private DetailView detailView;
        [SerializeField] private Merger merger;
        [SerializeField] private int index;

        public void SetFood(DeckFood deckFood) {
            _deckFood = deckFood;
            view.SetFood(this);
        }

        public DeckFood GetFood() {
            return _deckFood;
        }

        public void OnPointDown() {
            if(_deckFood != null) detailView.DrawDetail(detail, _deckFood);
        }

        public async void OnPointUp(List<PointableArea> areas) {
            List<DeckItem> target = areas.Select(x => x.transform.parent.GetComponent<DeckItem>()).ToList();
            target.Add(this);
            await merger.Merge(target);
            inventory.SortItem();
        }

        public int GetIndex() {
            return index;
        }

        public void LankUp() {
            _deckFood.lank += 1;
            view.SetFood(this);
        }
    }
}
