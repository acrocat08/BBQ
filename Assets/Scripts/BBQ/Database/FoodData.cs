using BBQ.Action;
using BBQ.Database;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor( typeof( FoodData ) )]
public class FoodDataEditor : Editor
{
    public override Texture2D RenderStaticPreview
    ( 
        string assetPath, 
        Object[] subAssets, 
        int width, 
        int height 
    )
    {
        var obj = target as FoodData;
        var icon = obj.foodImage;

        if ( icon == null )
        {
            return base.RenderStaticPreview( assetPath, subAssets, width, height );
        }

        var preview = AssetPreview.GetAssetPreview( icon );
        var final = new Texture2D( width, height );

        EditorUtility.CopySerialized( preview, final );

        return final;
    }
}
#endif

namespace BBQ.Database {
    [CreateAssetMenu(menuName = "Database/Food")]
    public class FoodData : ScriptableObject {
        public string foodName;
        public int tier;
        public int cost;
        public Sprite foodImage;
        public Color color;
        public bool isToken;
        public FoodAction action;
    }
    

}

