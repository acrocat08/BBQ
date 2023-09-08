using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BBQ.Cooking {
    
    [CreateAssetMenu(menuName = "ParamUpEffectFactory")]
    public class ParamUpEffectFactory : ScriptableObject {

        [SerializeField] private List<ParamImageSet> images;
        [SerializeField] private GameObject effectPrefab;
        [SerializeField] private float offset;
        [SerializeField] private Vector3 defaultPos;
        [SerializeField] private float posXMin;
        [SerializeField] private float posXMax;
        [SerializeField] private float posYMin;
        [SerializeField] private float posYMax;
        [SerializeField] private float moveDuration;
        [SerializeField] private float moveLength;
        [SerializeField] private Ease moveEasing;
        [SerializeField] Ease alphaEasing;
        [SerializeField] private Color minusColor;
        private Dictionary<string, Sprite> _dict;
        
        public async void Create(string imageName, int val, LaneFood laneFood) {
            if (_dict == null) InitDict();
            Vector3 pos = laneFood == null ? defaultPos : laneFood.transform.position;
            pos += offset * (Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector3.up);
            pos.x = Mathf.Clamp(pos.x, posXMin, posXMax);
            pos.y = Mathf.Clamp(pos.y, posYMin, posYMax);
            Transform obj = Instantiate(effectPrefab, GameObject.Find("Canvas").transform).transform;
            obj.position = pos;
            obj.Find("Text").GetComponent<Text>().text = (val >= 0 ? "+" : "-") + Mathf.Abs(val);
            if (val < 0) obj.Find("Text").GetComponent<Text>().color = minusColor;
            obj.Find("Image").GetComponent<Image>().sprite = _dict[imageName];
            obj.DOMoveY(obj.transform.position.y + moveLength, moveDuration).SetEase(moveEasing);
            CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
            DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0f, moveDuration).SetEase(alphaEasing);
            await UniTask.Delay(TimeSpan.FromSeconds(moveDuration));
            Destroy(obj.gameObject);
        }

        void InitDict() {
            _dict = new Dictionary<string, Sprite>();
            foreach (ParamImageSet set in images) {
                _dict[set.name] = set.sprite;
            }
        }
        
    }

    [Serializable]
    public class ParamImageSet {
        public string name;
        public Sprite sprite;
    }
}
