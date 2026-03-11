using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _messagePrefab;

    public void ShowMessage(JudgeType type,int lane)
    {
        int index = ConvertTypeToIndex(type);

        if (index < 0 || index >= _messagePrefab.Length) return;

        float laneX = lane - 1.5f;
        Vector3 pos = new Vector3(laneX, 0.76f, 0.15f);

        Instantiate(_messagePrefab[index], pos, Quaternion.Euler(45, 0, 0));
    }

    private int  ConvertTypeToIndex(JudgeType type)
    {
        switch(type)
        {
            case JudgeType.CRITICAL: return 0;
            case JudgeType.HIT: return 1;
            case JudgeType.ATTACK: return 2;
            case JudgeType.MISS: return 3;
            default: return -1;
        }
    }


}
