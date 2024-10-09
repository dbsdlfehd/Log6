using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    Animator anim;
    PlayerHpShow PlayerHpShow;

    public void PlayAnimation(int atkNum)
    {
        anim.SetFloat("Blend", atkNum);
        anim.SetTrigger("Atk");

    }

    [Header("체력")]
    public float maxHP;
    public float nowHP;
	public TextMeshProUGUI HP_UI;

	[Header("공격")]
	public int Atk;
    public Slider slider;
	public TextMeshProUGUI Atk_UI;

	[Header("속도")]
    public int speed;
    public float minPos;//이게 뭐더라?
    public float maxPos;//잘 모르겠음 아시는 분 주석좀 ㅋㅋ
    public RectTransform pass;//얘도 ㅋㅋㅋㅋㅋ
    public int atkNum;//콤보? 번호일듯

	private void Start()
	{
		anim = GetComponent<Animator>();
		nowHP = maxHP;
		PlayerHpShow = GetComponent<PlayerHpShow>();
	}

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

        //플레이어 체력 UI에 나타내기
        HP_UI.text = "현재 플레이어 체력 : " + nowHP.ToString();
		Atk_UI.text = Atk.ToString();

	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("weapon"))
            return;

		nowHP = nowHP - collision.GetComponent<FarATK>().damage;

        //현재 체력이 0보다 작거나 같으면
        if (nowHP < 0)
        {
			Dead();
		}
    }

    void Dead()
    {
		gameObject.SetActive(false);//플레이어 오브젝트 안보이기 처리
	}
}
