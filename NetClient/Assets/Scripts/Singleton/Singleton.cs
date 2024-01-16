using System;

public abstract class SingLeton<T>
{
    private readonly static object lockObj = new object();

    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = (T)System.Activator.CreateInstance(typeof(T));
                    }
                }
            }
            return _instance;
        }
    }

    protected SingLeton()
    {
        Initialize();
    }

    public virtual void Initialize() { }
}
