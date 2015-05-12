// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssetPathData.cs
// Author   : wenlin
// Created  : 2013/3/26 
// Purpose  : 
// **********************************************************************
using System;
using System.Collections.Generic;

namespace AssetBundleEditor
{
	public class AssetSaveData
	{
		/// <summary>
		/// The version of asset.
		/// </summary>
		public long version = 0;
		/// <summary>
		/// The time of CurVersion.
		/// </summary>
		public string versionTime = "";
		/// <summary>
		/// The file time.
		/// </summary>
		public string fileTime 	  = "";
		/// <summary>
		/// The path of asset
		/// </summary>
		public string path = "";
        /// <summary>
        /// The path of export
        /// </summary>
        public string exportPath = "";
		/// <summary>
		/// the group of asset
		/// </summary>
		public string groupName = "";
		/// <summary>
		/// The name of the file.
		/// </summary>
		public string fileName  = "";
        /// <summary>
        /// The id of the file
        /// </summary>
        public string fileID    = "";
        /// <summary>
        /// asset guid
        /// </summary>
        public string guid = string.Empty;
		/// <summary>
		/// the code of md5 
		/// </summary>
		public string md5   = "";
		/// <summary>
		/// The chang md5.
		/// </summary>
		public string changMd5 = "";
		/// <summary>
		/// Is asset is dirty
		/// </summary>
		public bool   isDirty  = false;
        /// <summary>
        /// Is reference Resource Dirty
        /// </summary>
        public bool isReferenceDirty = false;
		/// <summary>
		/// The dataFile is exist.
		/// </summary>
		public bool   isExist  = true;
        /// <summary>
        /// The dataFile is exported
        /// </summary>
        public bool isExport = false;
        /// <summary>
        /// The dataFile is need uncompress 
        /// </summary>
        public bool needUncompress = false;
        /// <summary>
        /// The dataFile size
        /// </summary>
        public uint dataSize = 0;

        //大包目录
        public string package = string.Empty;

        //大包编号
        public int packageNum = 0;

        //在大包中的位置
        public int packagePos = 0; 

        /// <summary>
        /// Asset Type
        /// </summary>
        public AssetbundleType type;

		public AssetSaveData ()
		{}

        public AssetSaveData( AssetbundleType type )
        {
            this.type = type;
        }
	}


    public class AssetDependencis
    {
        public string md5 = string.Empty;

        public bool isChang = true;

        public string path = string.Empty;

        public AssetDependencis()
        { }
    }
}

