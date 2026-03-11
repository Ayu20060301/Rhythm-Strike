using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GameEndController : MonoBehaviour
{
    [SerializeField]
    private GameFlowManager _gameFlowManager;

    [SerializeField]
    private ComboTextUI _comboTextUI;

    private void Start()
    {
        GManager.instance.isEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
       
        //ゲーム未開始またはゲーム終了済みなら監視終了
        if (!GManager.instance.isStart || GManager.instance.isEnd) return;

        //曲終了
        if (IsSongEnd())
        {
            Debug.Log("終了");

            _comboTextUI.comboTextUI.text = " "; //コンボ数を非表示

            //終了フラグ
            GManager.instance.isEnd = true;

            //GameFlowManagerに終了を委譲
            _gameFlowManager.OnSongFinished();
            
        }
    }

    bool IsSongEnd()
    {
        var audio = MusicManager.instance;
        var source = audio.GetComponent<AudioSource>();

        if (source == null || source.clip == null) return false;

        float songLength = source.clip.length;
        float current = source.time;

        return current >= songLength - Time.deltaTime;
    }
}
