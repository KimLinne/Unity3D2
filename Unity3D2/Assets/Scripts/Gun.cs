using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GameObject objHitHole; //ÇÁ¸®ÆÕ
    [SerializeField] Transform trsTarget;

    short shootCount;
    
    void Update()
    {
        lookTarget();
        shootTarget();
    }

    private void lookTarget()
    {
        if (trsTarget == null) return;

        transform.LookAt(trsTarget);
    }

    private void shootTarget()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)&& 
            Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10.0f, LayerMask.GetMask("Target")))
        {
            createHole(hit);
        }
    }

    private void createHole(RaycastHit _hit)
    {
        GameObject obj = Instantiate(objHitHole, _hit.point + _hit.normal * 0.0001f,
            Quaternion.FromToRotation(Vector3.forward, _hit.normal));

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        sr.sortingOrder = shootCount++;

        if (shootCount >= 32767)
        {
            shootCount = 0;
        }
    }
}
