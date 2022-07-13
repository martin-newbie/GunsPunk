using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScroll : MonoBehaviour
{
    SpriteRenderer SR;
    float offset;

    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float speed = InGameManager.Instance.objectSpeed;
        offset += speed * Time.deltaTime;
        SR.material.mainTextureOffset = new Vector2(offset, 0);
    }
}
