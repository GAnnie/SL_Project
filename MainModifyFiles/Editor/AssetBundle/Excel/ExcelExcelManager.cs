using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System;
using System.IO;


namespace AssetBundleEditor
{
    public class AssetBundleExportLogManager
    {
        //private static string ODBC_SHEET = "AssetBundle";

		private static readonly AssetBundleExportLogManager instance = new AssetBundleExportLogManager();
	    public static AssetBundleExportLogManager Instance
	    {
	        get
			{
				return instance;
			}
	    }
        private AssetBundleExportLogManager(){}

        //private enum EXCEL_STATE
        //{
        //    NONE = 0,
        //    WRITE = 1
        //};

        //private EXCEL_STATE _curState = EXCEL_STATE.NONE;
        //private OdbcConnection oCon = null;

        private FileStream fs = null;
        private StreamWriter sw = null;
        private string logFileName = "ExportOutputLog.txt";
        /// <summary>
        /// begin to Write message to EXCEL
        /// return error message
        /// </summary>
        /// <returns></returns>
        public string Begin( string exportData )
        {
            //string exportData = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
            string fileFolderPath = AssetBundleExport.exportPath + "/VersionControl-" + exportData + "/";

            if( !Directory.Exists( fileFolderPath )) 
            {
                Directory.CreateDirectory(fileFolderPath);
            }

            string filePath = fileFolderPath + logFileName;

            try
            {
                fs = new FileStream( filePath, FileMode.Create );
                sw = new StreamWriter(fs);
            }
            catch( Exception e )
            {
                return e.Message;
            }

            return string.Empty;
        }

        /// <summary>
        /// Write Export Head Message
        /// </summary>
        /// <param name="name"></param>
        /// <param name="time"></param>
        public void WriteExportHeadMessage(string name, string time)
        {
            if (fs != null)
            {
                sw.WriteLine("导出者 : " + name + " 导出时间： " + time);
            }
//#if  !UNITY_IPHONE
//            if (_curState != EXCEL_STATE.WRITE) return;
//            if (oCon == null)
//            {
//                _curState = EXCEL_STATE.NONE;
//                return;
//            }

//            WriteOdbcCommand(toText("INSERT INTO [{1}$] VALUES ('','','','','','','','')", ODBC_SHEET), oCon);
//            WriteOdbcCommand(toText("INSERT INTO [{1}$] VALUES ('{2}','{3}','','','','','','')", ODBC_SHEET, name, time), oCon);
//#endif
        }

        /// <summary>
        /// Write Change Message
        /// </summary>
        /// <param name="data"></param>
        public void WriteAssetChangeMessage(AssetSaveData data)
        {
            _WriteChangeMessage(data, "Change Asset");
        }

        /// <summary>
        /// Write New Message
        /// </summary>
        /// <param name="data"></param>
        public void WriteAssetNewMessage(AssetSaveData data)
        {
            _WriteNewMessage(data, "New Asset");    
        }

        /// <summary>
        /// Write Delete Message
        /// </summary>
        /// <param name="data"></param>
        public void WriteAssetDelMessage(AssetSaveData data)
        {
            _WriteDeleteMessage(data, "Delete Asset");
        }

        /// <summary>
        /// Write new scene message
        /// </summary>
        /// <param name="data"></param>
        public void WriteAssetNewSceneMessage(AssetSaveData data)
        {
            _WriteNewMessage(data, "NewScene");
        }

        public void WriteAssetDelSceneMessage(AssetSaveData data)
        {
            _WriteDeleteMessage(data, "DeleteScene");
        }

        public void WriteAssetChangeSceneMessage(AssetSaveData data)
        {
            _WriteChangeMessage(data, "ChangeScene");
        }

        public void WriteAssetNewConfigMessage(AssetSaveData data)
        {
            _WriteNewMessage(data, "NewConfigFile");
        }

