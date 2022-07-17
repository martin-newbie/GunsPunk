using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimation : MonoBehaviour
{
    private void Update()
    {
        float move = InGameManager.Instance.objectSpeed;
        transform.Translate(Vector3.left * move * Time.deltaTime);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
