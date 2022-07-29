using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Collider2D atkCol;
    HostileAttack atk;

    void Awake()
    {
        atkCol = GetComponent<Collider2D>();
        atk = GetComponent<HostileAttack>();
        atkCol.enabled = false;
    }

    private void Update()
    {
        float move = InGameManager.Instance.objectSpeed;
        transform.Translate(Vector3.left * move * Time.deltaTime);
    }

    void AttackOn()
    {
        atkCol.enabled = true;
    }

    void AttackOff()
    {
        atkCol.enabled = false;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
