using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ParticleDataBase", menuName = "Particle/ParticleDataBaseを作成")]

public class ParticleDataBase : ScriptableObject
{
    //リストに追加
    public List<ParticleData> particleDatas = new List<ParticleData>();

    //メソッドの取得
    public ParticleData GetParticleData(ParticleType type)
    {
        return particleDatas.Find(particle => particle.type == type);
    }
}