        public void WriteAssetDelConfigMessage(AssetSaveData data)
        {
            _WriteDeleteMessage(data, "DeleteConfigFile");
        }


        public void WriteAssetChangeConfigMessage(AssetSaveData data)
        {
            _WriteChangeMessage(data, "ChangeConfigFile");
        }
		
        public void WriteAssetNewCommoneObjectMessage(AssetSaveData data)
        {
            _WriteNewMessage(data, "NewCommonFile");
        }

        public void WriteAssetDelCommonObjMessage(AssetSaveData data)
        {
            _WriteDeleteMessage(data, "DeleteCommonFile");
        }


        public void WriteAssetChangeCommonObjMessage(AssetSaveData data)
        {
            _WriteChangeMessage(data, "ChangeCommonFile");
        }		
		

        public void WriteNewAtlasMessage(AssetSaveData data)
        {
            _WriteNewMessage(data, "NewAtlas");
        }

        public void WriteDeleteAtlasMessage(AssetSaveData data)
        {
            _WriteDeleteMessage(data, "DeleteAtlas");
        }


        public void WriteChangeAtlasMessage(AssetSaveData data)
        {
            _WriteChangeMessage(data, "ChangeAtlas");
        }



        public void WriteNewUIMessage(AssetSaveData data)
        {
            _WriteNewMessage(data, "NewUI");
        }

        public void WriteDeleteUIMessage(AssetSaveData data)
        {
            _WriteDeleteMessage(data, "DeleteUI");
        }


        public void WriteChangeUIMessage(AssetSaveData data)
        {
            _WriteChangeMessage(data, "ChangeUI");
        }


        private void _WriteChangeMessage(AssetSaveData data, string changeMessage)
        {
            if (fs != null)
            {
                sw.WriteLine("资源修改 -->> " + changeMessage +
                              " ++ 资源名 ：" + data.fileName +
                              " ++ 资源版本: " + data.version +
                              " ++ 资源路径 ：" + data.path +
                              " ++ 资源MD5 :" + data.md5 +
                              " ++ 合拼包名:" + data.package + "*" + data.packageNum + "*" + data.packagePos);
            }

            //if (_curState != EXCEL_STATE.WRITE) return;
            //if (oCon == null)
            //{
            //    _curState = EXCEL_STATE.NONE;
            //    return;
            //}

            //WriteOdbcCommand(toText("INSERT INTO [{1}$] VALUES ('','','{2}','{3}','{4}', '{5}','{6}','{7}')",
            //                        ODBC_SHEET, changeMessage, data.fileName,
            //                        data.version, data.fileTime,
            //                        data.path, data.md5), oCon);
        }


        private void _WriteNewMessage(AssetSaveData data, string newMessage)
        {
            if (fs != null)
            {
                sw.WriteLine("资源创建 -->> " + newMessage +
                              " ++ 资源名 ：" + data.fileName +
                              " ++ 资源版本: " + data.version +
                              " ++ 资源路径 ：" + data.path +
                              " ++ 资源MD5 :" + data.md5 +
                              " ++ 合拼包名:" + data.package + "*" + data.packageNum + "*" + data.packagePos);

            }

/*
            if (_curState != EXCEL_STATE.WRITE) return;
            if (oCon == null)
            {
                _curState = EXCEL_STATE.NONE;
                return;
            }

            WriteOdbcCommand(toText("INSERT INTO [{1}$] VALUES ('','','{2}','{3}','{4}', '{5}','{6}','{7}')",
                                  ODBC_SHEET, newMessage, data.fileName,
                                  data.version, data.versionTime,
                                  data.path, data.md5), oCon);
 */
        }

