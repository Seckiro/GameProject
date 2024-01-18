using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSystem : SystemBase
{
    private CameraFollow _cameraFollow;
    private Character _character;
    public override bool SystemActive { get; set; }

    public override void SystemInit()
    {
        // �ҵ����������ű�
        _cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();

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
