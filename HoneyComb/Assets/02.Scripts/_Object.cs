using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Object : MonoBehaviour
{
    public int id;//NPC번호

    //public bool isNPC;//NPC인지 확인하는 건데 필요 없는듯

    //NPC별 횟차별 대화 횟수
    public int EachTalkCount = 0;
    public int TempEachTalkCount = 0;

    //테스트용 이름 변수
    public new string name;

    //NPC별 죽은뒤 대화 확인용 bool형 변수
	public bool isDeadUp = false;
	public bool isUpGraded = false;

    public int PlayerDead;
    public int tempPlayerDead = 0;
    public bool isDialogged = true;

    public GameObject malPeungSeon;//말풍선 이미지
    private Player player;//플레이어 스크립트
    private float range = 1.5f; // 플레이어 감지범위

	private void Start()
	{
		player = FindObjectOfType<Player>();//무조건 해줘야됨 (초기화)
	}

	private void FixedUpdate()
	{
		Vector2 direction = player.transform.position - transform.position;

		float X = Mathf.Abs(Mathf.Round(direction.x));
		float Y = Mathf.Abs(Mathf.Round(direction.y));

        //Debug.Log($"{X} {Y}");

		if (isDialogged && (X<= range && Y <= range))//선언문에서 조절하셈
        {
			Debug.Log($"{name} 말풍선 켜기");
			malPeungSeon.SetActive(true);
		}
        else if (!isDialogged || (X >= range && Y >= range))//선언문에서 조절하셈
		{
            Debug.Log($"{name} 말풍선 끄기");
            malPeungSeon.SetActive(false);
        }
	}
}
