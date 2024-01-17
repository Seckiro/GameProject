using UnityEngine;

public abstract class SystemBase : ISystemBase
{
    public virtual void SystemInit() { }

    public virtual void SystemStart() { }

    public virtual void SystemUpdata() { }

    public virtual void SystemEnd() { }

    public virtual void SystemQuit() { }

    public virtual void SystemDestroy() { }
}

