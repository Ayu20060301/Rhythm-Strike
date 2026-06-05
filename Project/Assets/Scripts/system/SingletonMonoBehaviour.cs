using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    protected static T _instance;
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                //シーンから探す
                _instance = FindObjectOfType<T>();

                if(_instance == null)
                {
                    Debug.LogError($"[Singleton]{typeof(T)}がシーン上にありません");
                }

               
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {

        if(_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(_instance != this)
        {
            Destroy(this.gameObject);
        }

    }

}
