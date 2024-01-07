using System;
using System.Collections.Generic;
using System.Linq;
using BBQ.Database;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BBQ.Shopping {
    [CreateAssetMenu(menuName = "Shop/View")]
    public class ShopView : ScriptableObject {

        [SerializeField] private int itemWidth;
        [SerializeField] private int margin;
        [SerializeField] private float moveDuration;

        [SerializeField] private GameObject levelUpPrefab;
        [SerializeField] private float levelUpShowUpDuration;
        [SerializeField] private Ease levelUpShowUpEasing;
        [SerializeField] private GameObject levelUpImagePrefab;
        [SerializeField] private float levelUpImageLength;
        [SerializeField] private Ease levelUpImageEasing;
        [SerializeField] private float levelUpShowDuration;
        [SerializeField] private float levelUpFadeDuration;
        [SerializeField] private ItemSet itemSet;
        [SerializeField] private ViewParam param;

        public void PlaceFood(ShopFood item, int index, Transform container) {
            item.transform.SetParent(container, false);
            item.GetComponent<RectTransform>().anchoredPosition
                = (itemWidth + margin) * index * Vector3.right;
            item.Fall();
        }
        
        public void PlaceTool(ShopTool item, Transform container) {
            item.transform.SetParent(container, false);
            item.GetComponent<RectTransform>().anchoredPosition
                = (itemWidth + margin) * 5 * Vector3.right;
            item.Fall();
        }
        
        public void MoveFood(ShopFood item, int index, Transform container) {
            item.transform.SetParent(container, false);
            Vector3 toPos = (itemWidth + margin) * index * Vector3.right;
            item.GetComponent<RectTransform>().DOLocalMove(toPos, moveDuration);
        }

        public void UpdateText(Shop shop, int cost) {
            Text levelText = shop.transform.Find("Level").Find("Text").GetComponent<Text>();
            levelText.text = "Level " + shop.GetShopLevel();
            if (shop.GetShopLevel() == 5) {
                shop.transform.Find("LevelUp").gameObject.SetActive(false);
            }
            else {
                Text buttonText = shop.transform.Find("LevelUp").Find("Cost").GetComponent<Text>();
                buttonText.text = cost.ToString(); 
            }

        }

        public async UniTask LevelUp(int level) {
            
            RectTransform levelup = Instantiate(levelUpPrefab).GetComponent<RectTransform>();
            levelup.Find("Title").GetComponent<Text>().text = "Shop Level " + level;
            levelup.Find("Title").GetComponent<Text>().color = param.tierColors[level];
            levelup.Find("Explain").GetComponent<Text>().text = "ティア" + level + "の食材と道具が解放されました！";
            levelup.SetParent(GameObject.Find("Canvas").transform, false);
            Vector2 toSize = levelup.sizeDelta;
            toSize.x = Screen.width;

            DOTween.To(
                () => levelup.sizeDelta,
                x => levelup.sizeDelta = x,
                toSize,
                levelUpShowUpDuration
            ).SetEase(levelUpShowUpEasing);

            Transform container = levelup.Find("Container").transform;
            
            List<Sprite> images = itemSet.GetFoodPool().Where(x => x.tier == level).Select(x => x.foodImage)
                .Concat(itemSet.tools.Where(x => x.tier == level).Select(x => x.toolImage))
                .OrderBy(x => Guid.NewGuid())
                .ToList();

            for (int i = 0; i < images.Count; i++) {
                Transform itemImage = Instantiate(levelUpImagePrefab).transform;
                itemImage.SetParent(container, false);
                itemImage.GetComponent<Image>().sprite = images[i];
                itemImage.localPosition = Vector3.zero;
                itemImage.transform.Rotate(0, 0, Random.Range(0, 360));
                int angle = Random.Range(i * (360 / images.Count), i * (360 / images.Count) + (180 / images.Count));
                Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.up;
                itemImage.DOLocalMove(levelUpImageLength * dir, levelUpShowUpDuration + levelUpShowDuration).SetEase(levelUpImageEasing);
                itemImage.DORotate(new Vector3(0, 0, 360), levelUpShowUpDuration + levelUpShowDuration);
            }

            container.DOLocalRotate(new Vector3(0, 0, 90), levelUpShowUpDuration + levelUpShowDuration)
                .SetEase(levelUpImageEasing);

            await UniTask.Delay(TimeSpan.FromSeconds(levelUpShowUpDuration + levelUpShowDuration));

            
            DOTween.To(
                () => levelup.GetComponent<CanvasGroup>().alpha,
                x => levelup.GetComponent<CanvasGroup>().alpha = x,
                0,
                levelUpFadeDuration
            ).SetEase(Ease.Linear);


            await UniTask.Delay(TimeSpan.FromSeconds(levelUpFadeDuration));
            Destroy(levelup.gameObject);
        }
    }

}