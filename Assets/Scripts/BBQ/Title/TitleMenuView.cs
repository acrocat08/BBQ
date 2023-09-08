using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBQ.Title {
    [CreateAssetMenu(menuName = "TitleMenu/View")]
    public class TitleMenuView : ScriptableObject {

        [SerializeField] private float logoFloatLength;
        [SerializeField] private float logoFloatSpeed;
        public async void FloatLogo(TitleMenu titleMenu) {
            Transform logo = titleMenu.transform.Find("Logo");
            Vector3 centerPos = logo.localPosition;
            while (logo != null) {
                logo.localPosition = centerPos + logoFloatLength * Vector3.up * Mathf.Sin(logoFloatSpeed * Time.time);
                await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
            }
        }
    }
}
