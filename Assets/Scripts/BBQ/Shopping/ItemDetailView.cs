using System.Collections.Generic;
using BBQ.Common;
using BBQ.Database;
using BBQ.PlayData;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Shopping {
    
    [CreateAssetMenu(menuName = "DetailView")]
    public class ItemDetailView : ScriptableObject {

        [SerializeField] private ViewParam param;
        [SerializeField] private List<Material> materials;
        
        public void DrawFoodInfo(Transform container, FoodData foodData, int lank) {
            Transform baseInfo = container.Find("BaseInfo");
            baseInfo.gameObject.SetActive(true);
            baseInfo.Find("Food").GetComponent<Image>().sprite = foodData.foodImage;
            baseInfo.Find("Food").GetComponent<Image>().material = materials[lank - 1];
            baseInfo.Find("Lank").GetComponent<Text>().text = foodData.tier > 0 ? "ティア" + foodData.tier + " 食材" : "トークン食材";
            baseInfo.Find("Line").GetComponent<Image>().color = param.tierColors[foodData.tier];
            baseInfo.Find("Name").GetComponent<Text>().text = foodData.foodName;
            baseInfo.Find("Detail").GetComponent<DetailText>().SetDetail(foodData.action.summaries[lank - 1]);
        }
        
        public void DrawToolInfo(Transform container, ToolData toolData) {
            Transform baseInfo = container.Find("BaseInfo");
            baseInfo.gameObject.SetActive(true);
            baseInfo.Find("Food").GetComponent<Image>().sprite = toolData.toolImage;
            baseInfo.Find("Food").GetComponent<Image>().material = materials[0];
            baseInfo.Find("Lank").GetComponent<Text>().text = "道具";
            baseInfo.Find("Line").GetComponent<Image>().color = param.toolColor;
            baseInfo.Find("Name").GetComponent<Text>().text = toolData.toolName;
            baseInfo.Find("Detail").GetComponent<DetailText>().SetDetail(toolData.action.summaries[0]);
        }
        
        public void DrawEffectInfo(Transform container, FoodEffect effectData) {
            Transform baseInfo = container.Find("BaseInfo");
            baseInfo.gameObject.SetActive(true);
            baseInfo.Find("Food").GetComponent<Image>().sprite = effectData.effectImage;
            baseInfo.Find("Food").GetComponent<Image>().material = materials[0];
            baseInfo.Find("Lank").GetComponent<Text>().text = "エフェクト";
            baseInfo.Find("Line").GetComponent<Image>().color = param.effectColor;
            baseInfo.Find("Name").GetComponent<Text>().text = effectData.effectName;
            baseInfo.Find("Detail").GetComponent<DetailText>().SetDetail(effectData.action.summaries[0]);
        }
        

    }
}
