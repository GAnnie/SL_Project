using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class NGUIAtlasTool : EditorWindow 
{
	private static NGUIAtlasTool win = null;
	
	
	[MenuItem("OptimizationTool/NGUI Atlas Tool")]
	public static void ShowWin()
	{
		if( win == null )
		{
			win = (NGUIAtlasTool)EditorWindow.GetWindow( typeof( NGUIAtlasTool ));
		}
		
		if( win != null )
		{
			win.Show();
			win.minSize = new Vector2(600, 500);
			win.maxSize = new Vector2(1000, 500);
		}
	}


	private Vector2 pos;
	private NGUIAtlasSet _nguiAtlasSet = null;
	void OnGUI()
	{
		if( _nguiAtlasSet == null )
		{
			InitNguiAtlasSet();
		}
		DropAreaGUI();

		GUI.color = Color.green;
		if( GUILayout.Button( "Update Data ", GUILayout.Height( 20 )))
		{
			UpdateMessage();
		}		
		GUI.color = Color.white;
		
		if( _nguiAtlasSet != null )
		{
			GameEditorUtils.ShowList< string, string >( _nguiAtlasSet.nguiAtlasSet,ShowAtlas, ref pos, (int)position.width, 250 );
		}
		
		DelAtlas();
		
		
		GameEditorUtils.Space( 2 );
		
//		GUI.color = Color.green;
//		if( GUILayout.Button( "Save Atlas Data ", GUILayout.Height( 40 )))
//		{
//			SaveNguiAtlasSet();
//		}
		
		GUI.color = Color.yellow;
		if( GUILayout.Button( "Change Atlas Format ", GUILayout.Height( 40 )))
		{
			if( EditorUtility.DisplayDialog( " Tips ", " Would you want to chang NGUI Atlas' importor ??" , "OK", "CANCEL"))
			{
				ChangeNguiAtlas();
			}
		}
		
		GUI.color = Color.white;
	}
	
	private void UpdateMessage()
	{
		
		if( showDataBaseMessage != null )
		{
			showDataBaseMessage.Clear();
		}
	}
	
	
	#region NGUIAtlasTmpMessage	
	private class NGUIAtlasTmpMessage
	{
		public string atlasName = string.Empty;
		
		public string atlasPath = string.Empty;
		
		public UIAtlas uiAtlas = null;
			
		public bool show = false;
		
		public TextureImporter importer;
		
		public bool isSuccess = false;
		
		public bool isSquare = false;
		
		public NGUIAtlasTmpMessage( string atlasName , string atlasPath )
		{
			
			GameObject prefab = AssetDatabase.LoadMainAssetAtPath( atlasPath ) as GameObject;
			uiAtlas = prefab.GetComponent< UIAtlas >();
			if( uiAtlas != null )
			{
				Texture texture = uiAtlas.texture;
				if( texture != null )
				{
			 		this.importer = (TextureImporter)TextureImporter.GetAtPath( AssetDatabase.GetAssetPath( texture ));
				}
			}
			
			isSuccess = (this.importer != null );
			
			if( isSuccess )
			{
				this.atlasName = atlasName;
				
				this.atlasPath = atlasPath;
				
				int originalWith = 0;
				int originalHeight = 0;
				
				GetTexutreOriginalSize(importer, ref originalWith, ref originalHeight);
				isSquare =  ( originalWith == originalHeight );
				
			}
		}
		
		
		/// <summary>
		/// Gets the size of the texutre original.
		/// </summary>
		/// <param name='imp'>
		/// Imp.
		/// </param>
		/// <param name='wight'>
		/// Wight.
		/// </param>
		/// <param name='height'>
		/// Height.
		/// </param>
		private void GetTexutreOriginalSize( TextureImporter importer , ref int wight , ref int height )
		{
			//============================= GET Original size of Textur ======================================== 
			
			
			MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance );
			
			object[] args = new object[2] { 0, 0 };
		
			mi.Invoke(importer, args);
			
	        wight = (int)args[0];
	        height = (int)args[1];
			//==================================================================================================
		}		
			
	}
	#endregion
	
	
	private Dictionary< string, NGUIAtlasTmpMessage> showDataBaseMessage = new Dictionary<string, NGUIAtlasTmpMessage>();
	private List< string > delAtlasList = new List<string>();
	void ShowAtlas(string atlasName,  string atlasPath )
	{
		
		if( !showDataBaseMessage.ContainsKey( atlasName ))
		{
			NGUIAtlasTmpMessage tmpMessage = new NGUIAtlasTmpMessage( atlasName, atlasPath );
			
			if( tmpMessage.isSuccess )
			{
				showDataBaseMessage.Add( atlasName, tmpMessage );
			}
			else
			{
				delAtlasList.Add( atlasName );
				return;
			}
		}
		
		
		EditorGUILayout.BeginVertical();
		{
			EditorGUILayout.BeginHorizontal();
			{
				if( !showDataBaseMessage[atlasName].isSquare )
				{
					GUI.color = Color.red ;	
				}
				else
				{
					GUI.color = Color.white;
				}
				
				showDataBaseMessage[atlasName].show = EditorGUILayout.Foldout( showDataBaseMessage[atlasName].show,atlasName );
				
				GUI.color = Color.green;
				if( GUILayout.Button( "SEL", GUILayout.Width( 50 ), GUILayout.Height( 20 )))
				{
					UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath( atlasPath );
					EditorGUIUtility.PingObject( obj );
				}
				
				GUI.color = Color.red ;
				if( GUILayout.Button( "DEL", GUILayout.Width( 50 ), GUILayout.Height( 20 )))
				{
					if( _nguiAtlasSet.nguiAtlasSet.ContainsKey( atlasName ))
					{
						delAtlasList.Add( atlasName );
					}
				}
				GUI.color = Color.white;
			}
			EditorGUILayout.EndHorizontal();
			
			if( showDataBaseMessage[atlasName].show ) 
			{
				ShowBaseAtlas( 	showDataBaseMessage[atlasName] );
			}			
		}
		EditorGUILayout.EndVertical();
		

	}
	
	
	void ShowBaseAtlas(NGUIAtlasTmpMessage msg )
	{
		GUI.color = Color.green;
		EditorGUILayout.LabelField( "Atlas Name : ", msg.atlasName );
		EditorGUILayout.LabelField( "Atlas Path : ", msg.atlasPath );
		
		if( !msg.isSquare )
		{
			GUI.color = Color.red;	
		}
		EditorGUILayout.LabelField( "Is Square : ", msg.isSquare ? "true":"false" );
		GUI.color = Color.white;
		
		GUI.color = Color.white;
	}
	
	
	
	
	void DelAtlas()
	{
		if( delAtlasList.Count > 0 )
		{
			foreach( string delAtlasName in delAtlasList )
			{
				if( _nguiAtlasSet.nguiAtlasSet.ContainsKey( delAtlasName ))
				{
					_nguiAtlasSet.nguiAtlasSet.Remove( delAtlasName );
				}				
			}
			delAtlasList.Clear();
		}
	}
	
	
	
	
	private void DropAreaGUI()
	{
		
		/*
		Event evt = Event.current;
		Rect dropArea = GUILayoutUtility.GetRect( 0f,  50f );
		GUI.Box(dropArea, "Drop a NGUI Atlas here");
		
		
		switch( evt.type )
		{
			case EventType.DragUpdated:
			case EventType.DragPerform:
				if (!dropArea.Contains(evt.mousePosition))
					break;
				
				DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
				
				if (evt.type == EventType.DragPerform)
				{
					DragAndDrop.AcceptDrag();
					foreach(UnityEngine.Object draggedObject in DragAndDrop.objectReferences)
					{
						GameObject go = draggedObject as GameObject;
						UIAtlas uiAtlas = go.GetComponent< UIAtlas >();
					
						if (uiAtlas == null) 
						{
							ShowNotification( new GUIContent( "Please add <	UIAtlas Prefab > in here "));
							continue;
						}
						
						if( !_nguiAtlasSet.nguiAtlasSet.ContainsKey( go.name ) )
						{
							string path = AssetDatabase.GetAssetPath( go );
							_nguiAtlasSet.nguiAtlasSet.Add( go.name, path );
						}
						else
						{
							ShowNotification( new GUIContent( string.Format("Atlas : < {0} > is exist!!!", go.name)));
							continue;
						}
					}
				}
				
				Event.current.Use();
				break;			
		}
		
		*/
		
	}
	
	
	private string uiAtlasPath     = "UI/Altas_V3";
	private string uiAtlasDataPath = "Editor/UI/Data/";
	private string uiAtlasDataName = "uiAtlasData.txt";
	void InitNguiAtlasSet()
	{
//		Dictionary< string, object > obj =  GameEditorUtils.GetJsonFile( Application.dataPath + "/" + uiAtlasDataPath + uiAtlasDataName, true );
//		
//		if( obj != null )
//		{
//			JsonNGUIAtlasSetParser parser = new JsonNGUIAtlasSetParser();
//			_nguiAtlasSet = parser.DeserializeJson_NGUIAtlasSet( obj );
//			
//			if( _nguiAtlasSet.nguiAtlasSet == null )
//			{
//				_nguiAtlasSet.nguiAtlasSet = new Dictionary<string, string>();
//			}
//		}
//		else
//		{
//			_nguiAtlasSet = new NGUIAtlasSet(true);
//		}
		
		_nguiAtlasSet = new NGUIAtlasSet(true);
		
		List< string > prefabList =  GameEditorUtils.GetAssetsList( Application.dataPath + "/"  + uiAtlasPath, "prefab");
		
		foreach( string path in prefabList )
		{
			GameObject prefab = AssetDatabase.LoadMainAssetAtPath( path ) as GameObject;
			if( prefab != null )
			{
				UIAtlas uiAtlas = prefab.GetComponent< UIAtlas >();
				if( uiAtlas != null )
				{
					_nguiAtlasSet.nguiAtlasSet.Add( prefab.name, path );
				}
			}
		}
		
	}
