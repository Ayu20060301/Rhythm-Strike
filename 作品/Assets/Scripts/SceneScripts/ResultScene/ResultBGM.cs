using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultBGM : MonoBehaviour
{
    //BGM‚ğÄ¶‚·‚é
    public void BGMStart()
    {
        BGMManager.instance.SetBGMState(BGMType.RESULT);
    }

    //BGM‚ğ~‚ß‚é
    public void StopBGM()
    {
        BGMManager.instance.BGMStop();
    }


}
