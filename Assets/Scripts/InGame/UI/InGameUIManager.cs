using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIManager : Singleton<InGameUIManager>
{

    public void OnPointerDown()
    {
        InGameManager.Instance.CurPlayer.OnAttackStart();
    }

    public void OnPointerUp()
    {
        InGameManager.Instance.CurPlayer.OnAttackEnd();
    }
}
