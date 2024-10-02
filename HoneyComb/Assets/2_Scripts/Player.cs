using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayAnimation(int atkNum)
    {
        anim.SetFloat("Blend", atkNum);
        anim.SetTrigger("Atk");

    }
    public Slider slider;
    public int speed;
    public float minPos;
    public float maxPos;
    public RectTransform pass;
    public int atkNum;

    public void SetAtk()
    {
        slider.value = 0;
        minPos = pass.anchoredPosition.x;
        maxPos = pass.sizeDelta.x + minPos;
        StartCoroutine(ComboAtk());
    }

    IEnumerator ComboAtk()
    {
        yield return null;
        while (!(Input.GetKeyDown(KeyCode.Space) || slider.value == slider.maxValue))
        {
            slider.value += Time.deltaTime * speed;
            yield return null;
        }
        if (slider.value >= minPos && slider.value <= maxPos)
        {
            PlayAnimation(atkNum++);
            if (atkNum < 4)  //  공격모션이 4개이기때문
                SetAtk();
            else
            {
                atkNum = 0;
                isAtk = false;
            }
        }
        else
        {
            PlayAnimation(0);
            isAtk = false;
            atkNum = 0;
        }
        slider.value = 0;
    }
    bool isAtk = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAtk)
        {
            isAtk = true;
            SetAtk();
        }
    }
}
