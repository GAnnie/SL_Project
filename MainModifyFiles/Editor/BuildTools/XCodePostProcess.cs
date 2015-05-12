using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace UnityEditor.H1Editor
{
    public static class XCodePostProcess
    {
		[PostProcessBuild(100)]
		public static void OnPostProcessBuild(BuildTarget target, string path)
		{
			if (target == BuildTarget.iPhone)
			{
				PlayerSettings.bundleIdentifier = "com.baoyugame.h1.ios.dev";
				PlayerSettings.bundleVersion = Version.bundleVersion;
			}

			if (target == BuildTarget.Android)
			{
				PlayerSettings.bundleIdentifier = "com.baoyugame.h1.android.dev";
				PlayerSettings.bundleVersion = Version.bundleVersion;
			}
		}
    }
}
