using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BBQ.Common {
    public class BackgroundView : MonoBehaviour
    {

        [SerializeField, Range(-1, 1)]
        float scrollSpeed = 1.0f;

        [SerializeField] Image cloudImage;
        Material _cloudMaterial;
        [SerializeField] private Image seaImage;
        [SerializeField] private float waveLength;
        [SerializeField] private float waveSpeed;

        void Start()
        {
            // 他に影響を与えないためにマテリアルを複製
            _cloudMaterial = new(cloudImage.material);
            cloudImage.material = _cloudMaterial;
            Wave(seaImage.transform);
        }

        void Update()
        {
            if (cloudImage == null || _cloudMaterial == null)
                return;

            // 現在のオフセットを取得
            Vector2 offset = cloudImage.material.mainTextureOffset;

            // X軸のスクロール
            if (scrollSpeed != 0.0f)
            {
                offset.x += scrollSpeed * Time.deltaTime;
            }

            // 更新したオフセットをマテリアルに適用
            cloudImage.material.mainTextureOffset = offset;
        }

        private void OnDestroy()
        {
            // マテリアルの消去
            Destroy(_cloudMaterial);
        }
        
        async void Wave(Transform wave) {
            Vector3 centerPos = wave.localPosition;
            while (wave != null) {
                wave.localPosition = centerPos + waveLength * Vector3.left * (Mathf.Sin(waveSpeed * Time.time) + 1);
                await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
            }
        }

    }
}
