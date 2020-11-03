using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_MahouShoujo : MonoBehaviour
{

    enum State
    {
        Normal,
    }

    private State state;

    // set character's speed
    public float speed = 3.0f;

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
    }

    private void Update(){
        // get look direction
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal,vertical);
        if (!Mathf.Approximately(move.x, 0.0f) || (!Mathf.Approximately(move.y, 0.0f)))
        {
            lookDirection.Set(move.x ,move.y);
            lookDirection.Normalize();
        }    

        // dash management
        if (Input.GetKeyDown(KeyCode.Space)){
            isDashBottonDown = true;
        }

        // set moving animaion paraments
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // hurt judgement
        HurtJudgement();

        // interaction
        HandleInteraction();
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);

        if (isDashBottonDown){
            float dashAmount = 15f;
            Vector2 dashPosition = rigidbody2d.position + lookDirection * dashAmount;
            RaycastHit2D dashHit = Physics2D.Raycast(rigidbody2d.position, lookDirection, dashAmount, dashLayerMask);
            if (dashHit.collider != null){
                dashPosition = dashHit.point;
            }
            rigidbody2d.MovePosition(dashPosition);
            isDashBottonDown = false;
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
        if (Input.GetKeyDown(KeyCode.F)){
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

    public void ChangeHealth(int amount)
    {
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

    public void MeleeAttack(Vector3 mouseDir) {
        Debug.Log("MeleeAttack!");
        Debug.Log(mouseDir);

    }

    public  void MissleAttack(Vector3 mouseDir) {
        Debug.Log("MissleAttack!");
        Debug.Log(mouseDir);
        
        GameObject bulletObject = Instantiate(bulletPrefab, rigidbody2d.position + Vector2.up*1.0f, Quaternion.identity);
        Bullets bullet = bulletObject.GetComponent<Bullets>();
        bullet.Fire(mouseDir,500);

        animator.SetTrigger("Launch");

    }
}
