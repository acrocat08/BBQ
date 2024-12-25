using System.Collections.Generic;
using System.Linq;
using BBQ.Database;
using UnityEngine;
using UnityEngine.Serialization;

namespace BBQ.Shopping {
    public class FoodStorage : MonoBehaviour {
        private Dictionary<FoodData, int> _storage;
        [SerializeField] private int storageNum;
        [SerializeField] private ItemSet itemSet;

        public void Init() {
            _storage = new Dictionary<FoodData, int>();
            foreach (FoodData food in itemSet.foods) {
                _storage[food] = storageNum;
            }
        }

        public void UseFood(FoodData food) {
            if (!_storage.Keys.Contains(food)) return;
            _storage[food]--;
        }

        public bool CheckStorage(FoodData food) {
            if (!_storage.Keys.Contains(food)) return true;
            return _storage[food] > 0;
        }
    }
}
