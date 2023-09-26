using System;
using System.Collections.Generic;
using BBQ.Database;
using UnityEngine;
using System.Linq;
using BBQ.Action;
using BBQ.Common;
using BBQ.Cooking;
using BBQ.PlayData;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using SoundMgr;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BBQ.Shopping {
    public class Shop : MonoBehaviour, IReleasable {
        [SerializeField] private ShopItemFactory itemFactory;
        [SerializeField] private ShopView view;
        [SerializeField] private PointSensor pointSensor;
        [SerializeField] private Transform detailContainer;
        [SerializeField] private Reroller reroller;
        private Coin _coin;
        private Carbon _carbon;
        private ShopFood[] _foods;
        private ShopTool _tool;
        private int _level;
        private int _levelUpDiscount;
        [SerializeField] private List<int> levelUpCosts;
        [SerializeField] private ActionAssembly assembly;
        [SerializeField] private ActionEnvironment env;
        [SerializeField] private ItemDetail detail;

        public void Init(int level, int levelupDiscount, Coin coin, Carbon carbon, int rerollTicket) {
            _foods = new ShopFood[] { null, null, null, null, null };
            _level = level;
            _levelUpDiscount = levelupDiscount;
            _coin = coin;
            _carbon = carbon;
            reroller.Init(this, _coin, rerollTicket);
            reroller.Reroll();
            view.UpdateText(this, levelUpCosts[_level - 1] - _levelUpDiscount);
        }
        
        public async void BuyFood(ShopFood shopFood, DeckInventory inventory) {
            if (!CheckCanBuyFood(shopFood, _coin, inventory)) return;
            InputGuard.Lock();
            _coin.Use(shopFood.GetFoodData().cost);
            inventory.AddItem(shopFood.deckFood);
            DeleteFoods(new List<ShopFood>{shopFood});            
            SoundPlayer.I.Play("se_buy");
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            await TriggerObserver.I.Invoke(ActionTrigger.Buy, new List<DeckFood>{shopFood.deckFood}, true);
            await TriggerObserver.I.Invoke(ActionTrigger.BuyOthers, new List<DeckFood>{shopFood.deckFood}, false);
            await UniTask.DelayFrame(1);
            pointSensor.UpdateArea();
            InputGuard.UnLock();
        }

        bool CheckCanBuyFood(ShopFood shopFood, Coin coin, DeckInventory inventory) {
            if (coin.GetCoin() < shopFood.GetFoodData().cost) return false;
            return inventory.CheckIsEmpty();
        }


        public void DeleteFoods(List<ShopFood> items) {
            if (_foods == null) return;
            for (int i = 0; i < 5; i++) {
                if (items.Contains(_foods[i])) {
                    TriggerObserver.I.RemoveFood(_foods[i].deckFood);
                    Destroy(_foods[i].gameObject);
                    _foods[i] = null;
                }
            }
        }

        public async void DeleteTool() {
            Destroy(_tool.gameObject);
            _tool = null;
            await UniTask.DelayFrame(1);
            pointSensor.UpdateArea();
        }
        
        public List<ShopFood> GetShopFoods() {
            return _foods.Where(x => x != null).ToList();
        }

        public async UniTask AddFoods(List<FoodData> data) {
            await MoveFoods();
            int cnt = 0;
            for (int i = 0; i < 5; i++) {
                if (cnt >= data.Count) break;
                if (_foods[i] != null) continue;
                _foods[i] = itemFactory.CreateFood(data[cnt], this, detail);
                _foods[i].deckFood.Releasable = this;
                view.PlaceFood(_foods[i], i, transform);
                TriggerObserver.I.RegisterFood(_foods[i].deckFood);
                cnt++;
            }
            await UniTask.DelayFrame(1);
            pointSensor.UpdateArea();
        }

        public async UniTask AddTool(ToolData tool) {
            if (_tool != null) {
                _tool.Drop();
                DeleteTool();
            }
            _tool = itemFactory.CreateTool(tool, this, detail);
            view.PlaceTool(_tool, transform);
            await UniTask.DelayFrame(1);
            pointSensor.UpdateArea();
        }

        async UniTask MoveFoods() {
            List<ShopFood> listed = _foods.Where(x => x != null).ToList();
            if (listed.Count == 0) return;
            if (listed.Count == 5) {
                _foods[0].Drop();
                TriggerObserver.I.RemoveFood(_foods[0].deckFood);
                _foods[0] = null;
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                listed = _foods.Where(x => x != null).ToList();
            }
            for (int i = 0; i < listed.Count; i++) {
                if(_foods[i] == null) break;
                if (i == listed.Count - 1) return;
            }
            _foods = new ShopFood[] { null, null, null, null, null };
            for (int i = 0; i < listed.Count; i++) {
                _foods[i] = listed[i];
                view.MoveFood(_foods[i], i, transform);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        }

        public int GetShopLevel() {
            return _level;
        }

        public int GetLevelUpDiscount() {
            return _levelUpDiscount;
        }

        void LevelUp() {
            int cost = levelUpCosts[_level - 1] - _levelUpDiscount;
            _coin.Use(cost);
            _level += 1;
            _levelUpDiscount = 0;
            view.UpdateText(this, levelUpCosts[_level - 1]);
        }

        public void OnLevelUpButtonClicked() {
            int cost = levelUpCosts[_level - 1] - _levelUpDiscount;
            if (_coin.GetCoin() < cost) return;
            SoundPlayer.I.Play("se_levelup");
            LevelUp();
        }

        public List<FoodObject> ReleaseFoods(List<DeckFood> foods) {
            return new List<FoodObject>();
        }

        public FoodObject GetObject(DeckFood food) {
            return _foods.FirstOrDefault(x => x.deckFood == food);
        }

        public async void UseTool(ShopTool shopTool, List<DeckFood> target) {
            if (_carbon.GetCarbon() < shopTool.data.cost) return;
            InputGuard.Lock();
            _carbon.Use(shopTool.data.cost);
            DeleteTool();
            await assembly.Run(shopTool.data.action.sequences[0].commands, env, null, target);
            InputGuard.UnLock();
        }
    }
}