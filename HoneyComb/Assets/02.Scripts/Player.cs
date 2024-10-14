using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    //Animator anim;
    PlayerHpShow PlayerHpShow;


    [Header("���� Ƚ��")]
    public int DeadCount = 0;
	public TextMeshProUGUI DeadCount_UI;

	//�÷��̾� ��ġ
	public Transform player;

    //�� ��ġ
	public Transform Home;

    //���� ��ġ
    public Transform DeadPoint;

	public bool isDead = false;
    public GameObject Dead_set;//���� ����
    //public void PlayAnimation(int atkNum)
    //{
    //    anim.SetFloat("Blend", atkNum);
    //    anim.SetTrigger("Atk");

    //}

    [Header("ü��")]
    public float maxHP;
    public float nowHP;
	public TextMeshProUGUI HP_UI;

	[Header("����")]
	public int Atk;
    //public Slider slider;
	public TextMeshProUGUI Atk_UI;

	[Header("�ӵ�")]
    public int speed;
    public float minPos;//�̰� ������?
    public float maxPos;//�� �𸣰��� �ƽô� �� �ּ��� ����
    public RectTransform pass;//�굵 ����������
    public int atkNum;//�޺�? ��ȣ�ϵ�

	private void Start()
	{
		//anim = GetComponent<Animator>();
		nowHP = maxHP;
		PlayerHpShow = GetComponent<PlayerHpShow>();
	}

	//public void SetAtk()
 //   {
 //       //slider.value = 0;
 //       minPos = pass.anchoredPosition.x;
 //       maxPos = pass.sizeDelta.x + minPos;
 //       StartCoroutine(ComboAtk());
 //   }

    IEnumerator ComboAtk()
    {
        yield return null;
        //while (!(Input.GetKeyDown(KeyCode.Space) || slider.value == slider.maxValue))
        //{
        //    slider.value += Time.deltaTime * speed;
        //    yield return null;
        //}
        //if (slider.value >= minPos && slider.value <= maxPos)
        //{
        //    //PlayAnimation(atkNum++);
        //    if (atkNum < 4)  //  ���ݸ���� 4���̱⶧��
        //        SetAtk();
        //    else
        //    {
        //        atkNum = 0;
        //        isAtk = false;
        //    }
        //}
        //else
        //{
        //    //PlayAnimation(0);
        //    isAtk = false;
        //    atkNum = 0;
        //}
        //slider.value = 0;
    }
    //bool isAtk = false;


    //������Ʈ �޼ҵ�
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && !isAtk)
        //{
        //    isAtk = true;
        //    //SetAtk();
        //}

        //���� ���� ���¿��� �����̽��ٸ� �����ٸ�?
        if(isDead == true && Input.GetKeyDown(KeyCode.Space)) 
        {
			//�÷��̾� ���̰� �ϱ�
			//gameObject.SetActive(true);

			//ü�� ����
			nowHP = maxHP;

            //������ ���·� �ٲ�
            isDead = false;

			//�� ��ġ�� ����
			player.position = Home.position;

			//���� ���� ���̴� �� ���ֱ�
			Dead_set.SetActive(false);
		}


		//���� ������ ���� ����
		if (isDead == true)
			player.position = DeadPoint.position;

        //UI
		//ü��
		HP_UI.text = "HP : " + nowHP.ToString();

        //���ݷ�
		Atk_UI.text = Atk.ToString();

        //���� Ƚ��
		DeadCount_UI.text = "���� Ƚ�� : " + DeadCount.ToString();
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("weapon"))
            return;

		nowHP = nowHP - collision.GetComponent<FarATK>().damage;

        //���� ü���� 0���� �۰ų� ������
        if (nowHP < 0)
        {
            //���� �Լ� ����
			Dead();
		}
    }

    //���� �Լ�
    void Dead()
    {
        //���� Ƚ�� �߰��ϱ�
        DeadCount++;

		isDead = true;
        Dead_set.SetActive(true);

		//���� ����
		player.position = DeadPoint.position;

		//gameObject.SetActive(false);//�÷��̾� ������Ʈ �Ⱥ��̱� ó��
	}
}
