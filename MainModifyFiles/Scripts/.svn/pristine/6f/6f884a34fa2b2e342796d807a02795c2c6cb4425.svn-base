// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  LZMA_Util.cs
// Author   : wenlin
// Created  : 2013/5/2 11:23:15
// Purpose  : 
// **********************************************************************

using System;
using System.Collections.Generic;
using System.IO;

using SevenZip;
using SevenZip.Compression.LZMA;

public class LZMA_Util
{
    public static void CompressFileLZMA(string inFile, string outFile)
    {
        Int32 dictionary = 1 << 23;
        Int32 posStateBits = 2;
        Int32 litContextBits = 3; // for normal files
        // UInt32 litContextBits = 0; // for 32-bit data
        Int32 litPosBits = 0;
        // UInt32 litPosBits = 2; // for 32-bit data
        Int32 algorithm = 2;
        Int32 numFastBytes = 128;

        string mf = "bt4";
        bool eos = true;
        bool stdInMode = false;

        CoderPropID[] propIDs =  {
                CoderPropID.DictionarySize,
                CoderPropID.PosStateBits,
                CoderPropID.LitContextBits,
                CoderPropID.LitPosBits,
                CoderPropID.Algorithm,
                CoderPropID.NumFastBytes,
                CoderPropID.MatchFinder,
                CoderPropID.EndMarker
            };

        object[] properties = {
                (Int32)(dictionary),
                (Int32)(posStateBits),
                (Int32)(litContextBits),
                (Int32)(litPosBits),
                (Int32)(algorithm),
                (Int32)(numFastBytes),
                mf,
                eos
            };

        using (FileStream inStream = new FileStream(inFile, FileMode.Open))
        {
            using (FileStream outStream = new FileStream(outFile, FileMode.Create))
            {
                SevenZip.Compression.LZMA.Encoder encoder = new SevenZip.Compression.LZMA.Encoder();
                encoder.SetCoderProperties(propIDs, properties);
                encoder.WriteCoderProperties(outStream);
                Int64 fileSize;
                if (eos || stdInMode)
                    fileSize = -1;
                else
                    fileSize = inStream.Length;
                for (int i = 0; i < 8; i++)
                    outStream.WriteByte((Byte)(fileSize >> (8 * i)));
                encoder.Code(inStream, outStream, -1, -1, null);
            }
        }

    }

    public static void CompressFileLZMA(byte[] bytes, string outFile)
    {
        MemoryStream input = new MemoryStream(bytes);
        CompressFileLZMA(input, outFile);
    }

    public static void CompressFileLZMA(byte[] bytes, Stream outpuStream)
    {
        MemoryStream input = new MemoryStream(bytes);
        CompressFileLZMA(input, outpuStream);
    }

    public static void CompressFileLZMA( Stream inStream, string outFile)
    {
        Int32 dictionary = 1 << 23;
        Int32 posStateBits = 2;
        Int32 litContextBits = 3; // for normal files
        // UInt32 litContextBits = 0; // for 32-bit data
        Int32 litPosBits = 0;
        // UInt32 litPosBits = 2; // for 32-bit data
        Int32 algorithm = 2;
        Int32 numFastBytes = 128;

        string mf = "bt4";
        bool eos = true;
        bool stdInMode = false;

        CoderPropID[] propIDs =  {
                CoderPropID.DictionarySize,
                CoderPropID.PosStateBits,
                CoderPropID.LitContextBits,
                CoderPropID.LitPosBits,
                CoderPropID.Algorithm,
                CoderPropID.NumFastBytes,
                CoderPropID.MatchFinder,
                CoderPropID.EndMarker
            };

        object[] properties = {
                (Int32)(dictionary),
                (Int32)(posStateBits),
                (Int32)(litContextBits),
                (Int32)(litPosBits),
                (Int32)(algorithm),
                (Int32)(numFastBytes),
                mf,
                eos
            };

        using (FileStream outStream = new FileStream(outFile, FileMode.Create))
        {
            SevenZip.Compression.LZMA.Encoder encoder = new SevenZip.Compression.LZMA.Encoder();
            encoder.SetCoderProperties(propIDs, properties);
            encoder.WriteCoderProperties(outStream);
            Int64 fileSize;
            if (eos || stdInMode)
                fileSize = -1;
            else
                fileSize = inStream.Length;
            for (int i = 0; i < 8; i++)
                outStream.WriteByte((Byte)(fileSize >> (8 * i)));
            encoder.Code(inStream, outStream, -1, -1, null);
        }
    }


