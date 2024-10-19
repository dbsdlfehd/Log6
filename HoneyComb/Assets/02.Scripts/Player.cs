using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    //Animator anim;
    PlayerHpShow PlayerHpShow;


    [Header("죽은 횟수")]
    public int DeadCount = 0;
	public TextMeshProUGUI DeadCount_UI;

	//플레이어 위치
	public Transform player;

    //집 위치
	public Transform Home;

    //지옥 위치
    public Transform DeadPoint;

	public bool isDead = false;
    public GameObject Dead_set;//죽음 연출
    //public void PlayAnimation(int atkNum)
    //{
    //    anim.SetFloat("Blend", atkNum);
    //    anim.SetTrigger("Atk");

    //}

    [Header("체력")]
    public float maxHP;
    public float nowHP;
	public TextMeshProUGUI HP_UI;

	[Header("공격")]
	public int Atk;
    //public Slider slider;
	public TextMeshProUGUI Atk_UI;

	[Header("속도")]
    public int speed;
    public float minPos;//이게 뭐더라?
    public float maxPos;//잘 모르겠음 아시는 분 주석좀 ㅋㅋ
    public RectTransform pass;//얘도 ㅋㅋㅋㅋㅋ
    public int atkNum;//콤보? 번호일듯

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
        //    if (atkNum < 4)  //  공격모션이 4개이기때문
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


    //업데이트 메소드
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && !isAtk)
        //{
        //    isAtk = true;
        //    //SetAtk();
        //}

        //만약 죽은 상태에서 스페이스바를 누른다면?
        if(isDead == true && Input.GetKeyDown(KeyCode.Space)) 
        {
			//플레이어 보이게 하기
			//gameObject.SetActive(true);

			//체력 만땅
			nowHP = maxHP;

            //안죽은 상태로 바꿈
            isDead = false;

			//집 위치로 가기
			player.position = Home.position;

			//죽음 연출 보이는 거 없애기
			Dead_set.SetActive(false);
		}


		//죽은 상태일 때는 경직
		if (isDead == true)
			player.position = DeadPoint.position;

        //UI
		//체력
		HP_UI.text = "HP : " + nowHP.ToString();

        //공격력
		Atk_UI.text = Atk.ToString();

        //죽은 횟수
		DeadCount_UI.text = "죽은 횟수 : " + DeadCount.ToString();
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("weapon"))
            return;

		nowHP = nowHP - collision.GetComponent<FarATK>().damage;

        //현재 체력이 0보다 작거나 같으면
        if (nowHP < 0)
        {
            //죽음 함수 실행
			Dead();
		}
    }

    //죽음 함수
    void Dead()
    {
        //죽은 횟수 추가하기
        DeadCount++;

		isDead = true;
        Dead_set.SetActive(true);

		//지옥 가기
		player.position = DeadPoint.position;

		//gameObject.SetActive(false);//플레이어 오브젝트 안보이기 처리
	}
}
