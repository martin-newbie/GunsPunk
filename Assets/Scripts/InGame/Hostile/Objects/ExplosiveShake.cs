using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveShake : MonoBehaviour
{

    public float radius = 10f;

    void Start()
    {
        CheckRadius();
    }

    void CheckRadius()
    {
        var player = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player"));
        if(player != null)
        {
            PlayerBase pb = player.GetComponent<PlayerBase>();
            Camera.main.GetComponent<CameraController>().ShakeForTime(0.5f, 0.5f * (Vector3.Distance(transform.position, pb.transform.position) / radius));
        }
    }
}
