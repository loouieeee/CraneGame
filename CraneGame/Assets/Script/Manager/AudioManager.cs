using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource sePrefab; // 用于实例化 SE

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource seSource;

    [Header("BGM Sources")]
    public AudioClip TitleBGM;
    public AudioClip StageSelectBGM;
    public AudioClip InGameBGM;

    [Header("SE Clips")]
    public AudioClip jumpSE;
    public AudioClip attackSE;
    public AudioClip clickSE;
    public AudioClip getDamageSE;
    public AudioClip FOFISE;
    public AudioClip gameOverSE;
    public AudioClip gameClearSE;
    public AudioClip gameFinishSE;

    [Header("CRANE SE")]
    public AudioClip craneMoveDownSE;
    public AudioClip craneMoveHorizontalSE;
    public AudioClip craneCloseSE;
    public AudioClip craneOpenSE;
    public AudioClip panelSE;
    public AudioClip panelAreaEffectSE;

    [Header("Cylinder SE")]
    public AudioClip CylinderSE;

    private Dictionary<AudioClip, AudioSource> sePlaying = new Dictionary<AudioClip, AudioSource>();

    private Coroutine fadeCoroutine;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this); 
        }
        else
        {
            Destroy(this);
        }
    }

    //public void PlayBGM(AudioClip clip)
    //{
    //    if (bgmSource.clip == clip) return;
    //    bgmSource.clip = clip;
    //    bgmSource.loop = true;
    //    bgmSource.Play();
    //}

    //public void StopBGM()
    //{
    //    bgmSource.Stop();
    //}

    // 播放BGM
    public void PlayBGM(AudioClip clip, bool loop = true, float fadeTime = 1f)
    {
        if (clip == null) return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeInBGM(clip, fadeTime, loop));
    }

    // 停止BGM（淡出）
    public void StopBGM(float fadeTime = 1f)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOutBGM(fadeTime));
    }
    private IEnumerator FadeInBGM(AudioClip newClip, float duration, bool loop)
    {
        if (bgmSource.isPlaying)
            yield return StartCoroutine(FadeOutBGM(duration / 2));

        bgmSource.clip = newClip;
        bgmSource.loop = loop;
        bgmSource.volume = 0;
        bgmSource.Play();

        float timer = 0f;
        while (timer < duration)
        {
            bgmSource.volume = Mathf.Lerp(0f, 1f, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        bgmSource.volume = 1f;
    }

    private IEnumerator FadeOutBGM(float duration)
    {
        float startVolume = bgmSource.volume;
        float timer = 0f;

        while (timer < duration)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.volume = 1f;
    }

    public void PlaySE(AudioClip clip)
    {
        if (clip == null) return;
        seSource.PlayOneShot(clip);

        //clip = Resources.Load<AudioClip>("Audio/jump");
    }
    //public void PlaySE(AudioClip clip, bool loop = false)
    //{
    //    if (clip == null) return;

    //    seSource.clip = clip;        // 把音频赋值给 AudioSource
    //    seSource.loop = loop;        // 设置是否循环
    //    seSource.Play();             // 播放
    //}

    public void StopSE()
    {
        seSource.Stop();
        seSource.loop = false;
    }

    // 播放 SE
    public void PlaySE(AudioClip clip, bool loop = false)
    {
        if (clip == null) return;
        if (!sePlaying.ContainsKey(clip))
        {
            AudioSource source = Instantiate(sePrefab, transform);
            source.clip = clip;
            source.loop = loop;
            source.Play();
            sePlaying.Add(clip, source);
        }
    }
    // 停止指定 SE
    public void StopSE(AudioClip clip)
    {
        if (clip == null) return;
        if (sePlaying.TryGetValue(clip, out AudioSource source))
        {
            source.Stop();
            Destroy(source.gameObject);
            sePlaying.Remove(clip);
        }
    }

    public void PlayTitleBGM() => PlayBGM(TitleBGM);
    public void PlayInGameBGM()=>PlayBGM(InGameBGM);
    public void PlayClickSE() => PlaySE(clickSE);
    public void PlayGetDamageSE() => PlaySE(getDamageSE);


    public void PlayBGMLoop(AudioClip clip)
    {
        if (bgmSource.clip == clip) return;

        bgmSource.clip = clip;
        bgmSource.Play();
        bgmSource.loop = true;
    }
}
