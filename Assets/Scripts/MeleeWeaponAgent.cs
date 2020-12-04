using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponAgent : WeaponAgent, IInteract
{
    private float timer = 0f;
    private float angle = 0f;
    private float backAngle = 20f;
    private float InitAngle { get; set; }
    private bool initFlag = false;
    
    private void Rotate()
    {
        angle += Time.deltaTime * 500f;
    }

    private void Init()
    {
        if (!initFlag)
        {
            InitAngle = GetCurrentAngle();
            initFlag = true;
            HandleAiming(InitAngle , backAngle, leftFlag);
        }
    }

    public override bool Attack()
    {
        state = State.MeleeAttack;
        Init();
        if (angle <= Weapon.AttackAngle)
        {
            Rotate();
            HandleAiming(InitAngle, backAngle - angle, leftFlag);
        }
        else
        {
            state = State.Normal;
            timer = 0;
            angle = 0;
            initFlag = false;
            return true;
        }
        timer += Time.deltaTime;
        return false;
    }

    private void OnTriggerStay2D(Collider2D gameObject)
    {

        if (transform.parent != null && initFlag == true)
        {

            LivingBaseAgent agent = gameObject.GetComponent<LivingBaseAgent>();
            if (agent != null)
            {
                Debug.Log("给爷死！");
                agent.ChangeHealth(-user.actualLiving.AttackAmount * Weapon.AttackAmount);
            }
        }
    }

}
