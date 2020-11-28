using UnityEngine;

public class BossAgent : MonsterAgent
{
    protected int laserNum = 3;
    protected float laserInternal = 1f;
    protected float laserScope = 45;
    protected float thetaSpeed = 30;

    protected int barrageNum = 300;
    protected float barrageInternal = 0.04f;

    public Lasers GetLasers()
    {
        return new Lasers(this, laserNum, laserInternal, laserScope, thetaSpeed);
    }

    public Barrage GetBarrage()
    {
        return new Barrage(this, barrageNum, barrageInternal);
    }
}


