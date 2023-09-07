using System;
using System.Collections.Generic;
using System.ComponentModel;
using BBQ.Cooking;
using BBQ.PlayData;
using UnityEngine;

namespace BBQ.Action {
    [Serializable]
    public class ActionEnvironment : MonoBehaviour {
        [HideInInspector] public Board board;
        [HideInInspector] public List<Lane> lanes;
        [HideInInspector] public Deck deck;
        [HideInInspector] public Dump dump;
        [HideInInspector] public HandCount handCount;
        [HideInInspector] public CookTime time;
        [HideInInspector] public Coin coin;
        public void Init(Board board, List<Lane> lanes, Deck deck, Dump dump, HandCount handCount, CookTime time, Coin coin) {
            this.board = board;
            this.lanes = lanes;
            this.deck = deck;
            this.dump = dump;
            this.handCount = handCount;
            this.time = time;
            this.coin = coin;
        }
    }
    
    [Serializable]
    public class ActionVariable {
        public string n1;
        public string n2;
        public int x1, x2, x3;
        public List<DeckFood> f1, f2, f3;
        public DeckFood invoker;
        public List<DeckFood> target;

        public ActionVariable(DeckFood invoker, List<DeckFood> target) {
            n1 = "";
            n2 = "";
            x1 = 0;
            x2 = 0;
            x3 = 0;
            f1 = new List<DeckFood>();
            f2 = new List<DeckFood>();
            f3 = new List<DeckFood>();
            this.invoker = invoker;
            this.target = target;
        }
        
        public int GetNum(string index) {
            if (index == null) Debug.LogWarning("値が設定されていません。"); 
            if (index == "x1") return x1;
            if (index == "x2") return x2;
            if (index == "x3") return x3;
            return int.Parse(index);
        }
        

        
        
        
        public List<DeckFood> GetFoods(string index) {
            if (index == null) Debug.LogWarning("値が設定されていません。"); 
            if (index == "f1") return f1;
            if (index == "f2") return f2;
            if (index == "f3") return f3;
            return new List<DeckFood>();
        }
        
        /*
        public void SetNum(string index, int val) {
            if (index == null) Debug.LogWarning("値が設定されていません。"); 
            if (index == "x1") x1 = val;
            if (index == "x2") x2 = val;
            if (index == "x3") x3 = val;
        }
        
        public void SetFoods(string index, List<DeckFood> val) {
            if (index == null) Debug.LogWarning("値が設定されていません。"); 
            if (index == "f1") f1 = val;
            if (index == "f2") f2 = val;
            if (index == "f3") f3 = val;
        }
        */
    }
}
