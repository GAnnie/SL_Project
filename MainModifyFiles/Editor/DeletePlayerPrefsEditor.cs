using UnityEditor;
using UnityEngine;

namespace Foundation.Core.Editor
{
    public class DeletePlayerPrefsEditor
    {
        [MenuItem("Tools/Delete All Player Prefs")]
        public static void CreateAppServices()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}