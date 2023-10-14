using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace BBQ.Common {
    public class DetailText : MonoBehaviour {

        [SerializeField] private Text text;
        [SerializeField] private List<Keyword> keywords;
        [SerializeField] private Vector2 centerPos;
        [SerializeField] private GameObject wordDetailPrefab;

        private Dictionary<Rect, Keyword> _wordAreas;

        private GameObject _nowDetail;


        void Update() {
            if (_wordAreas == null) _wordAreas = new Dictionary<Rect, Keyword>();
            
            Keyword word = GetWord();
            if (word != null && _nowDetail == null) {
                Transform detail = Instantiate(wordDetailPrefab).transform;
                detail.SetParent(GameObject.Find("Canvas").transform, false);
                detail.position = Input.mousePosition;
                DrawDetail(detail, word);
                _nowDetail = detail.gameObject;
            }

            if (word == null && _nowDetail != null) {
                Destroy(_nowDetail.gameObject);
            }
            

        }

        private void DrawDetail(Transform detail, Keyword keyword) {
            detail.Find("Title").GetComponent<Text>().text = keyword.word;
            detail.Find("Detail").GetComponent<Text>().text = keyword.describe;
        }

        public async void SetDetail(string msg) {
            _wordAreas = new Dictionary<Rect, Keyword>();
            text.text = msg;
            await UniTask.Delay(TimeSpan.FromSeconds(0.1));
            IList<UIVertex> vertexs = text.cachedTextGenerator.verts;
            Debug.Log(vertexs.Count);
            foreach (Keyword keyword in keywords) {
                MatchCollection mc = Regex.Matches(msg, keyword.word);
                foreach (Match match in mc) {
                    int idx = match.Index * 4;
                    UIVertex topLeft = vertexs[idx];
                    UIVertex bottomRight = vertexs[idx + 2];
        
                    topLeft.position /= text.pixelsPerUnit;
                    bottomRight.position /= text.pixelsPerUnit;

                    Rect rect = new Rect(topLeft.position.x, topLeft.position.y,
                        bottomRight.position.x - topLeft.position.x, bottomRight.position.y - topLeft.position.y);

                    _wordAreas[rect] = keyword;

                }
            }
            
        }


        Keyword GetWord() {
            Vector2 pos = Input.mousePosition / (Screen.width / 1920f);
            pos -= new Vector2(1920, 1080) / 2;
            pos -= centerPos;

            foreach (KeyValuePair<Rect,Keyword> pair in _wordAreas) {
                
                if (pair.Key.xMin > pos.x || pair.Key.xMax < pos.x) continue;
                if (pair.Key.yMax > pos.y || pair.Key.yMin < pos.y) continue;
                return pair.Value;
            }

            return null;
        }
        
    
    }
    
    [Serializable]
    public class Keyword {
        public string word;
        [Multiline]
        public string describe;

    }
}