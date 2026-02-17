using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SongData", menuName = "Song/SongDataを作成")]

public class SongData : ScriptableObject
{
    public int songID; //曲ID
    public string levelName; //レベル
    public Color levelColor; //レベルの色
    public string songName; //曲名
    public string artistName; //アーティスト名
    public int bpm; //BPM
    public Sprite songImage; //曲画像
}
