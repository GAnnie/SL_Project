// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  QiniuTest.cs
// Author   : willson
// Created  : 2014/11/18 
// Porpuse  : 
// **********************************************************************
using Qiniu.IO;
using UnityEngine;
using Qiniu.RS;
using Qiniu.Util;
using System;

public class QiniuTest
{
	protected static string Bucket = "kdtsgameprivate";
	protected static string NewKey
	{
		get { return Guid.NewGuid().ToString(); }
	}

	public QiniuTest()
	{

	}

	public void put()
	{
		IOClient target = new IOClient();
		string key = NewKey;
		Debug.Log(key);
		PutExtra extra = new PutExtra(); // TODO: 初始化为适当的值
		extra.MimeType = "text/plain"; // wav audio/x-wav
		extra.Crc32 = 123;
		extra.CheckCrc = CheckCrcType.CHECK;
		extra.Params = new System.Collections.Generic.Dictionary<string, string>();
		PutPolicy put = new PutPolicy(Bucket);
		
		target.PutFinished += new EventHandler<PutRet>((o, e) =>
		                                               {
			if (e.OK)
			{
				//RSHelper.RSDel(Bucket, key);
				Debug.Log("send: Hello, Qiniu Cloud!");
			}
		});
		
		string token = put.Token();
		Debug.Log(token);
		
		PutRet ret = target.Put(put.Token(), key, StreamEx.ToStream("Hello, Qiniu Cloud!"), extra);
	}
}