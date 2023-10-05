using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SoundMgr;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Tutorial.Action {
    [CreateAssetMenu(menuName = "Tutorial/ShowText")]
    public class ShowText : TutorialAction {
        [SerializeField] private float showSpeed;
        [SerializeField] private List<string> emotionKeys;
        [SerializeField] private List<Sprite> emotionImages;
        [SerializeField] private float delay;
        
        public override async UniTask Exec(Transform container, string text, string takoEmotion, float value) {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            Message(container).gameObject.SetActive(true);
            Tako(container).GetComponent<Image>().sprite = emotionImages[emotionKeys.IndexOf(takoEmotion)];
            await ShowUp(Message(container).Find("Text").GetComponent<Text>(), text);
        }

        async UniTask ShowUp(Text message, string text) {
            string str = text;
            for (int i = 0; i < str.Length; i++) {
                string nowStr = string.Concat(str.Take(i + 1));
                message.text = nowStr;
                if(i % 2 == 0) SoundPlayer.I.Play("se_message");
                await UniTask.Delay(TimeSpan.FromSeconds(showSpeed));
            }
        }
        
        

    }
}