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

    [Header("ü��")]
    public float HP;
	public TextMeshProUGUI HP_UI;

	[Header("����")]
	public int Atk;
    public Slider slider;
	public TextMeshProUGUI Atk_UI;

	[Header("�ӵ�")]
    public int speed;
    public float minPos;//�̰� ������?
    public float maxPos;//�� �𸣰��� �ƽô� �� �ּ��� ����
    public RectTransform pass;//�굵 ����������
    public int atkNum;//�޺�? ��ȣ�ϵ�

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
            if (atkNum < 4)  //  ���ݸ���� 4���̱⶧��
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

        //�÷��̾� ü�� UI�� ��Ÿ����
        HP_UI.text = "���� �÷��̾� ü�� : " + HP.ToString();
		Atk_UI.text = Atk.ToString();

	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("weapon"))
            return;

        HP = HP - collision.GetComponent<FarATK>().damage;

        if (HP > 0)
        {
            //�̷��� �׳� ����ִ� ��
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
