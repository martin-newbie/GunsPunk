using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideMonsterWarning : MonoBehaviour
{
    public MonsterBuster buster;
    public SpriteRenderer warning;
    public float moveSpeed;

    void Start()
    {
        StartCoroutine(WarningCoroutine(2f));
    }

    IEnumerator WarningCoroutine(float duration)
    {
        // warning spawn
        {
            float timer = 0f;

            while (timer < duration)
            {
                Vector2 size = new Vector2(20f * (timer / duration), 2f);
                warning.size = size;
                timer += Time.deltaTime;
                yield return null;
            }
        }

        // warning dissapear
        {
            float timer = duration;

            while (timer > 0f)
            {
                Vector2 size = new Vector2(20f, (timer / duration) * 2f);
                warning.size = size;
                timer -= Time.deltaTime;
                yield return null;
            }
        }

        // spawn monster
        {
            warning.gameObject.SetActive(false);

            Instantiate(buster, transform.position + new Vector3(0, -1.5f), Quaternion.identity);
        }

        yield break;
    }
}
