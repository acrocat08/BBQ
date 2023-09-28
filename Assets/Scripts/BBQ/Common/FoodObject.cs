using BBQ.Cooking;
using BBQ.PlayData;
using UnityEngine;

namespace BBQ.Common {
    public class FoodObject : MonoBehaviour {

        [SerializeField] protected FoodObjectView view;
        public DeckFood deckFood;
        
        public void Init(DeckFood food) {
            deckFood = food;
            view.Draw(this);
        }

        public void Hit() {
            view.Hit(this);
        }

        public virtual void Drop() {
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

        public void SetEffect() {
            view.AddEffect(this);
        }

        public void OnInvoke() {
            view.Invoke(this);
        }

        public virtual void LankUp() {
        }

        public void UpdateStack() {
            view.UpdateStack(this);
        } 

    }
}
