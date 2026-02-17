using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÉLÅ[ì¸óÕ
/// </summary>
public class InputHandler : MonoBehaviour
{
    public Action<int> OnLaneInput;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) OnLaneInput?.Invoke(0);
        if (Input.GetKeyDown(KeyCode.F)) OnLaneInput?.Invoke(1);
        if (Input.GetKeyDown(KeyCode.J)) OnLaneInput?.Invoke(2);
        if (Input.GetKeyDown(KeyCode.K)) OnLaneInput?.Invoke(3);
    }
}
