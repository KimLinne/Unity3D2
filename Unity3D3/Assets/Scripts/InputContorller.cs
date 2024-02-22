using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputContorller : MonoBehaviour
{

    Animator anim;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        moving();
    }

    private void moving()
    {
        anim.SetFloat("SpeedVertical", Input.GetAxis("Vertical"));
    }
}
