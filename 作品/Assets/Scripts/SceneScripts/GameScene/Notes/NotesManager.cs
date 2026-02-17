using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //Linq使用

//JSONファイルのデータ構造
[Serializable]
public class Data
{
    public string name;  //JSONファイル名
    public int maxBlock; //最大ブロック数
    public int BPM;      //BPM
    public int offset;   //オフセット
    public Note[] notes; //ノーツ配列
}

//ノーツ一つ分の情報
[Serializable]
public class Note
{
    public int type;   //ノーツのタイプ
    public int num;    //ノーツの番号
    public int block;  //レーンの番号
    public int LPB;    //1小節あたりのノーツ数
}

/// <summary>
/// ノーツオブジェクト管理クラス
/// JSONファイル読み込みとノーツ生成を担当
/// </summary>
public class NotesManager : MonoBehaviour
{

    private SongDataBase _songDB => DataBaseManager.instance.songDB;

    //ノーツ関連データ
    public int noteNum; //総ノーツ数
    public int singleNoteNum; //単体ノーツ数 
    public int coinNoteNum; //コインノーツ数

    public List<int> laneNum = new List<int>(); //何番のレーンに落ちてくるか
    public List<int> noteType = new List<int>(); //何ノーツか
    public List<float> notesTime = new List<float>(); //ノーツが判定線と重なる時間
    public List<GameObject> notesObj = new List<GameObject>(); //ノーツオブジェクト

    //内部用
    private string _songName;//JSONファイルを入れる変数
    private float _notesSpeed;  //ノーツ速度

    [SerializeField]
    private ScoreManager _scoreManager;
    [SerializeField]
    private ComboManager _comboManager;

    //初期化処理
    void Start()
    {
        _notesSpeed = GManager.instance.notesSpeed * 2; //ノーツスピード
        noteNum = 0;
        singleNoteNum = 0;
        coinNoteNum = 0;
        _songName = _songDB.songData[GManager.instance.songID].songName;//読み込むJSONファイル名
        Load(_songName); //Load()を呼び出す
    }

    /// <summary>
    /// JSONファイルからノーツデータを読み込み、ノーツを生成する
    /// </summary>
    /// <param name="songName">ロードする曲の名前</param>
    private void Load(string songName)
    {
        // 初期化
        laneNum.Clear();
        noteType.Clear();
        notesTime.Clear();
        notesObj.Clear();
      
        TextAsset ta = Resources.Load<TextAsset>(songName);
        if (ta == null)
        {
            Debug.LogError($"NotesManager: Resources に '{songName}' が見つかりません。ファイル名を確認してください（空白や末尾記号など）。");
            return;
        }

        Data inputJson;
        try
        {
            inputJson = JsonUtility.FromJson<Data>(ta.text);
        }
        catch (Exception e)
        {
            Debug.LogError($"NotesManager: JSON パースエラー: {e.Message}");
            return;
        }
        if (inputJson == null || inputJson.notes == null)
        {
            Debug.LogError("NotesManager: JSON のパースに失敗 または notes が null です。");
            return;
        }

        noteNum = inputJson.notes.Length;
        singleNoteNum = inputJson.notes.Count(noteNum => noteNum.type == 1);
        coinNoteNum = inputJson.notes.Count(noteNum => noteNum.type == 4);

        //スコアの最大値を設定
        _scoreManager.SetMaxScore(singleNoteNum * 5 + coinNoteNum * 5);

        //最大コイン数を設定
        _scoreManager.SetMaxCoin(coinNoteNum);


        float secPerBeat = 60.0f / inputJson.BPM; //1拍
        float secPerBar = secPerBeat * 4.0f; //1小節(4拍)

        foreach(var note in inputJson.notes)
        {
            var noteData = DataBaseManager.instance.noteDataBase.GetNoteByData(note.type);
            if(noteData == null)
            {
                Debug.LogWarning($"NoteDataが見つかりません type={note.type}");
                continue;
            }

            //time = (番号 / LPB) * 小節の長さ + offset
            float time =
              note.num * (60.0f / inputJson.BPM / note.LPB)
              + inputJson.offset * 0.001f;

            //リストに追加
            laneNum.Add(note.block);
            noteType.Add(note.type);
            notesTime.Add(time);

            //ノーツのZ座標(時間 × 速度)
            float z = time * _notesSpeed;

            var obj = Instantiate(
                noteData.prefab,
                new Vector3(note.block - 1.5f, noteData.y, z),
                noteData.prefab.transform.rotation
                );

            notesObj.Add(obj);
        }
    }
} 
