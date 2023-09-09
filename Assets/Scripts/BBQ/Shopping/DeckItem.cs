using BBQ.PlayData;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace BBQ.Shopping {
    public class DeckItem : MonoBehaviour {
        [SerializeField] private DeckItemView view;
        private DeckFood _deckFood;

        [SerializeField] private Transform detail;
        [SerializeField] private DetailView detailView;


        public void SetFood(DeckFood deckFood) {
            _deckFood = deckFood;
            view.SetFood(this);
        }

        public DeckFood GetFood() {
            return _deckFood;
        }

        public void OnPointDown() {
            if(_deckFood != null) detailView.DrawDetail(detail, _deckFood.data);
        }

        
    }
}
