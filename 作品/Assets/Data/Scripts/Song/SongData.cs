using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SongData", menuName = "Song/SongDataを作成")]

public class SongData : ScriptableObject
{
    public int songID; //曲ID
    public string songName; //曲名
    public string artistName; //アーティスト名
    public Sprite songImage; //曲画像

    [Header("難易度一覧")]
    public List<DifficultyData> difficultyDatas;

}
