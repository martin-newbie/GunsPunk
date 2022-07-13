using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCase
{
    Health,
    Ammunition
}

public class Item : MonoBehaviour
{
    float moveSpeed;
    public ItemCase Case;

    protected virtual void Update()
    {
        moveSpeed = InGameManager.Instance.objectSpeed;

        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

}
