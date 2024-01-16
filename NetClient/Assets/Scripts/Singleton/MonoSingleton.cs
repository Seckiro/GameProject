using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                T[] ts = GameObject.FindObjectsOfType<T>();
                if (ts != null && ts.Length > 0)
                {
                    if (ts.Length == 1)
                    {
                        _instance = ts[0];
                    }
                    else
                    {
                        Debug.LogError(string.Format("## Uni Exception ## Cls:{0} Info:Singleton not allows more than one instance", typeof(T)));
                    }
                }
                else
                {
                    _instance = new GameObject(string.Format("{0}(Singleton)", typeof(T).ToString())).AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected MonoSingleton() { }

    protected virtual void Awake()
    {
        _instance = this as T;
        DontDestroyOnLoad(this.gameObject);
    }

    protected virtual void OnDestroy()
    {
        _instance = null;
    }
}
