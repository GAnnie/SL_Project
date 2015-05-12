using UnityEngine;
using UnityEditor;

namespace Foundation.Core.Editor
{
    [InitializeOnLoad]
    public class HierarchySortKeybindings
    {
        [MenuItem("GameObject/Selection/Sort Transform Up &UP")]
        public static void SortHierarchyUp()
        {
            SortHierarchyBy(-1);
        }

        [MenuItem("GameObject/Selection/Sort Transform Down &DOWN")]
        public static void SortHierarchyDown()
        {
            SortHierarchyBy(1);
        }

        static void SortHierarchyBy(int offset)
        {
            if (Selection.activeTransform != null)
            {
				int siblingIndex = Selection.activeTransform.GetSiblingIndex();
				if(siblingIndex == 0 && offset < 0){
					Selection.activeTransform.SetAsLastSibling();
				}else{
					Selection.activeTransform.SetSiblingIndex(siblingIndex + offset);
					if(siblingIndex == Selection.activeTransform.GetSiblingIndex()){
						Selection.activeTransform.SetAsFirstSibling();
					}
				}
            }
        }
        
    }
}