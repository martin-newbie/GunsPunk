using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 originPos;
    Coroutine onCoroutine;

    private void Start()
    {
        originPos = transform.position;
    }

    public void ShakeForTime(float duration = 0.5f)
    {
        if (onCoroutine != null) StopCoroutine(onCoroutine);
        StartCoroutine(ShakeCoroutine(duration));

    }

    IEnumerator ShakeCoroutine(float duration, float amt = 0.5f)
    {
        float timer = duration;

        while (timer > 0f)
        {
            Vector3 randPos = originPos + ((Vector3)Random.insideUnitCircle * amt * (timer / duration));
            transform.position = randPos;

            timer -= Time.deltaTime;
            yield return null;
        }

        yield break;
    }
}
