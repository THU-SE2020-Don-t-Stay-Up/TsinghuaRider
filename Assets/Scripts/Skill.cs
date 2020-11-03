using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Analytics;

abstract public class Skill
{
    public abstract void Perform(int Amount, GameObject target);
}


public class AttackSkill : Skill
{
    public override void Perform(int Amount, GameObject targetObjet)
    {
        Character_Robot target = targetObjet.GetComponent<Character_Robot>();
        if (target == null)
        {
            // 异常处理
        }
        target.ChangeHealth(-Amount);
        //Vector2 position = new Vector2(o.transform.position.x, o.transform.position.y);
    }
}

public class SplitSkill : Skill
{
    public override void Perform(int Anount, GameObject o)
    {
        Transform position = o.transform;
    }
}
