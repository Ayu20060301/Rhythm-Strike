using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDataBase", menuName = "Sound/SoundDataBaseを作成")]


public class SoundDataBase : ScriptableObject
{
    [Header("効果音を入れる")]
    public List<SoundEffectData> soundEffectDatas = new List<SoundEffectData>();

    [Header("BGMを入れる")]
    public List<BGMData> bgmDatas = new List<BGMData>();

    //取得メソッドを用意する
    public SoundEffectData GetSoundEffect(SoundEffectType type)
    {
        return soundEffectDatas.Find(soundEffect => soundEffect.type == type);
    }

    public BGMData GetBGM(BGMType type)
    {
        return bgmDatas.Find(bgmDatas => bgmDatas.type == type);
    }

}
