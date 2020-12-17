using UnityEngine;

public class BossAgent : MonsterAgent
{
    protected int laserNum = 3;
    protected float laserInternal = 1f;
    protected float laserScope = 45;
    protected float thetaSpeed = 30;
    public AudioClip fireAudioClip;
    protected int barrageNum = 200;
    protected float barrageInternal = 0.05f;

    public Lasers GetLasers()
    {
        return new Lasers(this, laserNum, laserInternal, laserScope, thetaSpeed);
    }

    public Barrage GetBarrage()
    {
        return new Barrage(this, barrageNum, barrageInternal);
    }
}


