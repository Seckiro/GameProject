public class FloorSystem : SystemBase
{
    private FloorPoolControl _floorPoolControl = null;

    public override bool SystemActive { get; set; }

    private ScoreSystem ScoreSystem => GameManager.Instance.GetSystem<ScoreSystem>();

    public override void SystemInit()
    {
        _floorPoolControl = new FloorPoolControl();
    }

    public override void SystemReady()
    {
        // ±ﬂΩÁ≥ı ºªØ
        _floorPoolControl.LoadFloorData();
    }

    public override void SystemStart()
    {
        _floorPoolControl.StartGame();
        // _currentHeight = Camera.main.transform.position.y - _generateRange;
    }

    public override void SystemUpdate()
    {
        _floorPoolControl.UpdateCurrentFloor(ScoreSystem.CurrentGrade);
    }

    public override void SystemEnd()
    {
        _floorPoolControl.GameEnd();
    }

    public override void SystemDestroy()
    {


    }
}
