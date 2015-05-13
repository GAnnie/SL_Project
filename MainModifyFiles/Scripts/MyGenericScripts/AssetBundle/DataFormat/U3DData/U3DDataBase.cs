using System;
using UnityEngine;

public class U3DDataBase
{
    // Accumulative number , if this number large than MAX number , then unload this bundle
    private   int accNumber  = 0;
    protected int maxLimit = 1;

    public void AddAccNumber() { accNumber++;}
    public void Reset()        { accNumber = 0; }
    public virtual bool IsOvertake()   { return accNumber >= maxLimit;  }
    public virtual int AssetSize() { return 0;}

    protected U3DDataBase()
    {
    }

    virtual public void Unload()
    {
    }
}
