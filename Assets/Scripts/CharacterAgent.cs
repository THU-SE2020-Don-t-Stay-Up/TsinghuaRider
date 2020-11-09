using UnityEngine;
using Debug = UnityEngine.Debug;

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

    // movement

    float horizontal;
    float vertical;
    // dash realting
    private bool isDashBottonDown;

    [SerializeField] private LayerMask dashLayerMask;

    // animation

    Vector2 lookDirection = new Vector2(1, 0);

    // actions
    public GameObject WeaponPrefab;

    public Skill MissleAttack => living.Skills[0];
    public Skill MeleeAttack => living.Skills[1];

    float deltaTime = 0;
    private void Awake()
    {
        Global.characters = Character.LoadCharacter();

        living = Global.characters[characterIndex];
        print(Character.Name);

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        MoveSpeed = Character.MoveSpeed;
        actionState = ActionState.Normal;
    }

    private void Update()
    {
        switch (actionState)
        {
            // when in Normal state, player can move, perform skills, get hurt ,attack and interact
            case ActionState.Normal:
                // move
                MoveSpeed = 10.0f;
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
                HandleAttacking();
                Perform();
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

    public void SetState(int a)
    {
        switch (a)
        {
            case 0:
                actionState = ActionState.Normal;
                break;
            case 1:
                actionState = ActionState.Attacking;
                break;
            case 2:
                actionState = ActionState.Dashing;
                break;
            case 3:
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
            Stop();
            Debug.Log("Should perform skill1!");
            SetState(0);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            SetState(3);
            Stop();
            Debug.Log("Should perform skill2!");
            SetState(0);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SetState(3);
            Stop();
            Debug.Log("Should perform skill3!");
            SetState(0);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            SetState(3);
            Stop();
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
        SetState(1);
        Vector3 mousePosition = gameObject.GetComponent<PlayerAim>().GetMouseWorldPosition();
        living.AttackDirection = (mousePosition - transform.position).normalized;
        if (Input.GetMouseButtonDown(0))
        {
            if (living.AttackSpeed - deltaTime < 0.01)
            {
                deltaTime = 0;
                Stop();
                MissleAttack.Perform(this, null);
                Debug.Log("MissleAttack!");
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (living.AttackSpeed - deltaTime < 0.01)
            {
                Stop();
                MeleeAttack.Perform(this, null);
                Debug.Log("MeleeAttack!");
                deltaTime = 0;
            }
        }
        SetState(0);
    }
}

