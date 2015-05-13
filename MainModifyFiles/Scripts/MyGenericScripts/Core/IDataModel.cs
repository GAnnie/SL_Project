using UnityEngine;
using System.Collections;

public interface IDataModel{
	
	/// <summary>
	/// Requests the data.
	/// 数据可能来自服务器、文件、或者其它DataModel
	/// </summary>
 	void RequestData();

	/// <summary>
	/// Setup this instance.
	/// 一般情况下，Setup方法在RequestData数据返回之后调用
	/// </summary>
 	void Setup(object data);
	
	/// <summary>
	/// Releases all resource.
	/// 释放数据模型的所有数据，及其相关引用
	/// </summary>
 	void Dispose();
}
