using System.Collections.Generic;
using UnityEngine;

namespace BBQ.Shopping {
    public class ExplainableItem : ScriptableObject {
        public List<ExplainableItem> subItem;
        
        public virtual Sprite GetImage() {
            return null;
        }
        
    }
}
