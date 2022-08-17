using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffect : MonoBehaviour
{

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Play(Vector3 pos = default)
    {
        transform.position = pos;
        anim.SetTrigger("Trigger");
    }

}
