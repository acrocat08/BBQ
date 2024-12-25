using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BBQ.Common;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SoundMgr;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BBQ.Cooking {
    public class HandShot : MonoBehaviour {
        [SerializeField] private float maxPos;
        [SerializeField] private float moveDuration;
        [SerializeField] private float firstDuration;
        [SerializeField] private float hitDuration;
        [SerializeField] private float waitDuration;
        private List<Lane> _lanes;

        public void Init(List<Lane> lanes) {
            _lanes = lanes;
        }

        public async UniTask<List<FoodObject>> Shot(bool isDouble) {
            List<FoodObject> hitFoods = _lanes.Select(x => x.SearchNearestFood(transform.position.x)).ToList();

            RectTransform tr = GetComponent<RectTransform>();
            Vector2 toSize = tr.sizeDelta;

                tr.DOLocalMoveY(maxPos, moveDuration).SetEase(Ease.Linear);

                
                
            await UniTask.Delay(TimeSpan.FromSeconds(firstDuration));
            FoodObject bottom = hitFoods[2];
            if (bottom != null) {
                bottom.Hit();
                SoundPlayer.I.Play("se_hit1");
                SoundPlayer.I.Play("se_hit2-1");
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(hitDuration));
            FoodObject middle = hitFoods[1];
            if (middle != null) {
                middle.Hit();
                SoundPlayer.I.Play("se_hit1");
                SoundPlayer.I.Play("se_hit2-2");
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(hitDuration));
            FoodObject top = hitFoods[0];
            if (top != null) {
                top.Hit();
                SoundPlayer.I.Play("se_hit1");
                SoundPlayer.I.Play("se_hit2-3");
            }
            
            if (hitFoods.Count(x => x != null) == 0) {
                SoundPlayer.I.Play("se_nohit");
                return new();
            }

            await UniTask.Delay(TimeSpan.FromSeconds(waitDuration));
            return hitFoods.ToList();
        }
        
    }
}
