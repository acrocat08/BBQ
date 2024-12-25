using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SoundMgr;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Common {
    public class ExpGage : MonoBehaviour {
        [SerializeField] private RectTransform bar;
        [SerializeField] private RectTransform shadow;
        [SerializeField] private Text levelText;
        [SerializeField] private Image levelColor;
        [SerializeField] private Color levelUpColor;
        [SerializeField] private Text pointText;

        private int _nowLevel;
        private int _nowPoint;
        private const int Maxlevel = 150;

        public void Init(int level, int point) {
            float barWidth = CalcBarWidth(level, point);
            bar.sizeDelta = new(barWidth, bar.sizeDelta.y);
            _nowLevel = level;
            _nowPoint = point;
            UpdatePointText(point, GetMaxPoint(level));
            levelText.text = _nowLevel.ToString();
        }

        public async UniTask GainExp(int point) {
            int rest = point;
            float speed = (float)200f / rest;
            while (true) {
                int maxPoint = GetMaxPoint(_nowLevel);
                Debug.Log(maxPoint);
                Debug.Log(rest);
                Debug.Log(_nowPoint);
                if (_nowPoint + rest >= maxPoint) {
                    if (_nowLevel >= Maxlevel) break;
                    ScrollPointText(_nowPoint, maxPoint, maxPoint);
                    rest -= maxPoint - _nowPoint;
                    _nowPoint = maxPoint;
                    await ProgressGage();
                    _nowLevel++;
                    LevelUp();
                    _nowPoint = 0;
                    bar.sizeDelta = new(4, bar.sizeDelta.y);
                }
                else {
                    if (_nowLevel >= Maxlevel) break;
                    ScrollPointText(_nowPoint, _nowPoint + rest, maxPoint);
                    _nowPoint += rest;
                    await ProgressGage();
                    break;
                } 
            }
            UpdatePointText(_nowPoint, GetMaxPoint(_nowLevel));
        }

        private async UniTask ProgressGage() {
            float barWidth = CalcBarWidth(_nowLevel, _nowPoint);
            bar.DOSizeDelta(new(barWidth, bar.sizeDelta.y), 0.5f).SetEase(Ease.Linear);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            await UniTask.Yield();
        }

        private float CalcBarWidth(int level, int point) {
            float rate = (float)point / GetMaxPoint(level);
            if (shadow.sizeDelta.x * rate < 4) return 4;
            return shadow.sizeDelta.x * rate;
        }

        private int GetMaxPoint(int level) {
            return 50 + (level / 5) * 10;
        }

        private void LevelUp() {
            SoundPlayer.I.Play("se_gainStar");
            levelText.text = _nowLevel.ToString();
            Color nowColor = levelColor.color;
            levelColor.color = levelUpColor;
            levelColor.DOColor(nowColor, 0.3f).SetEase(Ease.InCubic);
        }

        public int GetLevel() {
            return _nowLevel;
        }
        
        public int GetPoint() {
            return _nowPoint;
        }

        void UpdatePointText(int point, int maxPoint) {
            pointText.text = point + " / " + maxPoint;
        }

        private async void ScrollPointText(int fromPoint, int toPoint, int maxPoint) {
            float interval = 0.5f / (toPoint - fromPoint);
            for (int i = 0; i < toPoint - fromPoint; i++) {
                UpdatePointText(fromPoint + i + 1, maxPoint);
                await UniTask.Delay(TimeSpan.FromSeconds(interval));
            }
        }

    }
}
