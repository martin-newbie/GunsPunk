using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HomeUI : MonoBehaviour
{

    protected HomeUIManager manager;

    public void Init(HomeUIManager _manager)
    {
        manager = _manager;
    }

}
