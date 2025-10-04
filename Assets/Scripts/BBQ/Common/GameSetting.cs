using System;
using System.Linq;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SoundMgr;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BBQ.Common {
    public class GameSetting : MonoBehaviour {
        
        private bool isMoving;
        [SerializeField] private EventTrigger backButton;
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider seSlider;


        public async void Open() {
            SoundPlayer.I.Play("se_select1");
            bgmSlider.value = PlayerConfig.GetBgmVolume();
            seSlider.value = PlayerConfig.GetSeVolume();
            isMoving = true;
            transform.localScale = Vector3.one;
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            DOTween.To(
                () => canvasGroup.alpha,
                x => canvasGroup.alpha = x,
                1f,
                0f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            isMoving = false;
            backButton.enabled = true;

        }
        
        public async void Close() {
            isMoving = true;
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            DOTween.To(
                () => canvasGroup.alpha,
                x => canvasGroup.alpha = x,
                0f,
                0f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            transform.localScale = Vector3.zero;
            isMoving = false;
            backButton.enabled = false;
        }

        public void OnBgmValueChanged() {
            PlayerConfig.Create(PlayerConfig.GetShopPool(9), PlayerConfig.GetPoolIndex(), PlayerConfig.GetPoolIndex(), PlayerConfig.GetGameMode(),
                bgmSlider.value, PlayerConfig.GetSeVolume());
            SoundPlayer.I.AdjustVolume();
        }
        
        public void OnSeValueChanged() {
            PlayerConfig.Create(PlayerConfig.GetShopPool(9), PlayerConfig.GetPoolIndex(), PlayerConfig.GetPoolIndex(), PlayerConfig.GetGameMode(),
                PlayerConfig.GetBgmVolume(), seSlider.value);
        }
        
    }
}
