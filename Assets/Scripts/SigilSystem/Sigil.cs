using System;
using UnityEditor;
using UnityEngine;

namespace SquallOfSpells.SigilSystem
{
    /// <summary>
    ///     width always 1, height is positive.
    /// </summary>
    [Serializable]
    public class Sigil : ScriptableObject
    {
        public Vector2[] points;

        public const float width = 1;

        public float   height;
        public Vector2 massCenter = Vector2.zero;

        public string previewPath;

        private Texture2D preview;

#if UNITY_EDITOR
        public Texture2D Preview {
            get
            {
                if (preview == null)
                    preview = AssetDatabase.LoadAssetAtPath<Texture2D>(previewPath);

                return preview;
            }
            set => preview = value;
        }
#endif
    }
}