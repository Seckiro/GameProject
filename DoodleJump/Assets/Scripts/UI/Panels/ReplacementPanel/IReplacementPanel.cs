using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReplacementPanel : IUIPanelBase
{
    int ListBackGroundCount { get; }

    void CharacterCallBack(GameObject gameObject, int index);

    void CharacterClick(GameObject gameObject, int index);

    void CharacterButtonClick(int index, bool active, GameObject gameObject);

    void BackGroundCallBack(GameObject gameObject, int index);

    void BackGroundClick(GameObject gameObject, int index);

    void BackGroundButtonClick(int index, bool active, GameObject gameObject);


}
