using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovieDataBase", menuName = "Movie/MovieDataBaseを作成")]

public class MovieDataBase : ScriptableObject
{
    [Header("チュートリアルに再生する映像を格納する")]
    public List<TutorialMovieData> tutorialMovieDatas = new List<TutorialMovieData>();

    //取得メソッドを用意する
    public TutorialMovieData GetTutorialMovie(TutorialType type)
    {
        return tutorialMovieDatas.Find(tutorialMovie => tutorialMovie.type == type);
    }

}
