using UnityEngine;

public class AssetBase
{
    private UnityEngine.Object _asset = null;

    private int _reference = 0;

    public void AddReferenceIndex()     { _reference++; }
    public void RemoveReferenceIndex()  { _reference--; }
    public bool IsNoneReference()       { return (_reference <= 0 );} 

    public AssetBase(UnityEngine.Object obj)
    {
        _asset = obj;
    }

    public UnityEngine.Object asset { get { return _asset; } }
	
	public UnityEngine.GameObject assetGameObject { get { return _asset as GameObject; } }

    public UnityEngine.Object Instantiate()
    {
        if ( _asset == null )
        {
            return null;
        }

        return GameObject.Instantiate(_asset);
    }


    public void UnLoad()
    {
        _asset = null;
    }
}
