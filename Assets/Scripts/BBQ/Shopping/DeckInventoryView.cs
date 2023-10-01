using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Shopping {
    
    [CreateAssetMenu(menuName = "DeckInventory/View")]
    public class DeckInventoryView : ScriptableObject {
        public void SetItem(DeckInventory deckInventory, string itemName) {
            deckInventory.transform.Find("UsedTools").Find(itemName).GetComponent<Image>().color = Color.white;
        }
    }
}
