using Cysharp.Threading.Tasks;
using DG.Tweening;
using SoundMgr;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Result {
    [CreateAssetMenu(menuName = "Result/TrueEnd")]
    public class TrueEnd : ScriptableObject {

        [SerializeField] private Sprite bgImage;
        [SerializeField] private Color titleColor;
        [SerializeField] private Sprite balloonImage;

        public void Init(Transform container) {
            SoundPlayer.I.Play("bgm_trueEnd");
            container.Find("BG").GetComponent<Image>().sprite = bgImage;
            container.Find("Title").GetComponent<Text>().text = "Clear!!";
            container.Find("Title").GetComponent<Text>().color = titleColor;
            container.Find("Balloon").GetComponent<Image>().enabled = true;
            container.Find("Balloon").GetComponent<Image>().sprite = balloonImage;
            container.Find("Balloon").DOLocalMoveY(50, 5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine)
                .SetLink(container.Find("Balloon").gameObject);
            container.Find("Balloon").DOLocalMoveX(600, 60f).SetEase(Ease.Linear)
                .SetLink(container.Find("Balloon").gameObject);
        }

        public async UniTask GotoTitle() {
            await SoundPlayer.I.FadeOutSound("bgm_trueEnd");
        }
        
    }
}