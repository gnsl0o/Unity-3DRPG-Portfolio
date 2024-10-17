using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public float bgmVolume;
    AudioSource bgmSource; // 배경음악 소스

    [Header("#SFX")]
    public float sfxVolume = 1.0f;
    AudioSource[] sfxSource; // 효과음 소스
    public int channels = 10;

    private Dictionary<string, AudioClip> audioClips;
    private int channelIndex = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        Init();
    }

    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmSource = bgmObject.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;
        bgmSource.volume = bgmVolume;

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxSource = new AudioSource[channels];

        for (int i = 0; i < sfxSource.Length; i++)
        {
            sfxSource[i] = sfxObject.AddComponent<AudioSource>();
            sfxSource[i].playOnAwake = false;
            sfxSource[i].volume = sfxVolume;
        }

        audioClips = new Dictionary<string, AudioClip>();
    }

    public void PlayBGM(string bgmName)
    {
        if (audioClips.TryGetValue(bgmName, out AudioClip clip))
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
        else
        {
            AudioClip loadedClip = Resources.Load<AudioClip>("Audio/" + bgmName);
            if (loadedClip != null)
            {
                audioClips[bgmName] = loadedClip;
                bgmSource.clip = loadedClip;
                bgmSource.Play();
            }
            else
            {
                Debug.LogError("BGM 파일을 찾을  수 없습니다: " + bgmName);
            }
        }
    }

    public void PlaySFX(string sfxName)
    {
        if (audioClips.TryGetValue(sfxName, out AudioClip clip))
        {
            PlaySFXClip(clip);
        }
        else
        {
            AudioClip loadedClip = Resources.Load<AudioClip>("Audio/" + sfxName);
            if (loadedClip != null)
            {
                audioClips[sfxName] = loadedClip;
                PlaySFXClip(loadedClip);
            }
            else
            {
                Debug.LogError("SFX 파일을 찾을 수 없습니다: " + sfxName);
            }
        }
    }

    private void PlaySFXClip(AudioClip clip)
    {
        int loopIndex = channelIndex % channels;

        if (!sfxSource[loopIndex].isPlaying)
        {
            sfxSource[loopIndex].clip = clip;
            sfxSource[loopIndex].Play();
            channelIndex++;
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmSource.volume = bgmVolume;
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        foreach (var source in sfxSource)
        {
            source.volume = sfxVolume;
        }
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    public void StopSFX(string sfxName)
    {
        foreach (var source in sfxSource)
        {
            if (source.clip != null && source.clip.name == sfxName && source.isPlaying)
            {
                source.Stop();
                break;
            }
        }
    }

    private void LoadVolumeSetting()
    {
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
    }
}
