using UnityEngine;
using UnityEditor;

using System.Collections;

namespace AssetBundleEditor
{
    public class ExportAssetbundleGuideWin : EditorWindow
    {
        [MenuItem("AssetBundle/Export Assetbundle Guide", false, 0)]
        public static void ShowWin()
        {
            EditorWindow.GetWindow(typeof(ExportAssetbundleGuideWin));
        }

       private GUIStyle style;
        Vector2 scrollPos;
        void OnGUI()
        {

            if (style == null)
            {
                style = new GUIStyle();
                style.fontSize = 18;

                // style.normal.background = null;
                style.normal.textColor = new Color(1, 1, 0);
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height));
            {
                GameEditorUtils.PartitionLine();

                EditorGUILayout.LabelField(" ===========打包Assetbundle 制作流程============", style,  GUILayout.Width(500));

                GameEditorUtils.LabelAndButton(" -----> 1）修改游戏贴图格式大小属性--------------", "修改贴图属性", ChangerTexture, style);

                GameEditorUtils.RepeatString("|", 3, style);
				
				GameEditorUtils.LabelAndButton(" -----> 2）修改Atlas 属性(仅用于IOS版本)---------------", "修改Atlas 属性", ModifyAtlas, style);

                GameEditorUtils.RepeatString("|", 3, style);
				
				//GameEditorUtils.LabelAndButton(" -----> 3）切割模型动作---------------", "切割模型动作", ModelAnimationClip, style);

                //GameEditorUtils.RepeatString("|", 3, style);

				//GameEditorUtils.LabelAndButton(" -----> 4）打包区域信息---------------", "打包区域信息", ExportMaparea, style);

                //GameEditorUtils.RepeatString("|", 3, style);
				
				GameEditorUtils.LabelAndButton(" -----> 3）导出ResourceData---------------", "导出ResourceData", ExportResourceData, style);

                GameEditorUtils.RepeatString("|", 3, style);
				
				GameEditorUtils.LabelAndButton(" -----> 4）导出资源 -------------------", "导出资源", ExportAssetbundle, style);
            }
            EditorGUILayout.EndScrollView();
        }


        /// <summary>
        /// 修改贴图
        /// </summary>
        private void ChangerTexture()
        {
            TextureImporterSettingsWin.ShowWin();
        }

        /// <summary>
        /// 切割导出模型动作
        /// </summary>
        private void ModelAnimationClip()
        {
//            AnimationTool.ModelAnimationExport.ShowWin();
        }
		
		/// <summary>
		/// 打包区域信息
		/// </summary>
		private void ExportMaparea()
		{
//			MapAreasGenerator.ShowWin();
		}
		
		/// <summary>
		/// 导出ResourceData
		/// </summary>
		private void ExportResourceData()
		{
			AssetManager.Instance.ExportResourceVersionDataFormat();
		}

        /// <summary>
        /// 删除模型动作
        /// </summary>
        private void ModifyAtlas()
        {
           NGUIAtlasTool.ShowWin();
        }

        /// <summary>
        /// 显示导出窗口
        /// </summary>
        private void ExportAssetbundle()
        {
            AssetManagerWin.ShowWin();
        }

    }
}


