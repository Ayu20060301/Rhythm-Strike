using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : SingletonMonoBehaviour<ParticleManager>
{

    //DataBaseManagerから取得する
    private ParticleDataBase _particleDB => DataBaseManager.instance.particleDB;

    Dictionary<ParticleType, ParticleData> _particleDict = new();
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        //データベースを辞書に変換
        foreach(var data in _particleDB.particleDatas)
        {
            _particleDict[data.type] = data;
        }
    }


    //エフェクトの再生メソッド
    public void PlayParticle(ParticleType type,Vector3 position,Quaternion rotation)
    {

        //エフェクトOFFなら再生しない
        if (!GManager.instance.isEffectOn) return;

        if(_particleDict.TryGetValue(type,out var particleData) && particleData.particlePrefab != null)
        {
            //prefabから生成
            var particleInstance = Instantiate(particleData.particlePrefab, position, rotation);

            //ParticleSystemを取得して再生
            var ps = particleInstance.GetComponent<ParticleSystem>();
            if(ps != null)
            {
                ps.Play();

                //自動破棄する場合
                Destroy(particleInstance, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                Debug.LogWarning($"ParticleType {type}が見つかりません");
            }


        }
    }


}
