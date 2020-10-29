using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public enum Direction { up, down, left, right };
    public Direction direction;

    void OnTriggerEnter2D(Collider2D other) {
        
        if (other.gameObject.CompareTag("Player")){
            Room room = gameObject.GetComponentInParent(typeof(Room)) as Room;
            if (room != null){
                room.PlayerEnter();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        
        if (other.gameObject.CompareTag("Player")){
            Room room = gameObject.GetComponentInParent(typeof(Room)) as Room;
            if (room != null){
                room.PlayerExit();
            }
        }
    }
}
