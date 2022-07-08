using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    [Header("Player")]
    public PlayerBase CurPlayer;

    [Header("UI")]
    public Canvas canvas;
    public GaugeContainer GaugePrefab;

    [Header("Object")]
    public Entity[] Hurdles;
    public Entity[] Monsters;
    public Transform[] SpawnPoses;

    [Header("Values")]
    public float objectSpeed;
    public float hurdleSpawnSpeed;
    public float monsterSpawnSpeed;

    Coroutine monsterSpawn_coroutine;
    Coroutine hurdleSpawn_coroutine;

    public GaugeContainer SpawnGaugeBar()
    {
        GaugeContainer temp = Instantiate(GaugePrefab, canvas.transform);
        temp.transform.SetAsFirstSibling();
        return temp;
    }

    private void Start()
    {
        GameStart();
    }

    public void GameOver()
    {
        StopCoroutine(hurdleSpawn_coroutine);
        StopCoroutine(monsterSpawn_coroutine);
    }

    public void GameStart()
    {
        hurdleSpawn_coroutine = StartCoroutine(HurdleCoroutine());
        monsterSpawn_coroutine = StartCoroutine(MonsterCoroutine());
    }

    IEnumerator HurdleCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(hurdleSpawnSpeed);
            SpawnHurdle();
        }
    }

    IEnumerator MonsterCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(monsterSpawnSpeed);
            SpawnMonsters();
        }
    }

    public void SpawnHurdle()
    {
        int rand_h = Random.Range(0, Hurdles.Length);
        int rand_t = Random.Range(0, SpawnPoses.Length);

        Entity temp = Instantiate(Hurdles[rand_h], SpawnPoses[rand_t].position, Quaternion.identity);
    }

    public void SpawnMonsters()
    {
        int rand_h = Random.Range(0, Monsters.Length);
        int rand_t = Random.Range(0, SpawnPoses.Length);

        // Todo: monster
        // Entity temp = Instantiate(Monsters[rand_h], SpawnPoses[rand_t].position, Quaternion.identity);
    }
}
