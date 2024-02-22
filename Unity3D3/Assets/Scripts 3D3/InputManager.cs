using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    Camera mainCam;
    UnitManager unitManager;

    private void Start()
    {
        mainCam = Camera.main;
        unitManager = UnitManager.Instance;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //Z 포지션 위치를 고정.
            //Vector3 mousePos = Input.mousePosition;
            //mousePos.z = -mainCam.transform.position.z;

            //Vector3 worldPoint = mainCam.ScreenToWorldPoint(mousePos);
            //Debug.Log(mousePos);


            //레이를 쏜다.
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 200.0f, LayerMask.GetMask("Ground")))
            {
                //UnitManager.Instance.MovePosition(hit.point); 를 start와 최상단에 인지 시켜 아래 문으로 줄일 수 있다.
                unitManager.MovePosition(hit.point);
            }

        }
    }
}
