using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance {
        get {
            if(instance == null) {
                instance = FindObjectOfType<T>();
            }
            return instance;
        }
    }

    protected virtual void Awake() {
        if(instance == null) {
            instance = (T)this;
        } else {
            Destroy(gameObject);
        }

        if(!gameObject.transform.parent) {
            DontDestroyOnLoad(gameObject);
        }
    }
}
