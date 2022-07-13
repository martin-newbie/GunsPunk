using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScroll : MonoBehaviour
{
    MeshRenderer SR;
    float offset;

    void Start()
    {
        SR = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (InGameManager.Instance.isGameActive)
        {
            float speed = InGameManager.Instance.objectSpeed;
            offset += (speed / 15) * Time.deltaTime;
            SR.material.mainTextureOffset = new Vector2(offset, 0);
        }
    }
}
