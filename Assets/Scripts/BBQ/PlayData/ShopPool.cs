using System.Collections.Generic;
using System.Text;
using BBQ.Database;
using Unity.VisualScripting;
using UnityEngine;

namespace BBQ.PlayData {
    public class ShopPool {

        public List<int> foodsIndex;
        public string poolName;

        public string Encode() {
            return "";
        }

        public ShopPool(List<int> index, string name) {
            foodsIndex = index;
            poolName = name;
        }


        public static ShopPool Decode(string code) {
            List<int> index = new List<int>();
            for (int i = 0; i < 5; i++) {
                for (int j = 0; j < 10; j++) {
                    index.Add(j + i * 20);
                }
            }
            return new ShopPool(index, "defaultPool");
        }
    }
}