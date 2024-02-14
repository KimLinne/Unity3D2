using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitHole : MonoBehaviour
{

    [SerializeField] float destroyTime = 1.0f;
    
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    
    void Update()
    {
        
    }
}
