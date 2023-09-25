using BBQ.Database;
using BBQ.PlayData;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Shopping {
    
    [CreateAssetMenu(menuName = "DetailView")]
    public class DetailView : ScriptableObject {

        public void DrawDetail(Transform container, FoodData foodData) {
            DrawFoodInfo(container, foodData, 1);
        }
        
        public void DrawDetail(Transform container, DeckFood deckFood) {
            DrawFoodInfo(container, deckFood.data, deckFood.lank);
        }
        
        public void DrawDetail(Transform container, ToolData toolData) {
            DrawToolInfo(container, toolData);
        }

        public void Clear(Transform container) {
            Transform baseInfo = container.Find("BaseInfo");
            baseInfo.gameObject.SetActive(false);
        }
        void DrawFoodInfo(Transform container, FoodData foodData, int lank) {
            Transform baseInfo = container.Find("BaseInfo");
            baseInfo.gameObject.SetActive(true);
            baseInfo.Find("Food").GetComponent<Image>().sprite = foodData.foodImage;
            baseInfo.Find("Cost").GetComponent<Text>().text = foodData.cost.ToString();
            baseInfo.Find("Name").GetComponent<Text>().text = foodData.foodName;
            baseInfo.Find("Detail").GetComponent<Text>().text = foodData.action.summaries[lank - 1];
        }
        
        void DrawToolInfo(Transform container, ToolData toolData) {
            Transform baseInfo = container.Find("BaseInfo");
            baseInfo.gameObject.SetActive(true);
            baseInfo.Find("Food").GetComponent<Image>().sprite = toolData.toolImage;
            baseInfo.Find("Cost").GetComponent<Text>().text = toolData.cost.ToString();
            baseInfo.Find("Name").GetComponent<Text>().text = toolData.toolName;
            baseInfo.Find("Detail").GetComponent<Text>().text = toolData.action.summaries[0];
        }
        

    }
}
