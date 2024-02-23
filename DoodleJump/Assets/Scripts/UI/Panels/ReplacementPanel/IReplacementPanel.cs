using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReplacementPanel : IUIPanelBase
{
    int ListBackGroundCount { get; }

    void CharacterCallBack(GameObject gameObject, int index);

    void BackGroundCallBack(GameObject gameObject, int index);

}
