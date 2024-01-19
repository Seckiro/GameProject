using UnityEngine;

public class CameraFollowSystem : SystemBase
{
    private Character _character;
    private CameraFollow _cameraFollow;

    public override bool SystemActive { get; set; }

    public override void SystemInit()
    {
        // �ҵ����������ű�
        _cameraFollow = Object.FindFirstObjectByType<CameraFollow>();

        _character = GameManager.Instance.GetSystem<CharacterSystem>().CurrentCharacter;

        // �ҵ����
        _cameraFollow.SetFollowInit(5);

        _cameraFollow.SetFollowObj(_character);
    }

    public override void SystemStart()
    {
        _cameraFollow.SetFollowStart();
    }

    public override void SystemEnd()
    {
        _cameraFollow.SetFollowEnd();
    }
}
