using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class BGMData
    {
        public string id;
        public AudioClip clip;
    }

    [System.Serializable]
    public class SEData
    {
        public string id;
        public AudioClip clip;
    }

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("BGM List")]
    [SerializeField]
    private List<BGMData> bgmList =
        new List<BGMData>();

    [Header("SE List")]
    [SerializeField]
    private List<SEData> seList =
        new List<SEData>();

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSource;

    [Header("BGM Fade")]
    [SerializeField] private float fadeDuration = 1.0f;

    private Dictionary<string, AudioClip> bgmDictionary =
        new Dictionary<string, AudioClip>();

    private Dictionary<string, AudioClip> seDictionary =
        new Dictionary<string, AudioClip>();

    private Coroutine bgmFadeCoroutine;

    private const float DEFAULT_VOLUME = 0.5f;

    private const string BGM_VOLUME_KEY = "BGM_VOLUME";
    private const string SE_VOLUME_KEY = "SE_VOLUME";

    private const string BGM_MIXER_PARAMETER = "BGMVolume";
    private const string SE_MIXER_PARAMETER = "SEVolume";


    // =====================================================
    // 初期化
    // =====================================================

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        InitializeAudioSources();
        InitializeBGMDictionary();
        InitializeSEDictionary();
    }


    private void Start()
    {
        LoadSettings();
        ApplyVolumes();
    }


    // =====================================================
    // AudioSource初期化
    // =====================================================

    private void InitializeAudioSources()
    {
        if (bgmSource == null)
        {
            bgmSource =
                gameObject.AddComponent<AudioSource>();
        }

        bgmSource.playOnAwake = false;
        bgmSource.loop = true;

        if (seSource == null)
        {
            seSource =
                gameObject.AddComponent<AudioSource>();
        }

        seSource.playOnAwake = false;
        seSource.loop = false;


        if (audioMixer != null)
        {
            AudioMixerGroup[] groups =
                audioMixer.FindMatchingGroups("BGM");

            if (groups.Length > 0)
            {
                bgmSource.outputAudioMixerGroup =
                    groups[0];
            }


            groups =
                audioMixer.FindMatchingGroups("SE");

            if (groups.Length > 0)
            {
                seSource.outputAudioMixerGroup =
                    groups[0];
            }
        }
    }


    // =====================================================
    // BGM Dictionary
    // =====================================================

    private void InitializeBGMDictionary()
    {
        bgmDictionary.Clear();

        foreach (BGMData data in bgmList)
        {
            if (data == null)
                continue;

            if (string.IsNullOrEmpty(data.id))
                continue;

            if (data.clip == null)
                continue;


            if (bgmDictionary.ContainsKey(data.id))
            {
                Debug.LogWarning(
                    "BGM IDが重複しています: " +
                    data.id
                );

                continue;
            }


            bgmDictionary.Add(
                data.id,
                data.clip
            );
        }
    }


    // =====================================================
    // SE Dictionary
    // =====================================================

    private void InitializeSEDictionary()
    {
        seDictionary.Clear();

        foreach (SEData data in seList)
        {
            if (data == null)
                continue;

            if (string.IsNullOrEmpty(data.id))
                continue;

            if (data.clip == null)
                continue;


            if (seDictionary.ContainsKey(data.id))
            {
                Debug.LogWarning(
                    "SE IDが重複しています: " +
                    data.id
                );

                continue;
            }


            seDictionary.Add(
                data.id,
                data.clip
            );
        }
    }


    // =====================================================
    // BGM再生
    // =====================================================

    public void PlayBGM(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning(
                "BGM IDが空です。"
            );

            return;
        }


        if (!bgmDictionary.TryGetValue(
            id,
            out AudioClip clip))
        {
            Debug.LogWarning(
                "BGMが見つかりません。ID: " +
                id
            );

            return;
        }


        PlayBGM(clip);
    }


    public void PlayBGM(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning(
                "BGMのAudioClipが設定されていません。"
            );

            return;
        }


        if (bgmSource == null)
        {
            Debug.LogError(
                "BGM用AudioSourceがありません。"
            );

            return;
        }


        if (bgmSource.clip == clip &&
            bgmSource.isPlaying)
        {
            return;
        }


        if (bgmFadeCoroutine != null)
        {
            StopCoroutine(bgmFadeCoroutine);
        }


        bgmFadeCoroutine =
            StartCoroutine(
                ChangeBGMWithFade(clip)
            );
    }


    private IEnumerator ChangeBGMWithFade(
        AudioClip newClip)
    {
        float startVolume =
            bgmSource.volume;


        if (bgmSource.isPlaying)
        {
            float time = 0f;


            while (time < fadeDuration)
            {
                time += Time.deltaTime;


                float t =
                    time / fadeDuration;


                bgmSource.volume =
                    Mathf.Lerp(
                        startVolume,
                        0f,
                        t
                    );


                yield return null;
            }
        }


        bgmSource.volume = 0f;

        bgmSource.Stop();

        bgmSource.clip = newClip;

        bgmSource.Play();


        float fadeInTime = 0f;


        while (fadeInTime < fadeDuration)
        {
            fadeInTime += Time.deltaTime;


            float t =
                fadeInTime / fadeDuration;


            bgmSource.volume =
                Mathf.Lerp(
                    0f,
                    1f,
                    t
                );


            yield return null;
        }


        bgmSource.volume = 1f;

        bgmFadeCoroutine = null;
    }


    public void StopBGM()
    {
        if (bgmFadeCoroutine != null)
        {
            StopCoroutine(bgmFadeCoroutine);

            bgmFadeCoroutine = null;
        }


        if (bgmSource == null)
            return;


        bgmSource.Stop();

        bgmSource.clip = null;

        bgmSource.volume = 1f;
    }


    // =====================================================
    // SE再生
    // =====================================================

    public void PlaySE(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning(
                "SE IDが空です。"
            );

            return;
        }


        if (!seDictionary.TryGetValue(
            id,
            out AudioClip clip))
        {
            Debug.LogWarning(
                "SEが見つかりません。ID: " +
                id
            );

            return;
        }


        PlaySE(clip);
    }


    // 通常のSE
    public void PlaySE(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning(
                "SEのAudioClipが設定されていません。"
            );

            return;
        }


        if (seSource == null)
        {
            Debug.LogError(
                "SE用AudioSourceがありません。"
            );

            return;
        }


        seSource.PlayOneShot(clip);
    }


    // =====================================================
    // SE再生（音量倍率指定）
    // =====================================================

    public void PlaySE(
        string id,
        float volumeMultiplier)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning(
                "SE IDが空です。"
            );

            return;
        }


        if (!seDictionary.TryGetValue(
            id,
            out AudioClip clip))
        {
            Debug.LogWarning(
                "SEが見つかりません。ID: " +
                id
            );

            return;
        }


        if (seSource == null)
        {
            Debug.LogError(
                "SE用AudioSourceがありません。"
            );

            return;
        }


        seSource.PlayOneShot(
            clip,
            Mathf.Max(
                0f,
                volumeMultiplier
            )
        );
    }


    // =====================================================
    // BGM音量
    // =====================================================

    public void SetBGMVolume(float value)
    {
        value =
            Mathf.Clamp01(value);


        SetMixerVolume(
            BGM_MIXER_PARAMETER,
            value
        );


        PlayerPrefs.SetFloat(
            BGM_VOLUME_KEY,
            value
        );


        PlayerPrefs.Save();
    }


    public float GetBGMVolume()
    {
        return PlayerPrefs.GetFloat(
            BGM_VOLUME_KEY,
            DEFAULT_VOLUME
        );
    }


    // =====================================================
    // SE音量
    // =====================================================

    public void SetSEVolume(float value)
    {
        value =
            Mathf.Clamp01(value);


        SetMixerVolume(
            SE_MIXER_PARAMETER,
            value
        );


        PlayerPrefs.SetFloat(
            SE_VOLUME_KEY,
            value
        );


        PlayerPrefs.Save();
    }


    public float GetSEVolume()
    {
        return PlayerPrefs.GetFloat(
            SE_VOLUME_KEY,
            DEFAULT_VOLUME
        );
    }


    // =====================================================
    // Mixer音量設定
    // =====================================================

    private void SetMixerVolume(
        string parameterName,
        float value)
    {
        if (audioMixer == null)
        {
            Debug.LogError(
                "AudioMixerが設定されていません。"
            );

            return;
        }


        if (value <= 0.0001f)
        {
            audioMixer.SetFloat(
                parameterName,
                -80f
            );

            return;
        }


        float decibel =
            Mathf.Log10(value) * 20f;


        bool success =
            audioMixer.SetFloat(
                parameterName,
                decibel
            );


        if (!success)
        {
            Debug.LogError(
                "AudioMixerのパラメータが見つかりません: " +
                parameterName
            );
        }
    }


    // =====================================================
    // 設定読み込み
    // =====================================================

    private void LoadSettings()
    {
        float bgmVolume =
            GetBGMVolume();

        float seVolume =
            GetSEVolume();


        SetMixerVolume(
            BGM_MIXER_PARAMETER,
            bgmVolume
        );


        SetMixerVolume(
            SE_MIXER_PARAMETER,
            seVolume
        );
    }


    private void ApplyVolumes()
    {
        SetMixerVolume(
            BGM_MIXER_PARAMETER,
            GetBGMVolume()
        );


        SetMixerVolume(
            SE_MIXER_PARAMETER,
            GetSEVolume()
        );
    }
}