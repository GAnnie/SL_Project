// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssetPaths.cs
// Author   : wenlin
// Created  : 2013/3/27 
// Purpose  : 
// **********************************************************************
using System;
using System.Collections.Generic;

namespace AssetBundleEditor
{
	public class AssetPaths
	{
		public Dictionary< string, AssetsGroup > assetPaths  = null;
        public Dictionary< string, AssetsGroup > scenePaths  = null;
		public Dictionary< string, AssetsGroup > commonPaths = null;

        public Dictionary<string,  AssetDependencisGroup> allDependencis = null;

		public AssetPaths ()
		{
            assetPaths 		 = new Dictionary<string, AssetsGroup>();
            scenePaths 		 = new Dictionary<string, AssetsGroup>();
			commonPaths      = new Dictionary<string, AssetsGroup>();

            allDependencis   = new Dictionary<string, AssetDependencisGroup>();
		}
	}
	

    public class AssetDependencisGroup
    {
        public AssetbundleType type;

        public Dictionary< string , AssetDependencis > dependencisDic = null;

        public AssetDependencisGroup()
        {
        }

        public AssetDependencisGroup( AssetbundleType type )
        {
            this.type = type;

            dependencisDic = new Dictionary<string,AssetDependencis>();
        }

    }
	
	public class AssetsGroup
	{
		//public string groupPath
		public string groupPath = String.Empty;
		
		//group name 
		public string groupName = String.Empty;

        //group export property
        public AssetExportProperty exportProperty = null;

		// sub file Message
		public Dictionary< string,  AssetSaveData > assets = null;
		
		public AssetsGroup() {}
		
		public AssetsGroup( string groupName, string groupPath )
		{
			this.groupName = groupName;
			
			this.groupPath = groupPath;

            this.exportProperty = new AssetExportProperty();

			this.assets = new Dictionary<string, AssetSaveData>();
		}
		
		
		public void SelectAllDirty( bool flag )
		{
			if ( this.assets != null )
			{
				foreach( KeyValuePair< string,  AssetSaveData > item in assets )
				{
					item.Value.isDirty = flag;					
				}
			}
		}
	}


}

