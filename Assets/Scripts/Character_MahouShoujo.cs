using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Character_MahouShoujo : MonoBehaviour
{
    enum State
    {
        Normal,
        Attacking,
        Dashing,
        Skilling,
    }

    private State state;

    private GameObject aiming;

    // set character's speed
    public float speed = 10.0f;

    // health & invincible time
    public int maxHealth = 5;
    private int currentHealth;
    public int health { get { return currentHealth; } }
    
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    // magic setting
    public int maxMagic = 5;
    private int currentMagic;
    public int magic { get { return currentMagic; } }

    // movement
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    // dash realting
    private bool isDashBottonDown;
    
    [SerializeField] private LayerMask dashLayerMask;

    // animation
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    // actions
    public GameObject bulletPrefab;

    // audio
    AudioSource audioSource;
    public AudioClip getHitClip;
    public AudioClip attackClip;
    public AudioClip getHealingClip;

    private void Awake(){
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        state = State.Normal;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();       
        aiming = GameObject.Find("MahouShoujo"); 
    }

    private void Update(){
        switch(state){
            // when in Normal state, player can move, perform skills, get hurt ,attack and interact
            case State.Normal:
                // move
                speed = 10.0f;
                HandleMovement();

                // do special action such as dash and skills
                Perform();

                // hurt judgement
                HurtJudgement();

                // interact with NPC or gears
                HandleInteraction();

                // do normal melee and missle attack
                HandleAttacking();
                break;
            
            // when in Attacking state, player should be unable to move or interact, but can perform skills, be hurt or do another attack
            case State.Attacking:
                HandleAttacking();
                Perform();
                HurtJudgement();
                break;

            // when in Skilling state, player could only wait until skill is over
            case State.Skilling:
                HurtJudgement();
                break;
        }

    }

    private void FixedUpdate() {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        rigidbody2d.MovePosition(position);

        Dash();
    }

    public void SetState(int a){
        switch(a){
            case 0:
                state = State.Normal;
                break;
            case 1:
                state = State.Attacking;
                break;
            case 2:
                state = State.Dashing;
                break;
            case 3:
                state = State.Skilling;
                break;

            default:
                state = State.Normal;
                break;
        }
    }

    private void HandleMovement(){
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal,vertical);
        if (!Mathf.Approximately(move.x, 0.0f) || (!Mathf.Approximately(move.y, 0.0f)))
        {
            lookDirection.Set(move.x ,move.y);
            lookDirection.Normalize();
        }   

        // set moving animaion paraments
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
    }
    
    private void Stop(){
        speed = 0.0f;
        lookDirection = Vector2.zero;
    }

    private void Dash(){
        if (isDashBottonDown){
            SetState(2);
            float dashAmount = 5f;
            Vector2 dashPosition = rigidbody2d.position + lookDirection * dashAmount;
            RaycastHit2D dashHit = Physics2D.Raycast(rigidbody2d.position, lookDirection, dashAmount, dashLayerMask);
            if (dashHit.collider != null){
                dashPosition = dashHit.point;
            }
            rigidbody2d.MovePosition(dashPosition);
            isDashBottonDown = false;
            SetState(0);
        }
    }

    private void Perform(){
        // to judge what special action should be performed
        // dash
        if (Input.GetKeyDown(KeyCode.Space)){
            isDashBottonDown = true;
            Debug.Log("Should perform dash!");
        }

        // skills
        if (Input.GetKeyDown(KeyCode.R)){
            SetState(3);
            Stop();
            Debug.Log("Should perform skill1!");
            SetState(0);
        }        
        if (Input.GetKeyDown(KeyCode.F)){
            SetState(3);
            Stop();
            Debug.Log("Should perform skill2!");
            SetState(0);
        }        
        if (Input.GetKeyDown(KeyCode.C)){
            SetState(3);
            Stop();
            Debug.Log("Should perform skill3!");
            SetState(0);
        }        
        if (Input.GetKeyDown(KeyCode.G)){
            SetState(3);
            Stop();
            Debug.Log("Should perform skill4!");
            SetState(0);
        }

    }

    private void HurtJudgement(){
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
    }

    private void HandleInteraction(){
        if (Input.GetKeyDown(KeyCode.T)){
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, 
            lookDirection, 1.5f, LayerMask.GetMask("Interactable"));
            if (hit.collider != null){
                NonPlayerCharacter npc = hit.collider.GetComponent<NonPlayerCharacter>();
                Gear gear = hit.collider.GetComponent<Gear>();
                if(npc != null){
                    npc.DisplayDialog();
                }
                if(gear != null){
                    gear.Action();
                }
            }
        }
    }

    public void ChangeHealth(int amount) {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            animator.SetTrigger("Hit");
            audioSource.PlayOneShot(getHitClip);

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        animator.SetTrigger("Heal");
        audioSource.PlayOneShot(getHealingClip);

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        //Debug.Log(currentHealth + "/" + maxHealth);
        // UI change
    }


    private void HandleAttacking() {
        SetState(1);
        if (Input.GetMouseButtonDown(0)) {
            Stop();
            Vector3 mousePosition = aiming.GetComponent<PlayerAim>().GetMouseWorldPosition();
            Debug.Log("MissleAttack!");
        }
        if (Input.GetMouseButtonDown(1))
        {
            Stop();
            Vector3 mousePosition = aiming.GetComponent<PlayerAim>().GetMouseWorldPosition();
            Vector3 mouseDir = (mousePosition - transform.position).normalized;
            float attackRange = 3f;
            Vector3 attackPosition = transform.position + mouseDir * attackRange;
            Debug.Log(attackPosition);
            Debug.Log("MeleeAttack!");
        }
        SetState(0);
    }

}
