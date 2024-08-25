using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace BBQ.Common {
    public class DetailText : MonoBehaviour {

        [SerializeField] private Text text;
        [SerializeField] private List<Keyword> keywords;
        [SerializeField] private Vector2 centerPos;
        [SerializeField] private GameObject wordDetailPrefab;
        [SerializeField] private Transform basePos;
        [SerializeField] private GameObject underBar;
        [SerializeField] private Transform underBarPos;

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
            /*
            foreach (Transform under in underBarPos) {
                Destroy(under.gameObject);
            }
            */
            text.text = msg;
            var wordList = msg.Replace("\r\n","\n").Split(new[]{'\n','\r'});
            msg = string.Join("", wordList);
            await UniTask.Delay(TimeSpan.FromSeconds(0.1));
            IList<UIVertex> vertexs = text.cachedTextGenerator.verts;
            foreach (Keyword keyword in keywords) {
                MatchCollection mc = Regex.Matches(msg, keyword.word);
                foreach (Match match in mc) {
                    int idx = match.Index * 4;
                    UIVertex topLeft = vertexs[idx];
                    UIVertex bottomRight = vertexs[idx + 2];
        
                    topLeft.position /= text.pixelsPerUnit;
                    bottomRight.position /= text.pixelsPerUnit;


                    Rect rect = new Rect(topLeft.position.x, topLeft.position.y,
                        (bottomRight.position.x - topLeft.position.x) * match.Length, bottomRight.position.y - topLeft.position.y);
                    /*
                    GameObject under = Instantiate(underBar, underBarPos);
                    RectTransform rt = under.GetComponent<RectTransform>();
                    rt.anchorMin = new Vector2(0.5f, 0.5f);
                    rt.anchorMax = new Vector2(0.5f, 0.5f);
                    rt.offsetMin = rect.min;
                    rt.offsetMax = rect.max;
                    var sizeDelta = rt.sizeDelta;
                    sizeDelta = new Vector2(sizeDelta.x, -sizeDelta.y);
                    rt.sizeDelta = sizeDelta;
                    */
                    _wordAreas[rect] = keyword;

                }
            }
            
        }


        Keyword GetWord() {
            Vector2 _basePos = basePos.GetComponent<RectTransform>().position;
            Vector2 windowSize = new Vector2(Screen.width, Screen.height) -
                                  _basePos * 2;
            float ratio = 1920f / windowSize.x;
            Vector2 pos = ((Vector2)Input.mousePosition - _basePos) * ratio;
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