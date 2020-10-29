using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Transform aimTransform;
    private GameObject Character_MahouShoujo;

    private void Awake() {
        aimTransform = transform.Find("Aim");
        Character_MahouShoujo = GameObject.Find("MahouShoujo");
    }

    private Vector3 GetMouseWorldPosition() {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    private static Vector3 GetMouseWorldPositionWithZ(){
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    private static Vector3 GetMouseWorldPositionWithZ(Camera  worldCamera){
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    private static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera){
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    private void Update() {
        HandleAiming();
        HandleActing();
    }

    private void HandleAiming() {
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 aimDir = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0,0,angle);
      
    }

    private void HandleActing() {
        if (Input.GetMouseButtonDown(0)){
            Vector3 mousePosition = GetMouseWorldPosition();
            //Vector3 aimDir = (mousePosition - transform.position).normalized;
            //Vector2 direction = new Vector2(0,0);
            //direction.Set(aimDir.x, aimDir.y);
            //rubyController.GetComponent<RubyController>().Launch(direction);
        }
    }
    
}
