using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectDisplayManager  
{
	private static GameObjectDisplayManager _inst = null;

	public static GameObjectDisplayManager Instance
	{
		get
		{
			if( _inst == null )
			{
				_inst = new GameObjectDisplayManager();
			}

			return _inst;
		}
	}

	private GameObjectDisplayManager()
	{
		_curShowCheckerNumberDict = new Dictionary<string, int>();
		_showCheckerDict = new Dictionary<string, List<ShowChecker>>();

	}

	//不限制显示类型
	public const string CHECKERTYPE_NOLIMIT = "nolimit";

	//当前显示的数量
	private int MAX_SHOW_NUMBER = 300;
	public void Setup(bool lowMemory)
	{
		if (lowMemory)
		{
			MAX_SHOW_NUMBER = 300;
		}
		else
		{
			MAX_SHOW_NUMBER = 500;
		}
	}

	//最大的数量
	private Dictionary<string, int > _curShowCheckerNumberDict = null;

	//如果当前相识的数量达到上限，则加进这个显示列表里面， 等待显示
	private Dictionary< string, List< ShowChecker > > _showCheckerDict = null;

	/// <summary>
	/// 加入显示列表 
	/// </summary>
	/// <param name="checker">Checker.</param>
	public void AddShowCommandItem( ShowChecker checker, string checkerType )
	{
		if( !_curShowCheckerNumberDict.ContainsKey ( checkerType ))
		{
			_curShowCheckerNumberDict.Add( checkerType, 0 );
			_showCheckerDict.Add( checkerType, new List<ShowChecker>());
		}

		if( _curShowCheckerNumberDict[checkerType] < MAX_SHOW_NUMBER || checkerType == CHECKERTYPE_NOLIMIT)
		{
			checker.ShowObj();
			_curShowCheckerNumberDict[checkerType] ++;
		}
		else
		{
			if( !_showCheckerDict[checkerType].Contains( checker ))
			{
				_showCheckerDict[checkerType].Add( checker );
			}
		}
	}

	/// <summary>
	/// 删除显示列表
	/// </summary>
	/// <param name="checker">Checker.</param>
	public bool DeleteShowCommandItem( ShowChecker checker, string checkerType )
	{
		if( !_curShowCheckerNumberDict.ContainsKey ( checkerType ) ||
		   !_showCheckerDict.ContainsKey(checkerType))
		{
			Debug.Log( "Can not finde Checker Typer of : " + checkerType );
			return false;
		}


		if( checker.isShowObj )
		{
			_curShowCheckerNumberDict[checkerType]--;

			//显示显示列表的第一个元素
			if( _showCheckerDict[checkerType].Count > 0)
			{
				ShowChecker preChecker = _showCheckerDict[checkerType][0];
				_showCheckerDict[checkerType].RemoveAt(0);

				AddShowCommandItem( preChecker, checkerType);
			}
		}
		else
		{
			if( _showCheckerDict[checkerType].Contains( checker ))
			{
				_showCheckerDict[checkerType].Remove( checker );
			}
		}

		checker.HideObj();

		return true;
	}

	public bool DeleteFromList( ShowChecker checker , string checkerType )
	{
		if( !_curShowCheckerNumberDict.ContainsKey ( checkerType ) ||
		   !_showCheckerDict.ContainsKey(checkerType))
		{
			Debug.Log( "Can not finde Checker Typer of : " + checkerType );
			return false;
		}
		
		if( checker.isShowObj )
		{
			_curShowCheckerNumberDict[checkerType]--;
		}
		else
		{
			if( _showCheckerDict[checkerType].Contains( checker ))
			{
				_showCheckerDict[checkerType].Remove( checker );
			}
		}
		return true;
	}


	private void CleanAll()
	{	
		_curShowCheckerNumberDict.Clear();
		_showCheckerDict.Clear();
	}
}
