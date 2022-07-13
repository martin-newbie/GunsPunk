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
    public Transform[] SpawnPoses;
    public Monster MonsterMelee;
    public Monster MonsterGun;
    public Item[] Items;

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

    Item SpawnRandomItem()
    {
        int rand = Random.Range(0, Items.Length);
        Item item = Items[rand];

        rand = Random.Range(0, SpawnPoses.Length);
        Transform trans = SpawnPoses[rand];

        Item spawn = Instantiate(item, trans.position, Quaternion.identity);
        return spawn;
    }

    public void GameOver()
    {
        objectSpeed = 0f;

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
            yield return new WaitForSeconds(hurdleSpawnSpeed + Random.Range(-1.5f, 0f));
            SpawnHurdle();
        }
    }

    IEnumerator MonsterCoroutine()
    {
        List<Monster> CurMonsters = new List<Monster>();
        float spawnDelay = 5f;

        while (true)
        {

            // test
            CurMonsters.Add(SpawnMonsters(0));
            yield return new WaitForSeconds(spawnDelay);
            CurMonsters.Add(SpawnMonsters(0));
            yield return new WaitForSeconds(spawnDelay);
            CurMonsters.Add(SpawnMonsters(1));
            // test

            if (Input.GetKeyDown(KeyCode.Space)) break;
            while (CurMonsters.Count > 0)
            {
                for (int i = 0; i < CurMonsters.Count; i++)
                {
                    if (!CurMonsters[i].isAlive) CurMonsters.Remove(CurMonsters[i]);
                }

                yield return null;
            }

            yield return new WaitForSeconds(spawnDelay);
        }
        yield break;
    }

    public Entity SpawnHurdle()
    {
        int rand_h = Random.Range(0, Hurdles.Length);
        int rand_t = Random.Range(0, SpawnPoses.Length);

        Entity temp = Instantiate(Hurdles[rand_h], SpawnPoses[rand_t].position, Quaternion.identity);
        return temp;
    }

    public Monster SpawnMonsters(int idx)
    {
        Monster monster = idx == 0 ? MonsterMelee : MonsterGun;

        int rand_t = Random.Range(0, SpawnPoses.Length);
        Monster temp = Instantiate(monster, SpawnPoses[rand_t].position, Quaternion.identity);

        temp.Init(rand_t);

        return temp;
    }
}
