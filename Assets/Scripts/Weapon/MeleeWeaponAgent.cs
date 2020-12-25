using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponAgent : WeaponAgent, IInteract
{
    private float timer = 0f;
    private float angle = 0f;
    private float backAngle = 60f;
    private float InitAngle { get; set; }
    private bool initFlag = false;
    
    private void Rotate()
    {
        angle -= Time.deltaTime * 1000f;
    }

    private void Init()
    {
        if (!initFlag)
        {
            InitAngle = GetCurrentAngle();
            initFlag = true;
            angle = Weapon.AttackAngle/2;
            HandleAiming(InitAngle , angle, leftFlag);
            CharacterAgent agent = GameObject.Find("CharacterLoader").GetComponent<CharacterLoader>().player.GetComponent<CharacterAgent>();
            agent.AudioSource.PlayOneShot(agent.MeleeAttackClip);
        }
    }

    /// <summary>
    /// 完成一次近战攻击，返回true；近战攻击未完成，返回false
    /// </summary>
    /// <returns></returns>
    public override bool Attack()
    {
        state = State.MeleeAttack;
        Init();
        if (angle >= -Weapon.AttackAngle/2)
        {
            Rotate();
            HandleAiming(InitAngle, angle, leftFlag);
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
            if (agent != null && agent.name != user.name)
            {
                //Debug.Log("给爷死！");
                agent.ChangeHealth(-user.actualLiving.AttackAmount * Weapon.AttackAmount);
            }
        }
    }

    /// <summary>
    /// 近战武器只根据人物朝向左右翻转sprite
    /// </summary>
    protected override void HandlePosition()
    {
        //Debug.Log("角色编号：");
        //Debug.Log(user.characterIndex);
        if (user.characterIndex == 0 || user.characterIndex ==1 )
        {
            //Vector3 weaponPosition = transform.position;

            if (!leftFlag && aimDir.x < 0)
            {
                leftFlag = true;
                Vector3 scale = transform.localScale;
                scale.y = -scale.y;
                transform.localScale = scale;
            }
            else if (leftFlag && aimDir.x >= 0)
            {
                leftFlag = false;
                Vector3 scale = transform.localScale;
                scale.y = -scale.y;
                transform.localScale = scale;

            }
        }

        //else if (user.characterIndex == 1)
        //{
        //    Vector3 weaponPosition = transform.position;

        //    if (upFlag && aimDir.y <= 0)
        //    {

        //        upFlag = false;
        //        // Vector3 scale = transform.localScale;
        //        //scale.y = -scale.y;
        //        //transform.localScale = scale;
        //    }
        //    else if (!upFlag && aimDir.y > 0)
        //    {

        //        upFlag = true;
        //        //Vector3 scale = transform.localScale;
        //        //scale.y = -scale.y;
        //        //transform.localScale = scale;
        //    }

        //    if (!leftFlag && aimDir.x < 0)
        //    {
        //        leftFlag = true;
        //        float deltaX = transform.position.x - user.GetPosition().x;
        //        transform.position = Vector3.MoveTowards(weaponPosition, new Vector3(user.GetPosition().x - deltaX, weaponPosition.y), 10000f);
        //        Weapon.offset = new Vector3(-1, 1, 0);

        //    }
        //    else if (leftFlag && aimDir.x >= 0)
        //    {
        //        leftFlag = false;
        //        float deltaX = transform.position.x - user.GetPosition().x;

        //        transform.position = Vector3.MoveTowards(weaponPosition, new Vector3(user.GetPosition().x - deltaX, weaponPosition.y), 10000f);
        //        Weapon.offset = new Vector3(1, 1, 0);
        //    }
        //}
        //else
        //{

        //}
    }
    /// <summary>
    /// 以下的函数为了测试使用
    /// </summary>
    public bool TestAttack()
    {
        InitAngle = GetCurrentAngle();
        initFlag = true;
        HandleAiming(InitAngle, backAngle, leftFlag);

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
}
