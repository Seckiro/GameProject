using UnityEngine;


public class BoundarySystem : SystemBase
{
    private Boundary _boundary;

    public override bool SystemActive { get; set; }

    public override void SystemInit()
    {
        _boundary = Object.FindObjectOfType<Boundary>();
    }

    public override void SystemReady()
    {
        _boundary.SetFollowCamera(Camera.current);
    }

    public override void SystemStart()
    {
        _boundary.SetBoundaryStart();
    }

    public override void SystemEnd()
    {
        _boundary.SetBoundaryEnd();
    }
}
