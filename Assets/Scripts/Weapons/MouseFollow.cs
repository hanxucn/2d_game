using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    private void Update() {
        FaceMouse();
    }
   
    private void FaceMouse() {
        Vector3 mousePostion = Input.mousePosition;
        mousePostion = Camera.main.ScreenToWorldPoint(mousePostion);
        Vector2 direction = transform.position - mousePostion;
        transform.right = -direction;
    }
}
