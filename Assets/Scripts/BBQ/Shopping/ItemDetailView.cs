using System.Collections.Generic;
using BBQ.Database;
using BBQ.PlayData;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Shopping {
    
    [CreateAssetMenu(menuName = "DetailView")]
    public class ItemDetailView : ScriptableObject {

        [SerializeField] private ViewParam param;
        
        public void DrawFoodInfo(Transform container, FoodData foodData, int lank) {
            Transform baseInfo = container.Find("BaseInfo");
            baseInfo.gameObject.SetActive(true);
            baseInfo.Find("Food").GetComponent<Image>().sprite = foodData.foodImage;
            baseInfo.Find("Lank").GetComponent<Text>().text = foodData.tier > 0 ? "ランク" + lank + " 食材" : "トークン食材";
            baseInfo.Find("Line").GetComponent<Image>().color = param.tierColors[foodData.tier];
            baseInfo.Find("Name").GetComponent<Text>().text = foodData.foodName;
            baseInfo.Find("Detail").GetComponent<Text>().text = foodData.action.summaries[lank - 1];
        }
        
        public void DrawToolInfo(Transform container, ToolData toolData) {
            Transform baseInfo = container.Find("BaseInfo");
            baseInfo.gameObject.SetActive(true);
            baseInfo.Find("Food").GetComponent<Image>().sprite = toolData.toolImage;
            baseInfo.Find("Lank").GetComponent<Text>().text = "道具";
            baseInfo.Find("Line").GetComponent<Image>().color = param.toolColor;
            baseInfo.Find("Name").GetComponent<Text>().text = toolData.toolName;
            baseInfo.Find("Detail").GetComponent<Text>().text = toolData.action.summaries[0];
        }
        
        public void DrawEffectInfo(Transform container, FoodEffect effectData) {
            Transform baseInfo = container.Find("BaseInfo");
            baseInfo.gameObject.SetActive(true);
            baseInfo.Find("Food").GetComponent<Image>().sprite = effectData.effectImage;
            baseInfo.Find("Lank").GetComponent<Text>().text = "エフェクト";
            baseInfo.Find("Line").GetComponent<Image>().color = param.effectColor;
            baseInfo.Find("Name").GetComponent<Text>().text = effectData.effectName;
            baseInfo.Find("Detail").GetComponent<Text>().text = effectData.action.summaries[0];
        }
        

    }
}
