using System.Collections.Generic;
using System.Linq;
using System.Text;
using BBQ.Database;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

namespace BBQ.PlayData {
    public class ShopPool {

        public List<int> foodsIndex;
        public string poolName;

        public string Encode() {
            foodsIndex.Sort();
            string listText = poolName + ":" + string.Join(",", foodsIndex);
            string encoded = AesCipher.Encrypt(listText);
            return encoded;
        }

        public ShopPool(List<int> index, string poolName) {
            foodsIndex = index;
            this.poolName = poolName;
        }


        public static ShopPool Decode(string code) {
            string[] decoded = AesCipher.Decrypt(code).Split(":");
            string poolName = decoded[0];
            List<int> index = decoded[1].Split(",").Select(int.Parse).ToList();
            index.Sort();
            return new(index, poolName);
        }
    }
}