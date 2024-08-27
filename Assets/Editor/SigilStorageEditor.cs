using UnityEditor;
using UnityEngine.Windows;

namespace SquallOfSpells.Editor
{
    public class SigilStorageEditor : UnityEditor.Editor
    {
        private const string sigilPath    = "Assets/Resources/Sigils/";
        private const string previewsPath = "Assets/Textures/Sigils/Previews/";

        [MenuItem("Sigil system/delete all")]
        public static void DeleteAll()
        {
            FileUtil.DeleteFileOrDirectory(sigilPath);
            Directory.CreateDirectory(sigilPath);

            FileUtil.DeleteFileOrDirectory(previewsPath);
            Directory.CreateDirectory(previewsPath);
        }
    }
}