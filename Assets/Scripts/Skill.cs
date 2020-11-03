using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Analytics;

abstract public class Skill
{
    public abstract void Perform(GameObject o);
}


public class AttackSkill : Skill
{
    public override void Perform(GameObject o)
    {
        Transform position = o.transform;
    }
}

public class SplitSkill : Skill
{
    public override void Perform(GameObject o)
    {
        Transform position = o.transform;
    }
}
