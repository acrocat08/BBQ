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
            string listText = string.Join(",", foodsIndex);
            string encoded = AesCipher.Encrypt(listText);
            return encoded;
        }

        public ShopPool(List<int> index, string poolName) {
            foodsIndex = index;
            this.poolName = poolName;
        }


        public static ShopPool Decode(string code, string poolName) {
            string decoded = AesCipher.Decrypt(code);
            List<int> index = decoded.Split(",").Select(int.Parse).ToList();
            index.Sort();
            return new ShopPool(index, poolName);
        }
    }
}