using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SoundMgr;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BBQ.Cooking {
    public class HandShot : MonoBehaviour {
        [SerializeField] private float maxHeight;
        [SerializeField] private float durationSecond;
        private List<Lane> _lanes;

        public void Init(List<Lane> lanes) {
            _lanes = lanes;
        }

        public async UniTask<List<LaneFood>> Shot() {
            PlaySound();
            List<LaneFood> hitFoods = new List<LaneFood>();
            RectTransform tr = GetComponent<RectTransform>();
            float delta = (maxHeight - tr.sizeDelta.y) / (durationSecond * 1000);
            int laneCount = 0;
            for (int i = 0; i < durationSecond * 1000; i++) {
                await UniTask.Delay(TimeSpan.FromSeconds(0.001f));
                Vector2 size = tr.sizeDelta;
                size.y += delta;
                tr.sizeDelta = size;

                if (laneCount >= _lanes.Count) continue;
                if (size.y >= _lanes[laneCount].transform.position.y) {
                    LaneFood hitFood = _lanes[laneCount].SearchNearestFood(transform.position.x);
                    if (hitFood != null) {
                        hitFood.Hit();
                        hitFoods.Add(hitFood);
                    }
                    laneCount++;
                }
            }
            hitFoods.Reverse();
            return hitFoods;
        }

        private async void PlaySound() {
            List<LaneFood> hitFood = _lanes.Select(x => x.SearchNearestFood(transform.position.x)).ToList();
            if (hitFood.Count(x => x != null) == 0) {
                SoundPlayer.I.Play("se_nohit");
                return;
            }
            if (hitFood[2] != null) {
                SoundPlayer.I.Play("se_hit1");
                SoundPlayer.I.Play("se_hit2-1");
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.02f));

            if (hitFood[1] != null) {
                SoundPlayer.I.Play("se_hit1");
                SoundPlayer.I.Play("se_hit2-2");
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.04f));
            if (hitFood[0] != null) {
                SoundPlayer.I.Play("se_hit1");
                SoundPlayer.I.Play("se_hit2-3");
            }
        }
        
        
    }
}
