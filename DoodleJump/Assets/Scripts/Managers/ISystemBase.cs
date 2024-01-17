public interface ISystemBase : ISystemInit, ISystemStart, ISystemUpdata, ISystemEnd, ISystemDestroy, ISystemQuit { }

public interface ISystemInit
{
    void SystemInit();
}
public interface ISystemStart
{
    void SystemStart();
}
public interface ISystemUpdata
{
    void SystemUpdata();
}
public interface ISystemEnd
{
    void SystemEnd();
}
public interface ISystemDestroy
{
    void SystemDestroy();
}
public interface ISystemQuit
{
    void SystemQuit();
}
