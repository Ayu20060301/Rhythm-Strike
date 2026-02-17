using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NotesDataBase", menuName = "Notes/NotesDataBaseを作成")]

public class NotesDataBase : ScriptableObject
{
    [Header("各ノーツを入れる")]
    public List<NotesData> noteDatas = new List<NotesData>();

    //メソッドの取得
    public NotesData GetNoteByData(int type)
    {
        return noteDatas.Find(n => n.type == type);
    }
}
