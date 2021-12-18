/****************************************************
    文件：EffectOfflineData.cs
    作者：TA94
    日期：2021/10/27 23:9:11
    功能：Nothing
*****************************************************/
using UnityEngine;

public class EffectOfflineData : OfflineData
{
    public ParticleSystem[] mParticle;
    public TrailRenderer[] mTrailRe;

    public override void BindData()
    {
        base.BindData();
        mParticle = gameObject.GetComponentsInChildren<ParticleSystem>();
        mTrailRe = gameObject.GetComponentsInChildren<TrailRenderer>();
    }

    public override void ResetProp()
    {
        base.ResetProp();
        foreach (var particle in mParticle)
        {
            particle.Clear(true);
            particle.Play();
        }

        foreach (var trail in mTrailRe)
        {
            trail.Clear();
        }
    }
}
