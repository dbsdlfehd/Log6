using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    PlayerHpShow PlayerHpShow;

	public PrefabSpawner prefabSpawner;// 적 생성 스크립트

	public _Object[] npcObjects = new _Object[2]; // NPC

	[Header("죽은 횟수")]
    public int DeadCount = 0;
	public TextMeshProUGUI DeadCount_UI;

	public Transform player;            // 플레이어 위치
	public Transform Home;              // 집 위치
	public Transform DeadPoint;         // 지옥 위치

	public bool isDead = false;
    public GameObject Dead_set;         // 죽음 연출

    [Header("체력")]
    public float maxHP;
    public float nowHP;
	public TextMeshProUGUI HP_UI;

	[Header("공격")]
	public int Atk;
	public TextMeshProUGUI Atk_UI;

	[Header("속도")]
    public float minPos;
    public float maxPos;
    public RectTransform pass;
    public int atkNum;
    

	private void Start()
	{
		nowHP = maxHP;
		PlayerHpShow = GetComponent<PlayerHpShow>();
	}
    private void Update()
    {
        if(isDead == true && Input.GetKeyDown(KeyCode.Space)) // 플레이어 죽음 연출
        {
			nowHP = maxHP;							// 피 회복
            isDead = false;							// 안 죽음
			player.position = Home.position;		// 집 위치로 가기
			Dead_set.SetActive(false);              // 죽음 연출 없애기

			foreach (var npc in npcObjects)			// NPC 말풍선 보여주기
			{
				npc.isDialogged = true;
			}
		}

		if (isDead == true)							// 죽을시 좌표 고정
			player.position = DeadPoint.position;

		HP_UI.text = nowHP.ToString();    // 현재 체력 수치
		Atk_UI.text = Atk.ToString();				// 공격력 수치
		DeadCount_UI.text = "죽은 횟수 : " + DeadCount.ToString();
	}

    void OnTriggerEnter2D(Collider2D collision) // 상대 원거리 공격
    {
        if (!collision.CompareTag("weapon"))
            return;

		nowHP = nowHP - collision.GetComponent<FarATK>().damage;

        if (nowHP < 0) // 딸피
		{
			Dead();
		}
    }

    public void Dead()
    {
        DeadCount++;						  // 죽은 횟수 추가하기

		isDead = true;
        Dead_set.SetActive(true);
		player.position = DeadPoint.position; // 지옥 위치 이동
		prefabSpawner.isSpawnned = false;
	}
}
