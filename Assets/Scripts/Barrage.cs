using UnityEngine;

public class Barrage
{
    private int barrageNum;
    private int nowBarrageNum;
    private float barrageInternal;
    private float nowBarrageInternal;
    private MonsterAgent agent;
    private Vector3 randomDirection;

    public Barrage(MonsterAgent agent, int barrageNum, float barrageInternal)
    {
        this.agent = agent;
        this.barrageNum = barrageNum;
        this.barrageInternal = barrageInternal;
        nowBarrageInternal = 0;
        nowBarrageNum = 0;
    }
    public bool Perform()
    {
        agent.rigidbody2d.velocity = Vector3.zero;
        RandomFireBall();
        if (nowBarrageNum >= barrageNum)
        {
            ClearBarrage();
            return true;
        }
        else
            return false;
    }

    public void RandomFireBall()
    {
        nowBarrageInternal += Time.deltaTime;
        if (nowBarrageInternal >= barrageInternal)
        {
            randomDirection = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            agent.ActualMonster.AttackSkill.AttackAt(randomDirection, null, agent);
            nowBarrageNum++;
            nowBarrageInternal = 0;
        }
    }

    public void ClearBarrage()
    {
        nowBarrageNum = 0;
        nowBarrageInternal = 0;
    }
}


