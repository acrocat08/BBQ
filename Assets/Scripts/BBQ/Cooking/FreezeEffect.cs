using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BBQ.Cooking {
    public class FreezeEffect : MonoBehaviour {

        [SerializeField] private float duration;
        [SerializeField] private Ease easing;
        [SerializeField] private GameObject fractalPrefab;
        [SerializeField] private int fractalNum;
        [SerializeField] private float fractalDuration;
        [SerializeField] private float fractalLength;
        [SerializeField] private float fractalMinSize;
        [SerializeField] private float fractalMaxSize;

        public void Draw() {
            transform.localScale = Vector3.zero;
            transform.Rotate(0, 0, Random.Range(0, 360));
            transform.DOScale(Vector3.one, duration).SetEase(easing);
            for (int i = 0; i < fractalNum; i++) {
                Transform fractal = Instantiate(fractalPrefab, transform).transform;
                fractal.localPosition = Vector3.zero;
                fractal.transform.Rotate(0, 0, Random.Range(0, 360));
                fractal.transform.localScale = Vector3.one * (Random.Range(fractalMinSize, fractalMaxSize));
                int angle = Random.Range(i * 60, i * 60 + 60);
                Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.up;
                fractal.transform.DOLocalMove(fractalLength * dir, fractalDuration)
                    .OnComplete(() => { Destroy(fractal.gameObject);});
                fractal.transform.DORotate(new Vector3(0, 0, 360), fractalDuration);
                Image image = fractal.GetComponent<Image>();
                DOTween.ToAlpha(
                    ()=> image.color,
                    color => image.color = color,
                    0f,
                    fractalDuration 
                ).SetEase(Ease.InQuad);
            }
        }
    }
}
