
﻿using System.Collections.Generic;
﻿using UnityEngine;


public class CharacterAgent : LivingBaseAgent
{
    /// <summary>
    /// 角色基本属性信息
    /// </summary>
    private Character Character => living as Character;
    /// <summary>
    /// 怪物编号，用于加载对应怪物的属性信息
    /// </summary>
    public int characterIndex;
    /// <summary>
    /// 状态机
    /// </summary>
    enum ActionState
    {
        Normal,
        Attacking,
        Dashing,
        Skilling,
    }
    private ActionState actionState;


    /// <summary>
    /// 物品栏
    /// </summary>
    private Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;

    // movement
    float horizontal;
    float vertical;

    // dash realting
    private bool isDashBottonDown;
    [SerializeField] private LayerMask dashLayerMask;

    // animation

    Vector2 lookDirection = new Vector2(0, 0);

    // actions
    public GameObject WeaponPrefab;

    public Skill MissleAttack => living.Skills[0];
    public Skill MeleeAttack => living.Skills[1];

    float deltaTime = 0;
    private void Awake()
    {

        living = Global.characters[characterIndex];
        print(Character.Name);

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        MoveSpeed = Character.MoveSpeed;
        actionState = ActionState.Normal;

        // 初始化背包及UI，由于unity奇怪的机制，每次重启项目就要重新设置脚本。先注释掉调用UI代码
        inventory = new Inventory(UseItem);
        //uiInventory.SetInventory(inventory);
    }


    private void Update()
    {
        switch (actionState)
        {
            // when in Normal state, player can move, perform skills, get hurt ,attack and interact
            case ActionState.Normal:
                // move
                HandleMovement();

                // do special action such as dash and skills
                Perform();

                // interact with NPC or gears
                HandleInteraction();

                // do normal melee and missle attack
                HandleAttacking();
                break;

            // when in Attacking state, player should be unable to move or interact, but can perform skills, be hurt or do another attack
            case ActionState.Attacking:
                Perform();

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("MeleeAttack"))
                {
                    SetState(0);
                    Debug.Log("melee attack finish!");
                }

                break;

            // when in Skilling state, player could only wait until skill is over
            case ActionState.Skilling:
                break;
        }
        CheckState();
        deltaTime += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x += MoveSpeed * horizontal * Time.deltaTime;
        position.y += MoveSpeed * vertical * Time.deltaTime;
        rigidbody2d.MovePosition(position);

        Dash();
    }

    /// <summary>
    /// 捡起地图上的道具
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter2D(Collider2D collider)
    {
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestorySelf();
        }
    }

    /// <summary>
    /// 道具的使用效果在初始化背包时就被传入
    /// </summary>
    /// <param name="item"></param>
    public void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion: 
                Debug.Log("我回血啦，我nb了！");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
                break;
            case Item.ItemType.ManaPotion: 
                Debug.Log("我回蓝啦，我很有精神！");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 1 });
                break;

        }
    }

    /// <summary>
    /// 改变角色状态机的状态
    /// </summary>
    /// <param name="a"></param>
    public void SetState(int a)
    {
        switch (a)
        {
            case 0:
                actionState = ActionState.Normal;
                MoveSpeed = 10.0f;
                break;
            case 1:
                actionState = ActionState.Attacking;
                Stop();
                break;
            case 2:
                actionState = ActionState.Dashing;
                break;
            case 3:
                Stop();
                actionState = ActionState.Skilling;
                break;

            default:
                actionState = ActionState.Normal;
                break;
        }
    }

    private void HandleMovement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal, vertical);
        if (!Mathf.Approximately(move.x, 0.0f) || (!Mathf.Approximately(move.y, 0.0f)))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        // set moving animaion paraments
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

    }

    private void Stop()
    {
        MoveSpeed = 0.0f;
        lookDirection = Vector2.zero;
    }

    private void Dash()
    {
        if (isDashBottonDown)
        {
            SetState(2);
            float dashAmount = 5f;
            Vector2 dashPosition = rigidbody2d.position + lookDirection * dashAmount;
            RaycastHit2D dashHit = Physics2D.Raycast(rigidbody2d.position, lookDirection, dashAmount, dashLayerMask);
            if (dashHit.collider != null)
            {
                dashPosition = dashHit.point;
            }
            rigidbody2d.MovePosition(dashPosition);
            isDashBottonDown = false;
            SetState(0);
        }
    }

    private void Perform()
    {
        // to judge what special action should be performed
        // dash
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDashBottonDown = true;
            Debug.Log("Should perform dash!");
        }

        // skills
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetState(3);
            Debug.Log("Should perform skill1!");
            SetState(0);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            SetState(3);
            Debug.Log("Should perform skill2!");
            SetState(0);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SetState(3);
            Debug.Log("Should perform skill3!");
            SetState(0);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            SetState(3);
            Debug.Log("Should perform skill4!");
            SetState(0);
        }

    }


    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f,
            lookDirection, 1.5f, LayerMask.GetMask("Interactable"));
            if (hit.collider != null)
            {
                NonPlayerCharacter npc = hit.collider.GetComponent<NonPlayerCharacter>();
                Gear gear = hit.collider.GetComponent<Gear>();
                if (npc != null)
                {
                    npc.DisplayDialog();
                }
                if (gear != null)
                {
                    gear.Action();
                }
            }
        }
    }

    private void HandleAttacking()
    {
        Vector3 mousePosition = gameObject.GetComponent<PlayerAim>().GetMouseWorldPosition();
        living.AttackDirection = (mousePosition - transform.position).normalized;
        if (Input.GetMouseButtonDown(0))
        {
            if (living.AttackSpeed - deltaTime < 0.01)
            {
                SetState(1);
                deltaTime = 0;
                Stop();
                MissleAttack.Perform(this, null);
                Debug.Log("MissleAttack!");
                SetState(0);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (living.AttackSpeed - deltaTime < 0.01)
            {
                SetState(1);
                Stop();
                MeleeAttack.Perform(this, null);
                Debug.Log("MeleeAttack!");
                deltaTime = 0;
                animator.SetTrigger("Melee");
                SetState(0);
            }
        }
    }
}

