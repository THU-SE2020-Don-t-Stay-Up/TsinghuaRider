
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    private Inventory weaponColumn;
    private Inventory buffColumn;
    private UIInventory uiInventory;
    private UIInventory uiWeaponColumn;
    private UIInventory uiBuffColumn;

    // movement
    float horizontal;
    float vertical;

    // dash realting
    private bool isDashBottonDown;
    [SerializeField] private LayerMask dashLayerMask;
    public int dashBar;     // 为了测试改成了public

    // animation

    Vector2 lookDirection = new Vector2(0, 0);

    // Weapons
    public GameObject WeaponPrefab { get; set; }

    private bool attackingFlag = false;

    float deltaTime = 0;

    Vector3 attackDirection = new Vector3(0, 0, 0);
    Vector3 mousePosition = new Vector3(0, 0, 0);

    // 为了测试改成public，之后要改回private
    public void Awake()
    {

        living = Global.characters[characterIndex].Clone() as Character;
        actualLiving = Global.characters[characterIndex].Clone() as Character;
        print(Character.Name);

        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        Animator = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();

        actionState = ActionState.Normal;
        ActualCharacter.State.AddStatus(new InvincibleState(), ActualCharacter.TimeInvincible);

        // 初始化背包及其UI
        uiInventory = GameObject.Find("UI_Inventory").GetComponent<UIInventory>();
        inventory = new Inventory();
        inventory.AddItem(new HealthPotion { Amount = 3 });
        inventory.AddItem(new StrengthPotion { Amount = 4 });
        inventory.AddItem(new Medkit { Amount = 1 });
        uiInventory.SetInventory(inventory);


        uiWeaponColumn = GameObject.Find("UI_Weapons").GetComponent<UIInventory>();
        weaponColumn = new Inventory();
        weaponColumn.AddItem(new Sword { Amount = 1 });
        weaponColumn.AddItem(new Gun { Amount = 1 });
        uiWeaponColumn.SetInventory(weaponColumn);

        uiBuffColumn = GameObject.Find("UI_Buffs").GetComponent<UIInventory>();
        buffColumn = new Inventory();
        uiBuffColumn.SetInventory(buffColumn);


        UpdateWeaponPrefab();
    }

    // 为了测试改成public，之后要改回private
    public void Update()
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

        HandleDirection();
        CheckState();
        deltaTime += Time.deltaTime;

        dashBar = Mathf.Clamp(dashBar + 1, 0, 901);
        UIDashBar.instance.SetValue((float)dashBar / 901f);
        if (dashBar % 300 == 0)
        {
            Debug.Log("老子dash回复了一次");
        }

    }

    // 为了测试改成public，之后要改回private
    public void FixedUpdate()
    {
        rigidbody2d.velocity = new Vector2(horizontal, vertical) * ActualCharacter.MoveSpeed;
        //rigidbody2d.AddForce(new Vector2(horizontal, vertical) * ActualCharacter.MoveSpeed);

        Dash();
    }

    public void UpdateWeaponPrefab()
    {
        try
        {
            WeaponPrefab = transform.GetComponentInChildren<WeaponAgent>().gameObject;
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
                living.State.AddStatus(new InvincibleState(), 0.5f);
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

        Animator.SetFloat("Speed", move.magnitude);
    }

    private void HandleDirection()
    {
        mousePosition = WeaponAgent.GetMouseWorldPosition();
        attackDirection = (mousePosition - transform.position).normalized;
        Animator.SetFloat("Look X", attackDirection.x);
        Animator.SetFloat("Look Y", attackDirection.y);
    }

    private void Stop()
    {
        ActualCharacter.MoveSpeed = 0.0f;
        //lookDirection = Vector2.zero;
    }

    private void Dash()
    {
        if (isDashBottonDown && dashBar >= 300)
        {
            SetState(2);
            float dashAmount = 3f;
            //Vector2 dashPosition = rigidbody2d.position + new Vector2(attackDirection.x,attackDirection.y) * dashAmount;
            //RaycastHit2D dashHit = Physics2D.Raycast(rigidbody2d.position + gameObject.GetComponent<BoxCollider2D>().offset + attackDirection * gameObject.GetComponent<BoxCollider2D>().size, attackDirection, dashAmount, dashLayerMask);
            Vector2 dashPosition = rigidbody2d.position + lookDirection * dashAmount;
            RaycastHit2D dashHit = Physics2D.Raycast(rigidbody2d.position + gameObject.GetComponent<BoxCollider2D>().offset + lookDirection* gameObject.GetComponent<BoxCollider2D>().size, lookDirection, dashAmount, dashLayerMask);
            if (dashHit.collider != null)
            {
                Debug.Log("撞墙了woc！");
                dashPosition = dashHit.point;
            }

            rigidbody2d.MovePosition(dashPosition);
            dashBar -= 300;
            UIDashBar.instance.SetValue((float)dashBar / 901f);
            Debug.Log("我闪！");
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
            if (dashBar >= 300)
            {
                isDashBottonDown = true;
                Debug.Log("Should perform dash!");
            }
            else
            {
                isDashBottonDown = false;

                Debug.Log("Dash CD中");
            }
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
            attackingFlag = !attackingFlag;
            Debug.Log("God Mode!");
            SetState(0);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            List<Item> items = weaponColumn.ItemList;
            Item weapon = items.FirstOrDefault(e => e is Weapon);
            if (weapon != null)
                weaponColumn.UseItem(weapon, this);
            SetState(0);
        }

    }

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f,
            attackDirection, 1.5f, LayerMask.GetMask("Interactable"));
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
        if (attackingFlag)
        {
            SetState(1);
            deltaTime = 0;
            attackingFlag = !WeaponPrefab.GetComponent<WeaponAgent>().Attack();
            SetState(0);
        }
        //鼠标左键
        if (Input.GetMouseButtonDown(0))
        {
            //attackingFlag = true;
            //Debug.Log(WeaponPrefab);
            if (WeaponPrefab != null)
            {
                WeaponAgent weaponAgent = WeaponPrefab.GetComponent<WeaponAgent>();
                if (ActualCharacter.AttackSpeed * weaponAgent.Weapon.AttackSpeed - deltaTime < 0.01)
               // if (true)
                 {
                    SetState(1);
                    attackingFlag = !weaponAgent.Attack();
                    deltaTime = 0;
                    SetState(0);
                }
            }
            else
            {
                if (ActualCharacter.AttackSpeed - deltaTime < 0.01)
                {
                    SetState(1);
                    MeleeAttack();
                    deltaTime = 0;
                    SetState(0);
                }
            }
        }
        //鼠标右键，使用刀光攻击
        else if (Input.GetMouseButtonDown(1))
        {
            //attackingFlag = true;
            //Debug.Log(WeaponPrefab);
            if (WeaponPrefab != null)
            {
                WeaponAgent weaponAgent = WeaponPrefab.GetComponent<WeaponAgent>();
                if (ActualCharacter.AttackSpeed * weaponAgent.Weapon.AttackSpeed - deltaTime < 0.01)
                // if (true)
                {
                    SetState(1);
                    attackingFlag = !weaponAgent.SwordLightAttack();
                    deltaTime = 0;
                    SetState(0);
                }
            }
            else
            {//没有武器时不响应
                
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

    /// <summary>
    /// 如果武器不在武器栏内，返回false；否则返回true
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool HasWeapon(Item item)
    {
        return weaponColumn.HasItem(item);
    }

    public void WeaponColumnAddItem(Item item)
    {
        weaponColumn.AddItem(item);
    }

    public void InventoryAddItem(Item item)
    {
        inventory.AddItem(item);
    }

    public void BuffColumnAddItem(Item item)
    {
        buffColumn.AddItem(item);
    }

    public void MeleeAttack()
    {
        Animator.SetTrigger("Melee");
       
        IEnumerable<GameObject> targetObjects = GetAttackRangeObjects(transform.position, attackDirection, ActualCharacter.AttackRadius, ActualCharacter.AttackAngle, "Monster");
        foreach (var targetObject in targetObjects)
        {
            LivingBaseAgent targetAgent = targetObject.GetComponent<LivingBaseAgent>();
            targetAgent.ChangeHealth(-ActualCharacter.AttackAmount);
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public override void ChangeHealth(float amount)
    {
        if (amount < 0)
        {
            if (actualLiving.State.HasStatus(new InvincibleState()))
            {
                Debug.Log($"{actualLiving.Name} Invincible");
                return;
            }

            else
            {
                actualLiving.CurrentHealth = (int)Mathf.Clamp(actualLiving.CurrentHealth + amount, 0, actualLiving.MaxHealth);
                UIHealthBar.instance.SetValue(actualLiving.CurrentHealth / (float)actualLiving.MaxHealth);

                Debug.Log($"{actualLiving.Name} now health is {actualLiving.CurrentHealth}");
                //animator.SetTrigger("Hit");
                //audioSource.PlayOneShot(getHitClip);
                actualLiving.State.AddStatus(new InvincibleState(), actualLiving.TimeInvincible);
                //print($"{actualLiving.Name}获得无敌{actualLiving.TimeInvincible}");
                if (IsDead())
                {
                    if (Interlocked.Exchange(ref actualLiving.isDead, 1) == 0)
                    {
                        //死亡动画
                        Destroy();
                    }
                }
            }
        }
        else
        {
            //animator.SetTrigger("Heal");
            //audioSource.PlayOneShot(getHealingClip);

            actualLiving.CurrentHealth = (int)Mathf.Clamp(actualLiving.CurrentHealth + amount, 0, actualLiving.MaxHealth);
            UIHealthBar.instance.SetValue(actualLiving.CurrentHealth / (float)actualLiving.MaxHealth);


        }
    }

    /// <summary>
    /// 用于测试Character使用Item的函数，最终要删除
    /// </summary>
    /// <param name="item"></param>
    public void UseItem(Item item)
    {
        inventory.UseItem(item, this);
    }
    public bool IsInventoryEmpty()
    {
        return inventory.IsEmpty();
    }
    public int GetItemAmount(Item item)
    {
        return inventory.GetItemAmount(item);
    }
    public void CleanInventory()
    {
        inventory.Clean();
    }


    public void UseBuffItem(Item item)
    {
        buffColumn.UseItem(item, this);
    }
    public int GetBuffItemAmount(Item item)
    {
        return buffColumn.GetItemAmount(item);
    }
    public void CleanBuffColumn()
    {
        buffColumn.Clean();
    }

    public void SwapeWeapon()
    {
        List<Item> items2 = weaponColumn.ItemList;
        Item weapon = items2.FirstOrDefault(e => e is Weapon);
        if (weapon != null)
            weaponColumn.UseItem(weapon, this);
    }
    public void CleanWeaponColumn()
    {
        weaponColumn.Clean();
    }
    public int GetWeaponAmount(Item item)
    {
        return weaponColumn.GetItemAmount(item);
    }
    public bool IsWeaponColumnEmpty()
    {
        return weaponColumn.IsEmpty();
    }


    public void ForceDash()
    {
        lookDirection = new Vector2(1, 0);
        isDashBottonDown = true;
        Dash();
    }
    public void ClearDashBar()
    {
        dashBar = 0;
    }
    // 以上的函数测试使用

}

