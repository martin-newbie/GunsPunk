using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
    public BusterWarning MonsterBuster;
    public Item[] Items;
    public Explosion explosion;
    public CameraController Cam;
    public AudioListener listener;

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

    [Header("Effects")]
    public ParticleSystem ammoEffect;

    Coroutine monsterSpawn_coroutine;
    Coroutine hurdleSpawn_coroutine;

    public void AmmoEffect(Vector3 pos, int count)
    {
        ammoEffect.transform.position = pos;

        var emission = ammoEffect.emission;

        ParticleSystem.Burst burst = emission.GetBurst(0);
        burst.count = count;
        emission.SetBurst(0, burst);

        ammoEffect.Play();
    }

    public void GetRoundCoin(int value)
    {
        roundCoin += value;
        GameManager.Instance.acquiredCoin += value;
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
        listener.transform.position = CurPlayer.transform.position;


        if (isGameActive)
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
        isGameActive = false;
        FindMonstersDestroy();

        StopCoroutine(hurdleSpawn_coroutine);
        StopCoroutine(monsterSpawn_coroutine);

        GameManager.Instance.SetBestScore(roundDistance);

        InGameUIManager.Instance.StartGameEnd();
        InGameUIManager.Instance.SetPlayerHp(0f, CurPlayer.maxHP);
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

    void FindMonstersDestroy()
    {
        var monsters = FindObjectsOfType<Monster>();
        foreach (var m in monsters)
        {
            StartCoroutine(m.DestroyMove());
        }
    }

    IEnumerator PlayerSwapCoroutine()
    {

        isRevived = true;
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
        float tik = 1f;
        float curTik = 0f;
        int secondCount = 0;
        int spawn = 0;

        while (true)
        {

            curTik += Time.deltaTime;
            if (tik <= curTik)
            {

                if (Random.Range(0, (20 - spawn)) <= 2)
                {
                    SpawnHurdle();
                }


                secondCount++;

                if (secondCount % 20 == 0 && secondCount != 0 && spawn < 10) spawn++;

                curTik = 0f;
            }


            yield return new WaitForSeconds(hurdleSpawnSpeed + Random.Range(-1.5f, 0f));
            SpawnHurdle();

            yield return null;
        }
    }

    IEnumerator MonsterCoroutine()
    {
        List<Monster> CurMonsters = new List<Monster>();
        float tik = 1f;
        float curTik = 0f;
        int maxCount = 3;
        int secondCount = 0;

        while (true)
        {
            if (tik <= curTik)
            {

                if (Random.Range(0, (50 - maxCount * 2)) <= 1)
                {
                    SpawnBuster();
                }

                if (CurMonsters.Count < maxCount)
                {
                    int spawnChance = 15 + (int)((float)CurMonsters.Count / (float)maxCount) * 10;
                    if (Random.Range(0, spawnChance) <= 2)
                    {
                        CurMonsters.Add(SpawnMonsters(Random.Range(0, 2)));
                    }
                }
                curTik = 0f;
                secondCount++;

                if (secondCount % 30 == 0 && secondCount != 0 && maxCount < 10) maxCount++;

            }
            curTik += Time.deltaTime;



            for (int i = 0; i < CurMonsters.Count; i++)
            {
                if (!CurMonsters[i].isAlive) CurMonsters.Remove(CurMonsters[i]);
            }

            yield return null;
        }
    }

    BusterWarning SpawnBuster()
    {
        float[] randY = new float[3] { -3.1f, -0.4f, 2.2f };
        float X = 10f;

        BusterWarning temp = Instantiate(MonsterBuster, new Vector3(X, randY[CurPlayer.curPosIdx]), Quaternion.identity);
        return temp;
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
