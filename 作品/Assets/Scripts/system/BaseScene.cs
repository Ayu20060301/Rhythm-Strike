using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーンを呼び出すクラス
/// </summary>
/// 
public class BaseScene : MonoBehaviour
{
        protected void Awake()
        {
            SceneController.instance.currentScene = this;
        }
}
