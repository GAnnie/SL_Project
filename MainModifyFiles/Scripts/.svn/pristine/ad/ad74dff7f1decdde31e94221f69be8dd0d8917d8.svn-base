using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ReferencesController<T> : IReferencesCtl where T : new()
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }

            return _instance;
        }
    }

    protected ReferencesController()
    { }

    public virtual void SetupReference(List<int> instanceID) { }

    public virtual void DeleteReference(List<int> instanceID) { }
}

public interface IReferencesCtl
{
    void SetupReference(List< int > instanceID);

    void DeleteReference(List<int> instanceID);
}
