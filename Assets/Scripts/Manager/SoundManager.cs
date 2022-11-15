using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectSoundType
{
    hi,
    h
}

public enum PlayerSoundType
{
    Attak1,
    Attak2,
    Attak3,
    Jump1,
    Jump2,
    Jump3,
    Die
}
[System.Serializable]
public class EffectSound
{
    public EffectSoundType Enum;
    public AudioClip clip;
}
[System.Serializable]
public class PlayerSound
{
    public PlayerSoundType Enum;
    public AudioClip clip;
}

public class SoundManager : Singleton<SoundManager>
{
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    [SerializeField] private AudioSource[] effectAudios;
    [SerializeField] private AudioSource bgmAudio;
    [SerializeField] private AudioSource playerAudio;
    [SerializeField] private EffectSound[] effectSounds;
    [SerializeField] private PlayerSound[] playerSounds;
    [SerializeField] private AudioClip[] bgmClips;
    /// <summary>
    /// 3D EffectSound ��� (�ڽ��� AudioSOurce�� ������ ���� ���)
    /// </summary>
    public void EffectPlay(AudioSource _au, EffectSoundType effectSound)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (effectSounds[i].Enum == effectSound)
            {
                _au.clip = effectSounds[i].clip;
                _au.Play();
                return;
            }

        }
        Debug.Log(effectSound + "����X");
    }
    /// <summary>
    /// 2D EffectSound ���
    /// </summary>
    public void EffectPlay(EffectSoundType effectSound)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (effectSounds[i].Enum == effectSound)
            {
                for (int j = 0; j < effectSounds.Length; j++)
                {
                    if (!effectAudios[j].isPlaying)
                    {
                        effectAudios[j].clip = effectSounds[i].clip;
                        effectAudios[j].Play();
                        return;
                    }
                }
                Debug.Log("��� EffectAudioSound�����");
                return;
            }

        }
        Debug.Log(effectSound + "����X");
    }
    /// <summary>
    /// �÷��̾� Voice
    /// </summary>
    public void PlayerPlay(PlayerSoundType PlayerSound)
    {
        for (int i = 0; i < playerSounds.Length; i++)
        {
            if (playerSounds[i].Enum == PlayerSound)
            {
                playerAudio.clip = playerSounds[i].clip;
                playerAudio.Play();
                return;
            }

        }
        Debug.Log(PlayerSound + "����X");
    }
    public void BgmPlay(int index)
    {
        bgmAudio.clip = bgmClips[index];
        bgmAudio.Play();
    }
}