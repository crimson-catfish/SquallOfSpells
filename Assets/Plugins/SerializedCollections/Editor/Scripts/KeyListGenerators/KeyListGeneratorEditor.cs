using UnityEditor;

namespace AYellowpaper.SerializedCollections.KeysGenerators
{
    [CustomEditor(typeof(KeyListGenerator), true)]
    public class KeyListGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var iterator = this.serializedObject.GetIterator();

            if (iterator.Next(true))
            {
                // skip script name
                iterator.NextVisible(true);

                while (iterator.NextVisible(true))
                {
                    EditorGUILayout.PropertyField(iterator);
                }
            }

            this.serializedObject.ApplyModifiedProperties();
        }
    }
}