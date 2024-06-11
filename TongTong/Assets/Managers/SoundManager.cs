using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SoundManager : ManagerBase
{
    private AudioSource bgmAudio;
    private List<AudioSource> sfxSources;

    private Dictionary<string, AudioClip> dicBgmClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> dicSfxClips = new Dictionary<string, AudioClip>();

    private Dictionary<AudioClip, float> sfxCalledTime = new Dictionary<AudioClip, float>();

    public string bgmPath;
    public string sfxPath;
    public string muteAllSFXExcept = "";

    private const int INIT_AUDIO_SOURCE_COUNT = 10;
    private const float VOLUME_LERP_TIME = 0.2f;

    private float bgmPlayTime = 0f;
    private float bgmVolume = 1f;
    private float sfxVolume = 1f;


    public override async UniTask ManagerInitProcessing()
    {
        await InitManager();

        await base.ManagerInitProcessing();
    }

    public override async UniTask InitManager()
    {
        bgmAudio = GetComponent<AudioSource>();

        await LoadSoundExAsync();

        await base.InitManager();
    }

    public void PlayBGM(AudioClip clip, bool isLoop = true)
    {
        //if (Core.DM.GetOptionData().isBgmOn == false)
        //    return;

        bgmAudio.clip = clip;
        bgmAudio.loop = isLoop;
        bgmAudio.Play();
    }

    public void PlayBGM(string sourceName, bool isLoop = true)
    {
        //if (Core.DM.GetOptionData().isBgmOn == false)
        //    return;

        if (dicBgmClips.ContainsKey(sourceName) == false)
        {
            Debug.LogWarning($"Bgm file does not exist: <color=red>{sourceName}</color>");
            return;
        }

        AudioClip clip = dicBgmClips[sourceName];

        PlayBGM(clip, isLoop);
    }

    public void StopBGM()
    {
        bgmAudio.loop = false;
        bgmAudio.Stop();
        bgmPlayTime = 0;
    }

    public void SetBGMVolume(float vol)
    {
        bgmAudio.volume = bgmVolume * vol;
    }

    public void PauseBGM()
    {
        bgmPlayTime = bgmAudio.time;
        bgmAudio.Stop();
    }

    public void ResumeBGM()
    {
        bgmAudio.time = bgmPlayTime;
        bgmAudio.Play();
    }

    public void PlaySFX(string sourceName, bool isLoop = false)
    {
        //if (Core.DM.GetOptionData().isSfxOn == false)
        //    return;

        if (dicSfxClips.ContainsKey(sourceName) == false)
        {
            Debug.LogError($"Sfx file does not exist: <color=red>{sourceName}</color>");
            return;
        }

#if UNITY_EDITOR
        if (string.IsNullOrEmpty(muteAllSFXExcept) == false && muteAllSFXExcept != sourceName) //사운드 픽업 테스트용
            return;
#endif

        AudioClip clip = dicSfxClips[sourceName];
        AudioSource source = GetAvailableSource();

        PlaySFX(source, clip, sfxVolume, isLoop);
    }

    public void PlaySFX(AudioSource source, AudioClip clip, float volume, bool isLoop = false)
    {
        //if (Core.DM.GetOptionData().isSfxOn == false)
        //    return;

        if (sfxCalledTime.ContainsKey(clip) == false)
            sfxCalledTime.Add(clip, 0);

        if (sfxCalledTime[clip] == Time.time)
            return;

        sfxCalledTime[clip] = Time.time;

        source.loop = isLoop;
        source.volume = volume;
        source.clip = clip;

        source.Play();
    }

    public void StopSFX(AudioSource source)
    {
        source?.Stop();
    }

    public void StopSFX(string sourceName)
    {
        for (int i = 0; i < sfxSources.Count; ++i)
        {
            if (sfxSources[i].isPlaying == false)
                continue;

            if (sfxSources[i].clip == null)
                continue;
            
            if (sfxSources[i].clip.name == sourceName)
                sfxSources[i].Stop();
        }
    }

    public void StopSFXAll()
    {
        for (int i = 0; i < sfxSources.Count; ++i)
        {
            if (sfxSources[i].isPlaying == false)
                continue;

            if (sfxSources[i].clip == null)
                continue;
            
            sfxSources[i].Stop();
        }
    }

    private async UniTask LoadSoundExAsync()
    {
        sfxSources = new List<AudioSource>();
        for (int i = 0; i < INIT_AUDIO_SOURCE_COUNT; i++)
            sfxSources.Add(gameObject.AddComponent<AudioSource>());

        var bgmClips = GetAudioClips(bgmPath);
        var sfxClips = GetAudioClips(sfxPath);

        for (int i = 0; i < bgmClips.Length; ++i)
            dicBgmClips.Add(bgmClips[i].name, bgmClips[i]);

        for (int i = 0; i < sfxClips.Length; ++i)
            dicSfxClips.Add(sfxClips[i].name, sfxClips[i]);

        await UniTask.CompletedTask;
    }

    public async UniTaskVoid PlaySoundSeriesAsync((string, bool)[] source_IsBgm)
    {
        for (int i = 0; i < source_IsBgm.Length; ++i)
        {
            if (source_IsBgm[i].Item2)
            {
                PlayBGM(source_IsBgm[i].Item1, i == source_IsBgm.Length - 1);
                await UniTask.Delay((int)(dicBgmClips[source_IsBgm[i].Item1].length * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
            }
            else
            {
                PlaySFX(source_IsBgm[i].Item1);
                await UniTask.Delay((int)(dicSfxClips[source_IsBgm[i].Item1].length * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
            }
        }
        
    }

    public async UniTaskVoid PlayBGMFadeAsync(float fadeDuration, string bgmAfter)
    {
        await LerpBGMVolumeAsync(0, fadeDuration, false);
        PlayBGM(bgmAfter);
        bgmAudio.volume = 0f;
        await LerpBGMVolumeAsync(bgmVolume, fadeDuration, false);
    }
    public void LerpBGMVolume(float toVolume, float duration, bool recoverAfter = false)
    {
        LerpBGMVolumeAsync(toVolume, duration, recoverAfter).Forget();
    }
    public void LerpBGMVolume(float toVolume, string duringSfx, bool recoverAfter = false)
    {
        LerpBGMVolume(toVolume, dicSfxClips[duringSfx].length, recoverAfter);
    }

    private async UniTask LerpBGMVolumeAsync(float toVolume, float duration, bool recoverAfter)
    {
        float timer = 0f;
        float initialVolume = bgmAudio.volume;
        float bgmVolume = initialVolume;
        float endTime = duration;

        endTime += recoverAfter ? VOLUME_LERP_TIME * 2 : VOLUME_LERP_TIME;

        while (timer < endTime)
        {
            timer += Time.unscaledDeltaTime;

            if (timer < VOLUME_LERP_TIME)
                bgmVolume = Mathf.Lerp(initialVolume, toVolume, timer / VOLUME_LERP_TIME);
            else if (recoverAfter && timer > duration + VOLUME_LERP_TIME)
                bgmVolume = Mathf.Lerp(toVolume, initialVolume, timer / endTime);

            if (bgmAudio != null)
                bgmAudio.volume = bgmVolume;

            await UniTask.Yield(cancellationToken: this.GetCancellationTokenOnDestroy());
        }
    }

    private AudioSource GetAvailableSource()
    {
        for (int i = 0; i < sfxSources.Count; i++)
        {
            if (sfxSources[i].isPlaying == false)
                return sfxSources[i];
        }

        int newIndexAt = sfxSources.Count;

        for (int i = 0; i < 5; ++i)
            sfxSources.Add(gameObject.AddComponent<AudioSource>());

        return sfxSources[newIndexAt];
    }

    private AudioClip[] GetAudioClips(string path)
    {
        return Resources.LoadAll<AudioClip>(path);
    }
}
