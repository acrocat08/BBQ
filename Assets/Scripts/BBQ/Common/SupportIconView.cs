using BBQ.Database;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Common {
    [CreateAssetMenu(menuName = "SupportIcon/View")]
    public class SupportIconView : ScriptableObject {

        public void Draw(Transform icon, SupportIcon iconData) {
            icon.Find("Back").GetComponent<Image>().color = iconData.backColor;
            icon.Find("Icon").GetComponent<Image>().color = Color.white;
            icon.Find("Icon").GetComponent<Image>().sprite = iconData.iconImage;
        }

        public void Hide(Transform icon) {
            icon.Find("Back").GetComponent<Image>().color = Color.clear;
            icon.Find("Icon").GetComponent<Image>().color = Color.clear;
        }

    }
}
