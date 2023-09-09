using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "DeckItem/View")]
    public class DeckItemView : ScriptableObject {
        public void SetFood(DeckItem deckItem) {
            Image foodImage = deckItem.transform.Find("Food").GetComponent<Image>();
            if (deckItem != null) foodImage.sprite = deckItem.GetFood().data.foodImage;
            foodImage.enabled = deckItem != null;
        }
    }
}