//	
//	void SaveNguiAtlasSet()
//	{
//		if( _nguiAtlasSet != null )
//		{
//			JsonNGUIAtlasSetParser parser = new JsonNGUIAtlasSetParser();
//			Dictionary< string, object > obj = parser.SerializeJson_NGUIAtlasSet( _nguiAtlasSet );
//			
//			GameEditorUtils.SaveJsonFile( obj,
//										  Application.dataPath + "/" + uiAtlasDataPath + uiAtlasDataName,
//										  true );
//										  
//		}
//	}
	
	
	
	void ChangeNguiAtlas()
	{
		int changNumber = 0;
		int totalNumber = 0;
		foreach( KeyValuePair< string ,string > item in _nguiAtlasSet.nguiAtlasSet )
		{
			totalNumber ++;
		}
		
		
		foreach( KeyValuePair< string ,string > item in _nguiAtlasSet.nguiAtlasSet )
		{
			changNumber++;
				
			if( showDataBaseMessage.ContainsKey( item.Key ))
			{
				NGUIAtlasTmpMessage msg = showDataBaseMessage[item.Key];
				Debug.Log( "Atlas : " + item.Key );
				
#if UNITY_ANDROID
				msg.importer.textureType =  TextureImporterType.GUI;
            	msg.importer.SetPlatformTextureSettings("Android", 2048, TextureImporterFormat.AutomaticCompressed);				
#elif UNITY_IPHONE
				// If atlas is square
				if(!msg.isSquare )
				{
					
					msg.importer.textureType =  TextureImporterType.GUI;
					msg.importer.SetPlatformTextureSettings("iPhone",  2048, TextureImporterFormat.AutomaticTruecolor );					
					AssetDatabase.ImportAsset(item.Value,ImportAssetOptions.ForceUpdate );
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
					
					if( msg.uiAtlas  != null)
					{
						NGUISettings.atlas = msg.uiAtlas;
						
						NGUISettings.unityPacking = false;
						
						NGUISettings.forceSquareAtlas = true;
						
						List<UIAtlasMaker.SpriteEntry> sprites = new List<UIAtlasMaker.SpriteEntry>();
						
						UIAtlasMaker.ExtractSprites( NGUISettings.atlas, sprites );
						
						UIAtlasMaker.UpdateAtlas(  NGUISettings.atlas, sprites);
						
					}
					else
					{
						Debug.LogError( string.Format("Texture Atlas :{0} , can not found the atlas prefab : {1}",
														item.Key,
														item.Value));
					}
					
				}
				
				
				msg.importer.textureType =  TextureImporterType.GUI;
				msg.importer.SetPlatformTextureSettings("iPhone",  2048, TextureImporterFormat.AutomaticCompressed );
#endif
				AssetDatabase.ImportAsset(item.Value,ImportAssetOptions.ForceUpdate );
			}
			
			EditorUtility.DisplayProgressBar( "Tips", 
											  string.Format("Changing .... {0}/{1}", changNumber,totalNumber ),
											  (float)changNumber / totalNumber);
														
														
			
		}
		
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		EditorUtility.ClearProgressBar();
							
							
		UpdateMessage();					
	}
	
	
}



public class NGUIAtlasSet
{
	public Dictionary< string, string > nguiAtlasSet = null;
	
	public NGUIAtlasSet()
	{}
	
	public NGUIAtlasSet( bool newSet)
	{
		nguiAtlasSet = new Dictionary<string, string>();
	}
		
}


