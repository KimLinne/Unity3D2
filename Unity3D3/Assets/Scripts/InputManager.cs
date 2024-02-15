using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -mainCam.transform.position.z;

            mainCam.ScreenToWorldPoint(mousePos);

        }
    }
}
