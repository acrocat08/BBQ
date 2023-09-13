using System.Collections.Generic;
using BBQ.Action;
using BBQ.Database;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor( typeof( FoodEffect ) )]
public class FoodEffectEditor : Editor
{
    public override Texture2D RenderStaticPreview
    ( 
        string assetPath, 
        Object[] subAssets, 
        int width, 
        int height 
    )
    {
        var obj = target as FoodEffect;
        var icon = obj.effectImage;

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
    [CreateAssetMenu(menuName = "Database/Effect")]
    public class FoodEffect : ScriptableObject {
        public string effectName;
        public Sprite effectImage;
        public FoodAction action;
        public List<ActionCommand> onAttached;
        public List<ActionCommand> onReleased;
    }
    

}