        private void _WriteDeleteMessage(AssetSaveData data, string deleteMessage)
        {

            if (fs != null)
            {
                sw.WriteLine("资源删除 -->> " + deleteMessage +
                              " ++ 资源名 ：" + data.fileName +
                              " ++ 资源版本: " + data.version +
                              " ++ 资源路径 ：" + data.path +
                              " ++ 资源MD5 :" + data.md5);
            }
            /*
            if (_curState != EXCEL_STATE.WRITE) return;
            if (oCon == null)
            {
                _curState = EXCEL_STATE.NONE;
                return;
            }

            WriteOdbcCommand(toText("INSERT INTO [{1}$] VALUES ('','','{2}','{3}','{4}', '','{5}','')",
                                  ODBC_SHEET, deleteMessage, data.fileName,
                                  data.version, data.path), oCon);

            */
        }

        /// <summary>
        /// End To Write  
        /// </summary>
        public void End()
        {

            if (fs != null)
            {
                sw.Close();
                fs.Close();

                sw = null;
                fs = null;
            }
        }

        private string toText(string text, params object[] additionalArgs)
        {
            if (additionalArgs.Length > 0)
            {
                for (int i = 0; i < additionalArgs.Length; i++)
                {
                    text = text.Replace("{" + (i + 1).ToString() + "}", additionalArgs[i].ToString());
                }
            }
            return text;
        }


/*
        private static void ReadXLS(string filetoread)
        {
            // Must be saved as excel 2003 workbook, not 2007, mono issue really
            string con = "Driver={Microsoft Excel Driver (*.xls)}; DriverId=790; Dbq=" + filetoread + ";";
            Debug.Log(con);
            string yourQuery = "SELECT * FROM [Sheet1$]";
            // our odbc connector 
            OdbcConnection oCon = new OdbcConnection(con);
            // our command object 
            OdbcCommand oCmd = new OdbcCommand(yourQuery, oCon);
            // table to hold the data 

            // open the connection 
            oCon.Open();
            // lets use a datareader to fill that table! 
            OdbcDataReader rData = oCmd.ExecuteReader();
            // now lets blast that into the table by sheer man power! 
            _dtData.Load(rData);
            // close that reader! 
            rData.Close();
            // close your connection to the spreadsheet! 
            oCon.Close();
            // wow look at us go now! we are on a roll!!!!! 
            // lets now see if our table has the spreadsheet data in it, shall we? 


            if (_dtData.Rows.Count > 0)
            {
                // do something with the data here 
                // but how do I do this you ask??? good question! 
                for (int i = 0; i < _dtData.Rows.Count; i++)
                {
                    // for giggles, lets see the column name then the data for that column! 
                    Debug.Log(" : " + _dtData.Rows[i][_dtData.Columns[1].ColumnName].ToString());
                }
            }
        }
*/


        //private static void SetExcel(string filetoread)
        //{
        //    string con = "Driver={Microsoft Excel Driver (*.xls)}; DriverId=790; Dbq=" + filetoread + ";ReadOnly=False;";
        //    Debug.Log(con);
        //    //		string yourQuery = "INSERT INTO [Sheet1$] VALUES ('AAA','AAA')"; 
        //    string yourQuery = "UPDATE [Sheet1$] SET AssetBundleName = 'Fred' WHERE id = 5";

        //    // our odbc connector 
        //    OdbcConnection oCon = new OdbcConnection(con);
        //    // our command object 

        //    // table to hold the data 

        //    // open the connection 
        //    oCon.Open();
        //    OdbcCommand oCmd = new OdbcCommand(yourQuery, oCon);
        //    int i = oCmd.ExecuteNonQuery();
        //    oCmd.Dispose();
        //    // lets use a datareader to fill that table! 
        //    //OdbcDataReader rData = oCmd.ExecuteReader(); 
        //    // now lets blast that into the table by sheer man power! 
        //    //dtYourData.Load(rData); 
        //    // close that reader! 
        //    //rData.Close(); 
        //    // close your connection to the spreadsheet! 
        //    oCon.Close();
        //    // wow look at us go now! we are on a roll!!!!! 
        //    // lets now see if our table has the spreadsheet data in it, shall we? 
        //}
}

	
}
