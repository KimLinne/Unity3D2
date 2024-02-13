using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    [SerializeField] float roundSpeed = 1f;


         
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {   //Rotate는 오브젝트 자체를 가운데 기준으로 돌린다.
            //RotateAround는 어떤 점을 기준으로 돌린다. 오브젝트와 카메라의 거리를 자동계산.
            transform.RotateAround(Vector3.zero, Vector3.up, roundSpeed * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            //방향 지정은 여러가지, up을 down으로 roundSpeed를 -roundSpeed 로.
            transform.RotateAround(Vector3.zero, Vector3.down, roundSpeed * Time.deltaTime);
        }
    }
}
