using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BBQ.Cooking {
    public class ShotEffect : MonoBehaviour {
        [SerializeField] private float length;
        [SerializeField] private float duration;

        public void Init(float angle) {
            transform.localPosition = Vector3.zero;
            transform.Rotate(Vector3.forward, Random.Range(0, 360));
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.up;
            transform.DOLocalMove(length * dir, duration)
                .OnComplete(() => { Destroy(gameObject);});
            Image image = GetComponent<Image>();
            transform.DOLocalRotate(new Vector3(0, 0, 180 * (Random.Range(0, 1) * 2 - 1)), duration).SetEase(Ease.OutQuad);
            /*
            DOTween.ToAlpha(
                ()=> image.color,
                color => image.color = color,
                0f,
                duration 
            ).SetEase(Ease.InSine);
            */
            transform.DOScale(Vector3.zero, duration).SetEase(Ease.InQuart);
        }
        
    }
}
