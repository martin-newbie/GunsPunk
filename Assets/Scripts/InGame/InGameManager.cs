using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    [Header("Player")]
    public PlayerBase CurPlayer;
    public PlayerBase SubPlayer;
    public PlayerBase[] PlayerPrefabs;
    public Transform[] PlayerPoses;
    public float PlayerStartPos_X;

    [Header("UI")]
    public Canvas canvas;
    public GaugeContainer GaugePrefab;

    [Header("Object")]
    public Entity[] Hurdles;
    public Transform[] SpawnPoses;
    public Monster MonsterMelee;
    public Monster MonsterGun;
    public Item[] Items;
    public Explosion explosion;

    [Header("Values")]
    public float objectSpeed;
    public float hurdleSpawnSpeed;
    public float monsterSpawnSpeed;
    public bool isRevived;
    public bool isGameActive;
    public int roundCoin;

    Coroutine monsterSpawn_coroutine;
    Coroutine hurdleSpawn_coroutine;

    public void SetRoundCoin(int value)
    {
        roundCoin += value;
        InGameUIManager.Instance.RoundCoin = roundCoin;
    }

    public Explosion SpawnExplosion(Vector3 pos)
    {
        Explosion temp = Instantiate(explosion, pos, Quaternion.identity);
        return temp;
    }

    public GaugeContainer SpawnGaugeBar()
    {
        GaugeContainer temp = Instantiate(GaugePrefab, canvas.transform);
        temp.transform.SetAsFirstSibling();
        return temp;
    }

    private void Start()
    {
        int playerIdx = GameManager.Instance.PlayerIdx;
        PlayerBase temp = Instantiate(PlayerPrefabs[playerIdx], PlayerPoses[0].position, Quaternion.identity);
        CurPlayer = temp;
        CurPlayer.UIInit();

        int subPlayerIdx = GameManager.Instance.SubPlayerIdx;
        temp = Instantiate(PlayerPrefabs[subPlayerIdx], PlayerPoses[0].position, Quaternion.identity);
        SubPlayer = temp;
        SubPlayer.gameObject.SetActive(false);

        SetRoundCoin(0);
        GameStart();
    }

    public Item SpawnRandomItem(Vector3 pos, int idx = -1)
    {
        int rand = Random.Range(0, Items.Length);

        Item item = idx == -1 ? Items[rand] : Items[idx];

        Item spawn = Instantiate(item, pos, Quaternion.identity);
        return spawn;
    }

    public void GameOver()
    {
        StopCoroutine(hurdleSpawn_coroutine);
        StopCoroutine(monsterSpawn_coroutine);


        if (!isRevived)
        {
            StartCoroutine(ReviveCoroutine());
        }
        else
        {

        }
    }

    IEnumerator ReviveCoroutine()
    {
        isRevived = true;
        isGameActive = false;
        InGameUIManager.Instance.SetPlayerHp(0, CurPlayer.maxHP);

        int curIdx = CurPlayer.curPosIdx;
        Vector3 spawnPos = PlayerPoses[curIdx].position;

        SubPlayer.gameObject.SetActive(true);
        SubPlayer.transform.position = spawnPos;
        SubPlayer.curPosIdx = curIdx;

        while (SubPlayer.transform.position.x <= PlayerStartPos_X)
        {
            SubPlayer.MoveForward();
            yield return null;
        }
        SubPlayer.UIInit();

        PlayerBase temp = CurPlayer;
        CurPlayer = SubPlayer;
        SubPlayer = null;

        isGameActive = true;

        while (temp.transform.position.x >= -12f)
        {
            temp.MoveForward(-1);
            yield return null;
        }

        hurdleSpawn_coroutine = StartCoroutine(HurdleCoroutine());
        monsterSpawn_coroutine = StartCoroutine(MonsterCoroutine());

        yield break;
    }

    public void GameStart()
    {
        StartCoroutine(GameStartCoroutine());
    }

    IEnumerator GameStartCoroutine()
    {
        while (CurPlayer.transform.position.x <= PlayerStartPos_X)
        {
            CurPlayer.MoveForward();
            yield return null;
        }

        isGameActive = true;

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
            yield return new WaitForSeconds(spawnDelay);
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
