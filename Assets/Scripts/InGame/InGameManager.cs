using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameResult
{
    public int charLevel;
    public float charExp;

    public int userLevel;
    public float userExp;

    public float roundDistance;
    public int roundCoin;
    public float roundExp;

    public GameResult(int c_level, float c_exp, int u_level, float u_exp, float r_distance, int r_coin, float r_exp)
    {
        charLevel = c_level;
        charExp = c_exp;
        userLevel = u_level;
        userExp = u_exp;
        roundDistance = r_distance;
        roundCoin = r_coin;
        roundExp = r_exp;
    }
}

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
    public CameraController Cam;

    [Header("Values")]
    public float objectSpeed;
    public float hurdleSpawnSpeed;
    public float monsterSpawnSpeed;
    public bool isRevived;
    public bool isGameActive;
    public bool isPaused;

    [Header("Results")]
    public int roundCoin;
    public float roundDistance;
    public float roundExp => roundDistance / 10f;
    GameResult result;

    Coroutine monsterSpawn_coroutine;
    Coroutine hurdleSpawn_coroutine;

    public void GetRoundCoin(int value)
    {
        roundCoin += value;
        InGameUIManager.Instance.RoundCoin = roundCoin;
    }

    public Explosion SpawnExplosion(Vector3 pos)
    {
        Explosion temp = Instantiate(explosion, pos, Quaternion.identity);
        return temp;
    }

    public GaugeContainer SpawnGaugeBar(Vector3 pos)
    {
        GaugeContainer temp = Instantiate(GaugePrefab, pos, Quaternion.identity, canvas.transform);
        temp.transform.SetAsFirstSibling();
        return temp;
    }

    private void Start()
    {
        PlayerPrefabs = GameManager.Instance.charactersPrefab;

        int playerIdx = GameManager.Instance.mainPlayerIdx;

        PlayerBase temp = Instantiate(PlayerPrefabs[playerIdx], PlayerPoses[0].position, Quaternion.identity);
        temp.Init(GameManager.Instance.GetCharacterInfo(playerIdx));

        CurPlayer = temp;
        CurPlayer.UIInit();

        int subPlayerIdx = GameManager.Instance.subPlayerIdx;
        temp = Instantiate(PlayerPrefabs[subPlayerIdx], PlayerPoses[0].position, Quaternion.identity);
        temp.Init(GameManager.Instance.GetCharacterInfo(subPlayerIdx));

        SubPlayer = temp;
        SubPlayer.gameObject.SetActive(false);

        SetHoldGaugeActive();
        GetRoundCoin(0);
        GameStart();
    }

    private void Update()
    {
        if(isGameActive)
        {
            roundDistance += objectSpeed * Time.deltaTime;
        }
    }

    void SetHoldGaugeActive()
    {
        InGameUIManager.Instance.HoldGauge.transform.parent.gameObject.SetActive(CurPlayer.GetComponent<HoldFirePlayer>() != null);
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

        InGameUIManager.Instance.StartGameEnd();

        CharacterInfo info = GameManager.Instance.GetMainPlayer();
        result = new GameResult(info.level, info.exp, GameManager.Instance.userLevel, GameManager.Instance.userExp, roundDistance, roundCoin, roundExp);
    }

    public GameResult GetResult()
    {
        GameManager.Instance.GetMainPlayer().SetExp(roundExp);
        GameManager.Instance.SetUserExp(roundExp);
        return result;
    }

    public void Revive()
    {
        StartCoroutine(ReviveCoroutine());
    }

    IEnumerator ReviveCoroutine()
    {
        yield return StartCoroutine(PlayerSwapCoroutine());

        yield break;
    }

    IEnumerator PlayerSwapCoroutine()
    {

        isRevived = true;
        isGameActive = false;
        InGameUIManager.Instance.SetPlayerHp(0, CurPlayer.maxHP);

        var monsters = FindObjectsOfType<Monster>();
        foreach (var m in monsters)
        {
            StartCoroutine(m.DestroyMove());
        }

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
        SetHoldGaugeActive();
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
