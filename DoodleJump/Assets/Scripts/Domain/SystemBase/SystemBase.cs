using UnityEngine;

public abstract class SystemBase : ISystemBase
{
    public abstract bool SystemActive { get; set; }

    public virtual void SystemInit() { }

    public virtual void SystemReady() { }

    public virtual void SystemStart() { }

    public virtual void SystemUpdate() { }

    public virtual void SystemEnd() { }

    public virtual void SystemQuit() { }

    public virtual void SystemDestroy() { }
}

