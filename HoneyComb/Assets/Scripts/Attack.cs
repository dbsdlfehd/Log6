using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
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
