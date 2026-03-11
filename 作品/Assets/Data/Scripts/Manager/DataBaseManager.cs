using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ゲーム全体で使うデータベースを一括管理するクラス
/// </summary>
public class DataBaseManager : SingletonMonoBehaviour<DataBaseManager>
{
    [Header("各種データベースを入れる")]
    public NotesDataBase noteDataBase; //ノーツ関連のデータ
    public SoundDataBase soundDataBase; //サウンド関連のデータ
    public UIDataBase uiDataBase; //UI関連のデータ
    public SongDataBase songDataBase; //曲関連のデータ
    public ParticleDataBase particleDataBase; //パーティクル関連のデータ
    public MovieDataBase movieDataBase; //Movie関連のデータ

    //外部からのプロパティ読み込み可能
    public NotesDataBase noteDB => noteDataBase;
    public SoundDataBase soundDB => soundDataBase;
    public UIDataBase uiDB => uiDataBase;
    public SongDataBase songDB => songDataBase;
    public ParticleDataBase particleDB => particleDataBase;
    public MovieDataBase movieDataDB => movieDataBase;

    protected override void Awake()
    {
        base.Awake();
    }
}
