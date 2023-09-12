using System.Collections;
using System.Collections.Generic;
using BBQ.Database;
using BBQ.PlayData;
using SoundMgr;
using UnityEngine;

namespace BBQ.Cooking {
    public class LaneFood : MonoBehaviour {
        public DeckFood deckFood;
        
        [SerializeField] private LaneFoodView view;

        
        
        public void Init(DeckFood food) {
            deckFood = food;
            view.Draw(this, deckFood.data);
        }

        public void Hit() {
            view.Hit(this);
        }

        public void Drop() {
            transform.SetParent(GameObject.Find("Canvas").transform);
            view.Drop(this);
        }

        public void Freeze() {
            deckFood.isFrozen = true;
            view.Freeze(this);
        }
        
        public void Fire() {
            deckFood.isFired = true;
            view.Fire(this);
        }

        public FoodData GetData() {
            return deckFood.data;
        }

        public void OnInvoke() {
            view.Invoke(this);
        }

    }
    
}