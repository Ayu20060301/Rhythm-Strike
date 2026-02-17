using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NotesData", menuName = "Notes/NotesDataを作成")]

public class NotesData : ScriptableObject
{
    public int type; //ノーツタイプ
    public GameObject prefab; //生成するprefab
    public float y; //各ノーツのy座標
}
