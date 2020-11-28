using System;
using TMPro;
using UnityEngine;

public class WeaponAgent : ItemAgent
{
    private Transform aimTransform;
    private CharacterAgent user;

    public GameObject bulletPrefab;
    public Weapon Weapon { get; set; }

    private Vector3 aimDir;

    bool leftFlag = false;
    bool upFlag = false;

    private void Awake()
    {
        Weapon = Global.items[itemIndex].Clone() as Weapon;
        Weapon.bulletPrefab = bulletPrefab;
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();

        aimTransform = transform;
        try
        {
            user = transform.parent.gameObject.GetComponent<CharacterAgent>();
        }
        catch (Exception)
        {
            user = null;
        }
    }

    private void Update()
    {
        if (user != null)
        {
            HandleAiming();
            HandlePosition();
        }
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec.z = 0f;
        return vec;
    }

    private void HandleAiming()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        aimDir = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    internal static void Use(CharacterAgent character, Item item)
    {
        
        character.WeaponPrefab = GameObject.Instantiate(Global.GetPrefab(item.GetType().ToString()), character.transform.position + ((Weapon) item).handleOffset, Quaternion.identity, character.transform);
        Debug.Log(character.WeaponPrefab);
    }

    public void Attack()
    {
        Weapon.Attack(user, aimDir);
    }

    private void HandlePosition()
    {
        if (user.characterIndex == 0)
        {
            Vector3 weaponPosition = transform.position;

            if (upFlag && aimDir.y <= 0)
            {
                //Debug.Log("向下看！"); 
                upFlag = false;
                float deltaX = transform.position.x - user.GetPosition().x;
                transform.position = Vector3.MoveTowards(weaponPosition, new Vector3(user.GetPosition().x - deltaX, weaponPosition.y), 10000f);

                //transform.Translate(new Vector3(user.GetPosition().x, weaponPosition.y));

            }
            else if (!upFlag && aimDir.y > 0)
            {
                //Debug.Log("向上看！");

                upFlag = true;
                float deltaX = transform.position.x - user.GetPosition().x;

                transform.position = Vector3.MoveTowards(weaponPosition, new Vector3(user.GetPosition().x - deltaX, weaponPosition.y), 10000f);

                //transform.Translate(new Vector3(user.GetPosition().x, weaponPosition.y));

            }

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

        else
        {

        }





    }

}

