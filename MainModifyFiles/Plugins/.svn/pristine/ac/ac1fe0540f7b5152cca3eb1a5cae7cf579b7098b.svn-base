// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ZipLibUtils.cs
// Author   : wenlin
// Created  : 2013/2/21 
// Purpose  : 
// **********************************************************************
using System;
using UnityEngine;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip.Compression;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

public class ZipLibUtils
{
    public static bool CompressFile(string source, string desc)
    {
        //return true;
        Func<byte[], byte[]> func = ZipLibUtils.Compress;
        return _OperateFile(source, desc, func); 
    }


    public static bool UnCompressFile(string source, string desc)
    {
        //return true;
        Func<byte[], byte[]> func = ZipLibUtils.Uncompress;
        return _OperateFile(source, desc, func);
    }
 

    private static bool _OperateFile( string source, string desc, Func< byte[], byte[] > operation )
    {
        FileStream sourceFs = null;
        try
        {
            sourceFs = new FileStream(source, FileMode.Open);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }

        if (sourceFs != null)
        {
            byte[] bytes = new byte[ sourceFs.Length ];
            sourceFs.Read(bytes, 0, bytes.Length);
            sourceFs.Close();

            bytes = operation(bytes);
            try
            {
                FileStream descFs = new FileStream(desc, FileMode.OpenOrCreate);
                descFs.Write(bytes, 0, bytes.Length);
                descFs.Close();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        return true;
    }


   public static byte[] Compress(byte[] input)  
    {  
        // Create the compressor with highest level of compression  
        Deflater compressor = new Deflater();  
        compressor.SetLevel(Deflater.BEST_COMPRESSION);  

        // Give the compressor the data to compress  
        compressor.SetInput(input);  
        compressor.Finish();  

        /* 
         * Create an expandable byte array to hold the compressed data. 
         * You cannot use an array that's the same size as the orginal because 
         * there is no guarantee that the compressed data will be smaller than 
         * the uncompressed data. 
         */  
        MemoryStream bos = new MemoryStream(input.Length);  

        // Compress the data  
        byte[] buf = new byte[1024];  
        while (!compressor.IsFinished)  
        {  
            int count = compressor.Deflate(buf);  
            bos.Write(buf, 0, count);  
        }  

        // Get the compressed data  
        return bos.ToArray();  
    }  
  
    public static byte[] Uncompress(byte[] input)  
    {  
        Inflater decompressor = new Inflater();  
        decompressor.SetInput(input);  

        // Create an expandable byte array to hold the decompressed data  
        MemoryStream bos = new MemoryStream(input.Length);  

        // Decompress the data  
        byte[] buf = new byte[1024];  
        while (!decompressor.IsFinished)  
        {  
            int count = decompressor.Inflate(buf);  
            bos.Write(buf, 0, count);  
        }    

        // Get the decompressed data  
        return bos.ToArray();  
    }  
    
	
	
	
    /// <summary>
    /// 压缩单个文件
    /// </summary>
    /// <param name="fileToZip">要压缩的文件</param>
    /// <param name="zipedFile">压缩后的文件</param>
    /// <param name="compressionLevel">压缩等级</param>
    /// <param name="blockSize">每次写入大小</param>
    public static void ZipFile(string fileToZip, string zipedFile, int compressionLevel, int blockSize)
    {
        //如果文件没有找到，则报错
        if (!System.IO.File.Exists(fileToZip))
        {
            throw new System.IO.FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
        }
 
        using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
        {
            using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
            {
                using (System.IO.FileStream StreamToZip = new System.IO.FileStream(fileToZip, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);
 
                    ZipEntry ZipEntry = new ZipEntry(fileName);
 
                    ZipStream.PutNextEntry(ZipEntry);
 
                    ZipStream.SetLevel(compressionLevel);
 
                    byte[] buffer = new byte[blockSize];
 
                    int sizeRead = 0;
 
                    try
                    {
                        do
                        {
                            sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                            ZipStream.Write(buffer, 0, sizeRead);
                        }
                        while (sizeRead > 0);
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
 
                    StreamToZip.Close();
                }
 
                ZipStream.Finish();
                ZipStream.Close();
            }
 
            ZipFile.Close();
        }
    }
 
    /// <summary>
    /// 压缩单个文件
    /// </summary>
    /// <param name="fileToZip">要进行压缩的文件名</param>
    /// <param name="zipedFile">压缩后生成的压缩文件名</param>
    public static void ZipFile(string fileToZip, string zipedFile)
    {
        //如果文件没有找到，则报错
        if (!File.Exists(fileToZip))
        {
            throw new System.IO.FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
        }
 
        using (FileStream fs = File.OpenRead(fileToZip))
        {
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
 
            using (FileStream ZipFile = File.Create(zipedFile))
            {
                using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
                {
                    string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);
                    ZipEntry ZipEntry = new ZipEntry(fileName);
                    ZipStream.PutNextEntry(ZipEntry);
                    ZipStream.SetLevel(5);
 
                    ZipStream.Write(buffer, 0, buffer.Length);
                    ZipStream.Finish();
                    ZipStream.Close();
                }
            }
        }
    }
 
    /// <summary>
    /// 压缩多层目录
    /// </summary>
    /// <param name="strDirectory">The directory.</param>
    /// <param name="zipedFile">The ziped file.</param>
    public static void ZipFileDirectory(string strDirectory, string zipedFile)
    {
        using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
        {
            using (ZipOutputStream s = new ZipOutputStream(ZipFile))
            {
                ZipSetp(strDirectory, s, "");
            }
        }
    }
 
    /// <summary>
    /// 递归遍历目录
    /// </summary>
    /// <param name="strDirectory">The directory.</param>
    /// <param name="s">The ZipOutputStream Object.</param>
    /// <param name="parentPath">The parent path.</param>
    private static void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath)
    {
        if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
        {
            strDirectory += Path.DirectorySeparatorChar;
        }          
        Crc32 crc = new Crc32();
 
        string[] filenames = Directory.GetFileSystemEntries(strDirectory);
 
        foreach (string file in filenames)// 遍历所有的文件和目录
        {
 
            if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
            {
//                string pPath = parentPath;
//                pPath += file.Substring(file.LastIndexOf("\\") + 1);
//                pPath += "\\";
//                ZipSetp(file, s, pPath);
            }
 
            else // 否则直接压缩文件
            {
                //打开压缩文件
                using (FileStream fs = File.OpenRead(file))
                {
 
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
 
                    string fileName = parentPath + file.Substring(file.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(fileName);
 
                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
 
                    fs.Close();
 
                    crc.Reset();
                    crc.Update(buffer);
 
                    entry.Crc = crc.Value;
                    s.PutNextEntry(entry);
 
                    s.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
 
    /// <summary>
    /// 解压缩一个 zip 文件。
    /// </summary>
    /// <param name="zipedFile">The ziped file.</param>
    /// <param name="strDirectory">The STR directory.</param>
    /// <param name="password">zip 文件的密码。</param>
    /// <param name="overWrite">是否覆盖已存在的文件。</param>
    public static void UnZip(string zipedFile, string strDirectory, string password, bool overWrite)
    {
        if (strDirectory == string.Empty)
		{
			return;
		}
		
        if (!strDirectory.EndsWith("/"))
            strDirectory = strDirectory + "/";
 
        using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipedFile)))
        {
            s.Password = password;
            ZipEntry theEntry;
 
            while ((theEntry = s.GetNextEntry()) != null)
            {
                string directoryName = "";
                string pathToZip = "";
                pathToZip = theEntry.Name;
 
                if (pathToZip != "")
                    directoryName = Path.GetDirectoryName(pathToZip) + "\\";
 
                string fileName = Path.GetFileName(pathToZip);
 
                Directory.CreateDirectory(strDirectory + directoryName);
 
                if (fileName != "")
                {
                    if ((File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!File.Exists(strDirectory + directoryName + fileName)))
                    {
                        using (FileStream streamWriter = File.Create(strDirectory + directoryName + fileName))
                        {
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
 
                                if (size > 0)
                                    streamWriter.Write(data, 0, size);
                                else
                                    break;
                            }
                            streamWriter.Close();
                        }
                    }
                }
            }
 
            s.Close();
        }
        
    }
	
    /// <summary>
    /// 解压缩一个 zip 文件。
    /// </summary>
    /// <param name="zipByte">压缩文件的二进制数据.</param>
    /// <param name="strDirectory">The STR directory.</param>
    /// <param name="password">zip 文件的密码。</param>
    /// <param name="overWrite">是否覆盖已存在的文件。</param>	
    
	
	public static Dictionary< string, MemoryStream > UnZipFromByte( byte[] zipByte, string strDirectory, string password )
	{
//        if (strDirectory == string.Empty)
//		{
//			return;
//		}
//		
//        if (!strDirectory.EndsWith("/"))
//            strDirectory = strDirectory + "/";
 
		Dictionary< string, MemoryStream > bytesDictionary = null;
		
		MemoryStream mms = new MemoryStream(zipByte);
        using (ZipInputStream s = new ZipInputStream(mms))
        {
            s.Password = password;
            ZipEntry theEntry;
 
			bytesDictionary = new Dictionary<string, MemoryStream>();
            while ((theEntry = s.GetNextEntry()) != null)
            {
                string directoryName = "";
                string pathToZip = "";
                pathToZip = theEntry.Name;
 
                if (pathToZip != "")
                    directoryName = Path.GetDirectoryName(pathToZip) + "\\";
 
                string fileName = Path.GetFileName(pathToZip);
 
//                Directory.CreateDirectory(strDirectory + directoryName);
 
                if (fileName != "")
                {
					if( !bytesDictionary.ContainsKey( strDirectory + fileName ))
					{
						MemoryStream streamWriter = new MemoryStream();
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);

                            if (size > 0)
                                streamWriter.Write(data, 0, size);
                            else
                                break;
                        }
						
						bytesDictionary.Add( strDirectory + fileName , streamWriter );
					}
					
					/*
                    if ((File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!File.Exists(strDirectory + directoryName + fileName)))
                    {
                        using (FileStream streamWriter = File.Create(strDirectory + directoryName + fileName))
                        {
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
 
                                if (size > 0)
                                    streamWriter.Write(data, 0, size);
                                else
                                    break;
                            }
                            streamWriter.Close();
                        }
                    }
                    */
                }
            }
 
            s.Close();
        }
		
		return bytesDictionary;
	}
}

