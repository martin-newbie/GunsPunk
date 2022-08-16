using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : Singleton<AudioManager>
{

    class VolumeSave
    {
        public float bgm = 1f;
        public float ui = 1f;
        public float game = 1f;
    }

    [SerializeField] bool clearSave;

    Dictionary<string, AudioClip> EffectAudioClips = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> BackgroundAudioClips = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> UIAudioClips = new Dictionary<string, AudioClip>();

    AudioObject audioPrefab;
    int stackCount = 50;
    Stack<AudioObject> AudioStack = new Stack<AudioObject>();

    List<AudioObject> CurrentPlayAudio = new List<AudioObject>();

    VolumeSave volumeSave = new VolumeSave();

    public float volumeBGM = 1f;
    public float volumeUI = 1f;
    public float volumeGAME = 1f;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        LoadVolume();
        DontDestroyOnLoad(gameObject);
    }

    void LoadVolume()
    {
        string data = PlayerPrefs.GetString("audio", "none");
        if (data != "none")
        {
            volumeSave = JsonUtility.FromJson<VolumeSave>(data);

            volumeBGM = volumeSave.bgm;
            volumeUI = volumeSave.ui;
            volumeGAME = volumeSave.game;
        }
        else
        {
            volumeSave = new VolumeSave();
        }
    }

    void SaveVolume()
    {
        volumeSave.bgm = volumeBGM;
        volumeSave.ui = volumeUI;
        volumeSave.game = volumeGAME;

        string data = JsonUtility.ToJson(volumeSave, true);
        PlayerPrefs.SetString("audio", data);

        if (clearSave) PlayerPrefs.DeleteKey("audio");
    }

    void Start()
    {

        audioPrefab = Resources.Load<AudioObject>("Prefabs/AudioObject");

        foreach (var item in Resources.LoadAll<AudioClip>("Audio/Effects"))
        {
            EffectAudioClips.Add(item.name, item);
        }
        foreach (var item in Resources.LoadAll<AudioClip>("Audio/Backgrounds"))
        {
            BackgroundAudioClips.Add(item.name, item);
        }
        foreach (var item in Resources.LoadAll<AudioClip>("Audio/UI"))
        {
            UIAudioClips.Add(item.name, item);
        }

        SpawnAudioPrefab();
        instance = this;
    }

    void SpawnAudioPrefab()
    {
        for (int i = 0; i < stackCount; i++)
        {
            AudioObject temp = Instantiate(audioPrefab, transform);
            temp.Init(this);
            temp.gameObject.SetActive(false);
            AudioStack.Push(temp);
        }
    }

    public void PlayBakcgroundSound(string key)
    {
        audioPrefab.PlayStaticSound(BackgroundAudioClips[key]);
    }

    public void ClearAudio()
    {
        foreach (var item in CurrentPlayAudio)
        {
            item.Push();
        }

        CurrentPlayAudio.Clear();
    }

    public void PlayUISound(string key)
    {
        AudioObject audio = Pop();
        audio.PlayStaticSound(UIAudioClips[key]);
    }

    public void PlayEffectSound(string key, Vector3 pos, bool loop = false)
    {
        AudioObject audio = Pop();
        audio.PlaySound(EffectAudioClips[key], pos, loop);
    }

    AudioObject Pop()
    {
        if (AudioStack.Count <= 0) SpawnAudioPrefab();


        AudioObject audio = AudioStack.Pop();
        audio.gameObject.SetActive(true);
        CurrentPlayAudio.Add(audio);
        return audio;
    }

    public void Push(AudioObject audio)
    {
        if (CurrentPlayAudio.Contains(audio)) CurrentPlayAudio.Remove(audio);

        audio.gameObject.SetActive(false);
        AudioStack.Push(audio);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveVolume();
    }

    private void OnApplicationQuit()
    {
        SaveVolume();
    }
}
