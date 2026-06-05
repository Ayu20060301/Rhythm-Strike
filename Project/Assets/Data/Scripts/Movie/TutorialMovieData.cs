using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialType
{
    NOTES, //ノーツ
    COINNOTES, //コインノーツ
    JUMPNOTES, //ジャンプノーツ
    DAMAGENOTES //ダメージノーツ
}

[CreateAssetMenu(fileName = "TutorialMovieData" , menuName = "Movie/TutorialMovieDataを作成")]

public class TutorialMovieData : ScriptableObject
{
    public TutorialType type;

    [Header("Movie")]
    public string moviePath; //StreamingAssetsからの相対パス
    public bool loop = true; //映像はループさせる

    [Header("Text")]
    public string descriptionName; //チュートリアルの説明を文字列で表す

    public string tutorialDisplayName;　//「このチュートリアルは何か」を文字列で表す

}
