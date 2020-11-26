
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CharacterAgent : LivingBaseAgent
{
    /// <summary>
    /// 角色基本属性信息
    /// </summary>
    private Character Character => living as Character;
    /// <summary>
    /// 角色实际属性，受各种buff影响
    /// </summary>
    public Character ActualCharacter => actualLiving as Character;
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

    // Weapons
    public GameObject WeaponPrefab { get; set; }

    private bool godMode = false;

    float deltaTime = 0;



    private void Awake()
    {

        living = Global.characters[characterIndex].Clone() as Character;
        actualLiving = Global.characters[characterIndex].Clone() as Character;
        print(Character.Name);

        rigidbody2d = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();

        actionState = ActionState.Normal;
        ActualCharacter.State.AddStatus(new InvincibleState(), ActualCharacter.TimeInvincible);

        // 初始化背包及UI
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);

        UpdateWeaponPrefab();
        //living.MissleWeapon.bulletPrefab = bulletPrefab;


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

                if (Animator.GetCurrentAnimatorStateInfo(0).IsName("MeleeAttack"))
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
        position.x += ActualCharacter.MoveSpeed * horizontal * Time.deltaTime;
        position.y += ActualCharacter.MoveSpeed * vertical * Time.deltaTime;
        rigidbody2d.MovePosition(position);

        Dash();
    }

    public void UpdateWeaponPrefab()
    {
        try
        {
            WeaponPrefab = transform.GetChild(0).gameObject;
        }
        catch (System.Exception)
        {
            WeaponPrefab = null;
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
                ActualCharacter.MoveSpeed = Character.MoveSpeed;
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
        Animator.SetFloat("Look X", lookDirection.x);
        Animator.SetFloat("Look Y", lookDirection.y);
        Animator.SetFloat("Speed", move.magnitude);

    }

    private void Stop()
    {
        ActualCharacter.MoveSpeed = 0.0f;
        lookDirection = Vector2.zero;
    }

    private void Dash()
    {
        if (isDashBottonDown)
        {
            SetState(2);
            float dashAmount = 5f;
            Vector2 dashPosition = rigidbody2d.position + lookDirection * dashAmount;
            for (int i = 0; i < 1000; i++)
            {
                RaycastHit2D dashHit = Physics2D.Raycast(rigidbody2d.position + gameObject.GetComponent<BoxCollider2D>().offset + lookDirection * gameObject.GetComponent<BoxCollider2D>().size, lookDirection, dashAmount * i / 1000, dashLayerMask);
                if (dashHit.collider != null)
                {
                    dashPosition = dashHit.point;
                    break;
                }

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
            inventory.UseItem(new HealthPotion { Amount = 1 }, this);
            SetState(0);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            SetState(3);
            inventory.UseItem(new StrengthPotion { Amount = 1 }, this);
            SetState(0);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SetState(3);
            inventory.UseItem(new Medkit { Amount = 1 }, this);
            SetState(0);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            SetState(3);
            godMode = !godMode;
            Debug.Log("Should perform skill4!");
            SetState(0);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            List<Item> items = inventory.ItemList;
            Item weapon = items.FirstOrDefault(e => e is Weapon);
            if (weapon != null)
                inventory.UseItem(weapon, this);
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
        if (godMode)
        {
            SetState(1);
            deltaTime = 0;
            WeaponPrefab.GetComponent<WeaponAgent>().Attack();
            SetState(0);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (WeaponPrefab != null)
            {
                WeaponAgent weaponAgent = WeaponPrefab.GetComponent<WeaponAgent>();
                if (ActualCharacter.AttackSpeed * weaponAgent.Weapon.AttackSpeed - deltaTime < 0.01)
                {
                    SetState(1);
                    weaponAgent.Attack();
                    deltaTime = 0;
                    SetState(0);
                }
            }
        }
    }

    /// <summary>
    /// 人物与环境交互，如果碰撞物体实现了IInteract接口，调用对应的InteractWith函数实现人物与环境的交互
    /// </summary>
    /// <param name="collision">碰撞实体</param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform == this.transform)
        {
            return;
        }
        IInteract interact = Utility.GetInterface<IInteract>(collision.gameObject);
        if (interact != null)
        {
            interact.InteractWith(gameObject);
        }
    }

    public void InventoryAddItem(Item item)
    {
        inventory.AddItem(item);
    }


}