    public static void CompressFileLZMA(Stream inStream, Stream outStream)
    {
        Int32 dictionary = 1 << 23;
        Int32 posStateBits = 2;
        Int32 litContextBits = 3; // for normal files
        // UInt32 litContextBits = 0; // for 32-bit data
        Int32 litPosBits = 0;
        // UInt32 litPosBits = 2; // for 32-bit data
        Int32 algorithm = 2;
        Int32 numFastBytes = 128;

        string mf = "bt4";
        bool eos = true;
        bool stdInMode = false;

        CoderPropID[] propIDs =  {
                CoderPropID.DictionarySize,
                CoderPropID.PosStateBits,
                CoderPropID.LitContextBits,
                CoderPropID.LitPosBits,
                CoderPropID.Algorithm,
                CoderPropID.NumFastBytes,
                CoderPropID.MatchFinder,
                CoderPropID.EndMarker
            };

        object[] properties = {
                (Int32)(dictionary),
                (Int32)(posStateBits),
                (Int32)(litContextBits),
                (Int32)(litPosBits),
                (Int32)(algorithm),
                (Int32)(numFastBytes),
                mf,
                eos
            };


        SevenZip.Compression.LZMA.Encoder encoder = new SevenZip.Compression.LZMA.Encoder();
        encoder.SetCoderProperties(propIDs, properties);
        encoder.WriteCoderProperties(outStream);
        Int64 fileSize;
        if (eos || stdInMode)
            fileSize = -1;
        else
            fileSize = inStream.Length;
        for (int i = 0; i < 8; i++)
            outStream.WriteByte((Byte)(fileSize >> (8 * i)));
        encoder.Code(inStream, outStream, -1, -1, null);
        
    }

    public static void DecompressFileLZMA(string inFile, string outFile)
    {
        using (FileStream input = new FileStream(inFile, FileMode.Open))
        {
            using (FileStream output = new FileStream(outFile, FileMode.Create))
            {
                SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();

                byte[] properties = new byte[5];
                if (input.Read(properties, 0, 5) != 5)
                    throw (new Exception("input .lzma is too short"));
                decoder.SetDecoderProperties(properties);

                long outSize = 0;
                for (int i = 0; i < 8; i++)
                {
                    int v = input.ReadByte();
                    if (v < 0)
                        throw (new Exception("Can't Read 1"));
                    outSize |= ((long)(byte)v) << (8 * i);
                }
                long compressedSize = input.Length - input.Position;

                decoder.Code(input, output, compressedSize, outSize, null);
            }
        }
    }

    public static void DecompressFileLZMA(byte[] bytes, string outFile, bool isOutputClose = true )
    {
        MemoryStream input = new MemoryStream(bytes);
        using (FileStream output = new FileStream(outFile, FileMode.Create))
        {
            DecompressFileLZMA(input, output, true, isOutputClose );
        }
    }

    public static void DecompressFileLZMA(byte[] bytes, Stream output, bool isInputClose = true, bool isOutputClose = true)
    {
        MemoryStream input = new MemoryStream(bytes);
        DecompressFileLZMA(input, output, isInputClose, isOutputClose);
    }

    public static void DecompressFileLZMA(Stream input, Stream output, bool isInputClose = true, bool isOutputClose = true)
    {
        //using (FileStream output = new FileStream(outFile, FileMode.Create))
        //{
            SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();

            byte[] properties = new byte[5];
            if (input.Read(properties, 0, 5) != 5)
                throw (new Exception("input .lzma is too short"));
            decoder.SetDecoderProperties(properties);

            long outSize = 0;
            for (int i = 0; i < 8; i++)
            {
                int v = input.ReadByte();
                if (v < 0)
                    throw (new Exception("Can't Read 1"));
                outSize |= ((long)(byte)v) << (8 * i);
            }
            long compressedSize = input.Length - input.Position;

            decoder.Code(input, output, compressedSize, outSize, null);

            if ( isInputClose ) input.Close();
            if ( isOutputClose ) output.Close();
        //}
    }
}
