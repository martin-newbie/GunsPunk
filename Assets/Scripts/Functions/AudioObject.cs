using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioObject : MonoBehaviour
{
    AudioSource Audio;
    AudioManager manager;

    public void Init(AudioManager _manager)
    {
        Audio = GetComponent<AudioSource>();
        manager = _manager;
    }

    public void PlayEffectSound(AudioClip clip, Vector3 pos = default, bool loop = false)
    {
        transform.position = pos;
        Audio.clip = clip;
        Audio.loop = loop;
        Audio.spatialBlend = 1f;

        Audio.Play();

        if (!loop)
            Invoke("Push", clip.length);
    }

    public void PlayStaticSound(AudioClip clip, bool loop = false)
    {
        Audio.clip = clip;
        Audio.loop = loop;
        Audio.spatialBlend = 0f;

        Audio.Play();

        if (!loop)
            Invoke("Push", clip.length);
    }

    public void Push()
    {
        Audio.Stop();
        manager.Push(this);
    }
}
