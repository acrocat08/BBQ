using System.Collections.Generic;
using BBQ.Action;
using BBQ.Database;
using BBQ.Shopping;
using UnityEngine;
using ToolData = BBQ.Database.ToolData;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor( typeof( ToolData ) )]
public class ToolDataEditor : Editor
{
    public override Texture2D RenderStaticPreview
    ( 
        string assetPath, 
        Object[] subAssets, 
        int width, 
        int height 
    )
    {
        var obj = target as ToolData;
        var icon = obj.toolImage;

        if ( icon == null )
        {
            return base.RenderStaticPreview( assetPath, subAssets, width, height );
        }

        var preview = AssetPreview.GetAssetPreview( icon );
        var final = new Texture2D( width, height );

        //EditorUtility.CopySerialized( preview, final );

        return preview;
    }
}
#endif

namespace BBQ.Database {
    [CreateAssetMenu(menuName = "Database/Tool")]
    public class ToolData : ExplainableItem {
        public string toolName;
        public int tier;
        public int cost;
        public Sprite toolImage;
        public FoodAction action;
        public string targetArea;
        public override Sprite GetImage() {
            return toolImage;
        }
    }
    

}