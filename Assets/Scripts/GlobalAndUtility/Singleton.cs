using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T instance;
    public static T Instance { get => instance; }

    protected virtual void Awake()
    {
        instance = this as T;
        var insts = FindObjectsOfType<T>();
        if (insts.Length > 1)
        {
            foreach(var inst in insts)
            {
                if(inst != instance)
                {
                    Destroy(inst);
                }
            }
        }
    }
}
