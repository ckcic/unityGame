using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip; // BGMのオーディオクリップ
    public float bgmVolume; // BGMの音量
    AudioSource bgmPlayer; 
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    public AudioClip[] sfxClips; // 効果音のオーディオクリップ
    public float sfxVolume; // 効果音の音量
    public int channels; // 効果音再生用
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx
    {
        Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win
    }

    private void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        // BGMプレーヤーの初期化
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 効果音プレーヤーの初期化
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int i = 0; i < channels; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].bypassListenerEffects = true;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    public void playBgm(bool isPlay)
    {
        if (isPlay) // trueの場合はBGMを再生し、falseの場合は停止
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void EffectBgm(bool isPlay) // エフェクトの有効化と無効化を制御
    {
        bgmEffect.enabled = isPlay;
    }

    public void playSfx(Sfx sfx) // 効果音の再生を制御
    {
        for(int i = 0;i < channels; i++)
        {
            int loopIndex =  (i + channelIndex) % channels;

            int ranIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee) // この効果音は2種類ある
            {
                ranIndex = Random.Range(0, 2);
            }

            if (sfxPlayers[loopIndex].isPlaying) { continue; }
            // 再生していないコンポーネントを見つけたら
            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[loopIndex].Play(); // 効果音を再生
            break;
        }

    }
}
