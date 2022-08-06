using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusterWarning : MonoBehaviour
{
    public MonsterBuster buster;
    public SpriteRenderer warning;
    public float moveSpeed;

    void Start()
    {
        StartCoroutine(WarningCoroutine(3f));
    }

    IEnumerator WarningCoroutine(float duration)
    {

        float timer = duration;
        float delay = 0.5f;

        while (timer > 0f)
        {

            warning.enabled = true;
            yield return new WaitForSeconds(delay);
            warning.enabled = false;
            yield return new WaitForSeconds(delay);

            timer -= delay;
            delay *= 0.85f;

            if (!InGameManager.Instance.isGameActive) Destroy(gameObject);
        }
        

        // spawn monster
        {
            warning.gameObject.SetActive(false);

            Instantiate(buster, transform.position + new Vector3(0, -1.5f), Quaternion.identity);
        }
        Destroy(gameObject);
        yield break;
    }
}
