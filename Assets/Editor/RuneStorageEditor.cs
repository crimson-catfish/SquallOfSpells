using UnityEditor;
using UnityEngine.Windows;

namespace Rune.Editor
{
    public class RuneStorageEditor : UnityEditor.Editor
    {
        private const string runesPath    = "Assets/Resources/Runes/";
        private const string previewsPath = "Assets/Textures/Runes/Previews/";

        [MenuItem("Rune system/delete all")]
        public static void DeleteAll()
        {
            FileUtil.DeleteFileOrDirectory(runesPath);
            Directory.CreateDirectory(runesPath);

            FileUtil.DeleteFileOrDirectory(previewsPath);
            Directory.CreateDirectory(previewsPath);
        }
    }
}