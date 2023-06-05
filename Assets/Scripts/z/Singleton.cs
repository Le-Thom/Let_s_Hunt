using UnityEngine;
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected static T instance = null;
    public static T Instance
    {
        get
        {
            instance = instance ?? (FindObjectOfType(typeof(T)) as T);
            instance = instance ?? new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
            return instance;
        }
    }
}