using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorldGenerator : MonoBehaviour
{
    public Item item;

    private void Awake()
    {
       //ItemWorld.SpawnItemWorld(new Vector3(0, 0), new Item { itemType = Item.ItemType.Sword, amount = 1 });
       ItemWorld.SpawnItemWorld(transform.position, item);
       Destroy(gameObject);
    }
}
