using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("端径")]
    public float HP;
	public TextMeshProUGUI HP_UI;

	[Header("因維")]
	public int Atk;
    public Slider slider;
	public TextMeshProUGUI Atk_UI;

	[Header("紗亀")]
    public int speed;
    public float minPos;//戚惟 更希虞?
    public float maxPos;//設 乞牽畏製 焼獣澗 歳 爽汐岨 せせ
    public RectTransform pass;//剰亀 せせせせせ
    public int atkNum;//爪左? 腰硲析牛

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
            if (atkNum < 4)  //  因維乞芝戚 4鯵戚奄凶庚
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

        //巴傾戚嬢 端径 UI拭 蟹展鎧奄
        HP_UI.text = "薄仙 巴傾戚嬢 端径 : " + HP.ToString();
		Atk_UI.text = Atk.ToString();

	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("weapon"))
            return;

        HP = HP - collision.GetComponent<FarATK>().damage;

        if (HP > 0)
        {
            //戚君檎 益撹 詞焼赤澗 移
        }
        else
        {
            Dead();
        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
