using System.Collections.Generic;
using BBQ.PlayData;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "DeckItem/View")]
    public class DeckItemView : ScriptableObject {

        [SerializeField] private List<Color> lankColor;
        
        public void SetFood(DeckItem deckItem) {
            DeckFood deckFood = deckItem.GetFood();
            Image foodImage = deckItem.transform.Find("Food").GetComponent<Image>();
            foodImage.sprite = deckFood != null ? deckFood.data.foodImage : null;
            foodImage.enabled = deckFood != null;
            Image lankImage = deckItem.transform.Find("Lank").GetComponent<Image>();
            lankImage.color = deckFood != null ? lankColor[deckFood.lank - 1] : Color.clear;
        }
    }
}
