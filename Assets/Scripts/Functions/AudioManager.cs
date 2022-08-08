using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{

    Dictionary<string, AudioClip> EffectAudioClips = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> BackgroundAudioClips = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> UIAudioClips = new Dictionary<string, AudioClip>();

    AudioObject audioPrefab;
    int stackCount = 50;
    Stack<AudioObject> AudioStack = new Stack<AudioObject>();

    List<AudioObject> CurrentPlayAudio = new List<AudioObject>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        audioPrefab = Resources.Load("AudioObject") as AudioObject;

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
        audio.PlayEffectSound(EffectAudioClips[key], pos, loop);
    }

    AudioObject Pop()
    {
        AudioObject audio = AudioStack.Pop();
        CurrentPlayAudio.Add(audio);
        return audio;
    }

    public void Push(AudioObject audio)
    {

        if (CurrentPlayAudio.Contains(audio)) CurrentPlayAudio.Remove(audio);

        audio.gameObject.SetActive(false);
        AudioStack.Push(audio);
    }
}
