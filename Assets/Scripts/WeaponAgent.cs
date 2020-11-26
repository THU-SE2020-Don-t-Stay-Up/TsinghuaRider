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
        character.WeaponPrefab = GameObject.Instantiate(Global.GetPrefab(item.GetType().ToString()), character.transform.position, Quaternion.identity, character.transform);
        Debug.Log(character.WeaponPrefab);
    }

    public void Attack()
    {
        Weapon.Attack(user, aimDir);
    }

}

