using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BBQ.Title {
    [CreateAssetMenu(menuName = "TitleMenu/View")]
    public class TitleMenuView : ScriptableObject {

        [SerializeField] private float logoFloatLength;
        [SerializeField] private float logoFloatSpeed;
        [SerializeField] private float waveLength;
        [SerializeField] private float waveSpeed;
        [SerializeField] private Vector3 waveDirection;
        [SerializeField] private GameObject smogPrefab;
        [SerializeField] private Vector3 smogMinPos;
        [SerializeField] private Vector3 smogMaxPos;
        private List<Transform> _smogs;
        private bool _isMoving;
        private List<string> _baseMenuText;
        
        public async void FloatLogo(TitleMenu titleMenu) {
            Transform logo = titleMenu.transform.Find("Logo");
            Vector3 centerPos = logo.localPosition;
            while (logo != null) {
                logo.localPosition = centerPos + logoFloatLength * Vector3.up * Mathf.Sin(logoFloatSpeed * Time.time);
                await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
            }
        }

        public async void Wave(Transform wave) {
            Vector3 centerPos = wave.localPosition;
            while (wave != null) {
                wave.localPosition = centerPos + waveLength * waveDirection.normalized * Mathf.Sin(waveSpeed * Time.time);
                await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
            }
        }

        public async void Smog(Transform bg) {
            if (_smogs == null) _smogs = new List<Transform>();
            while (bg != null) {
                if (_isMoving) return;
                await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(0.2f, 1f)));
                int num = Random.Range(1, 3);
                for (int i = 0; i < num; i++) {
                    Vector3 center = new Vector3(Random.Range(smogMinPos.x, smogMaxPos.x),
                        Random.Range(smogMinPos.y, smogMaxPos.y));
                    MakeSmog(center + Vector3.right * Random.Range(10, 30) + Vector3.up * Random.Range(10, 30));
                }
            }
        }

        async void MakeSmog(Vector3 pos) {
            Transform smog = Instantiate(smogPrefab).transform;
            float life = Random.Range(7, 12);
            smog.localScale = Vector3.one * Random.Range(0.5f, 2f);
            smog.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            smog.GetComponent<Image>().color = new Color(1, 1, 1) * Random.Range(1, 0.75f);
            smog.SetParent(GameObject.Find("Canvas").transform);
            smog.localPosition = pos;
            smog.DOLocalMoveY(smog.localPosition.y + 200f * Random.Range(1f, 1.5f) * (Screen.width / 1920f), life).SetEase(Ease.OutQuad);
            smog.DOScale(smog.transform.localScale * Random.Range(1.5f, 3f), life);
            smog.GetComponent<Image>().DOFade(0f, life);
            _smogs.Add(smog);
            await UniTask.Delay(TimeSpan.FromSeconds(life));
            _smogs.Remove(smog);
            Destroy(smog.gameObject);
        }

        public void GotoNext() {
            _isMoving = true;
            foreach (Transform smog in _smogs) {
                if (smog == null) continue;
                Destroy(smog.gameObject);
            }
        }

        public void UpdateText(List<Text> menuText, int index) {
            if (_baseMenuText.Count == 0) {
                _baseMenuText = menuText.Select(x => x.text).ToList();
            }
            for (int i = 0; i < menuText.Count; i++) {
                if (i == index) menuText[i].text = "→  " + _baseMenuText[i];
                else menuText[i].text = _baseMenuText[i];
            }
        }
    }
}
