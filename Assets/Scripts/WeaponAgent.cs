using System;
using TMPro;
using UnityEngine;

public class WeaponAgent : ItemAgent
{
    private Transform aimTransform;
    GameObject parent;
    public GameObject bulletPrefab;
    public Weapon Weapon { get; set; }

    private Vector3 aimDir;
    private void Awake()
    {
        Weapon = Global.items[itemIndex].Clone() as Weapon;
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();

        aimTransform = transform;
        try
        {
            parent = transform.parent.gameObject;
        }
        catch (Exception)
        {
            parent = null;
        }
    }

    private void Update()
    {
        if (parent != null)
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
        GameObject.Instantiate(Global.GetPrefab(item.GetType().ToString()), character.transform);
        character.WeaponPrefab = character.transform.GetChild(0).gameObject;

    }

    public void Attack()
    {
        //Weapon.GetType().GetInterface("IWeapon")
        Weapon.Attack(parent, aimDir);
        Debug.Log("Should Attack");
    }

}

