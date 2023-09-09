using BBQ.Database;
using BBQ.PlayData;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Shopping {
    
    [CreateAssetMenu(menuName = "DetailView")]
    public class DetailView : ScriptableObject {

        public void DrawDetail(Transform container, FoodData foodData) {
            DrawBaseInfo(container, foodData, 0);
        }

        public void Clear(Transform container) {
            Transform baseInfo = container.Find("BaseInfo");
            baseInfo.gameObject.SetActive(false);
        }
        void DrawBaseInfo(Transform container, FoodData foodData, int lank) {
            Transform baseInfo = container.Find("BaseInfo");
            baseInfo.gameObject.SetActive(true);
            baseInfo.Find("Food").GetComponent<Image>().sprite = foodData.foodImage;
            baseInfo.Find("Cost").GetComponent<Text>().text = foodData.cost.ToString();
            baseInfo.Find("Name").GetComponent<Text>().text = foodData.foodName;
            baseInfo.Find("Detail").GetComponent<Text>().text = foodData.action.summaries[lank];
        }
        

    }
}
