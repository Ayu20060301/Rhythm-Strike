using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameExit : MonoBehaviour
{
    
    /// <summary>
    /// アプリケーションを閉じる関数
    /// </summary>
    public void Exit()
    {
        Debug.Log("アプリケーションを閉じる");
        Application.Quit();
    }
}
