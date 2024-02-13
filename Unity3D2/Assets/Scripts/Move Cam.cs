using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    [SerializeField] float roundSpeed = 1f;


         
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {   //Rotate�� ������Ʈ ��ü�� ��� �������� ������.
            //RotateAround�� � ���� �������� ������. ������Ʈ�� ī�޶��� �Ÿ��� �ڵ����.
            transform.RotateAround(Vector3.zero, Vector3.up, roundSpeed * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            //���� ������ ��������, up�� down���� roundSpeed�� -roundSpeed ��.
            transform.RotateAround(Vector3.zero, Vector3.down, roundSpeed * Time.deltaTime);
        }
    }
}
