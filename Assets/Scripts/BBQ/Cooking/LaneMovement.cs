using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using BBQ.Database;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

namespace BBQ.Cooking {
    public class LaneMovement : MonoBehaviour {
        [SerializeField] private CookingParam param;
        [SerializeField] private Lane lane;
        [SerializeField] private int speedPerSecond;
        [SerializeField] private int rightBorder;
        [SerializeField] private float addFoodDuration;  
        [SerializeField] private Ease addFoodEasing;
        private float _startXPos;
        private bool _pauseMode;
        

        private void Start() {
            _pauseMode = false;
        }

        public async UniTask AddFood(LaneFood food, int index) {
            Transform tr = food.transform;
            tr.SetParent(GameObject.Find("Canvas").transform);
            Vector3 prevPos = tr.localPosition;
            
            tr.SetParent(transform);
            Vector3 targetPos = GetFoodPos(food, index);
            tr.localPosition = targetPos;
            tr.SetParent(GameObject.Find("Canvas").transform);
            targetPos = tr.localPosition;
            
            tr.localPosition = prevPos;
            await tr.DOLocalMove(targetPos, addFoodDuration).SetEase(addFoodEasing);
            tr.SetParent(transform);
        }
        
        public void Move() {
                if (_pauseMode) return;
                _startXPos = GetLoopedPos(_startXPos + speedPerSecond * 0.01f);
                List<LaneFood> foods = lane.GetFoods();
                for (int i = 0; i < foods.Count; i++) {
                    if (foods[i] == null) continue;
                    foods[i].transform.localPosition = GetFoodPos(foods[i], i);
                }
        }


        private float GetLoopedPos(float pos) {
            while (pos > rightBorder) pos -= rightBorder;
            while (pos < 0) pos += rightBorder;
            return pos;
        }

        private Vector3 GetFoodPos(LaneFood food, int index) {
            float pos = _startXPos;
            float offset = (param.foodSize + param.foodMargin) * index;
            if (speedPerSecond > 0) pos -= offset;
            else pos += offset;
            pos = GetLoopedPos(pos);
            return new Vector3(pos, 0, 0);
        }

        public void SetPauseMode(bool mode) {
            _pauseMode = mode;
        }
        

    }
}