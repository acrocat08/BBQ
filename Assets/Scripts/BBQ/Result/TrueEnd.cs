using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Result {
    [CreateAssetMenu(menuName = "Result/TrueEnd")]
    public class TrueEnd : ScriptableObject {

        [SerializeField] private Sprite bgImage;
        [SerializeField] private Color titleColor;

        public void Init(Transform container) {
            SoundPlayer.I.Play("bgm_trueEnd");
            container.Find("BG").GetComponent<Image>().sprite = bgImage;
            container.Find("Title").GetComponent<Text>().text = "Clear!!";
            container.Find("Title").GetComponent<Text>().color = titleColor;
        }

        public async UniTask GotoTitle() {
            await SoundPlayer.I.FadeOutSound("bgm_trueEnd");
        }
        
    }
}