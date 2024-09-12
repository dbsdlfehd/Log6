using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetTrigger("atk1");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            anim.SetTrigger("atk2");
        }
    }
}
