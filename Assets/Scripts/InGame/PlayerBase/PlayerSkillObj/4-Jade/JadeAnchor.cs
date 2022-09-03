using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JadeAnchor : MonoBehaviour
{

    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hostile"))
        {
            AudioManager.Instance.PlayEffectSound("AnchorHit");
            collision.GetComponent<Entity>().OnHit(damage, transform);
        }
    }
}
