using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("속도")]
    public float speed;
	private Rigidbody2D rb;//중력
	private Player player;//플레이어
	private PlayerAction playerAction;
	private PrefabSpawner prefabSpawner;

    private Rigidbody2D target;

	[Header("감지 범위")]
    public int SenserRangeX = 3;
    public int SenserRangeY = 3;

	[Header("체력")]
	public int maxHP; // 최대 체력 변수
    public int nowHP; // 현재 체력 변수

	[Header("이건 나도 몰루")]
    public Scanner scanner;
    public Scanner2 scanner2;

    bool isLive;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

	//체력바 프리펩 
	[SerializeField]					// private형 변수를 외부에서 조절할 수 있게 바꿔줌
	private GameObject prfHpBar;		// 프리펩 체력바

	RectTransform bghp_bar;				// bghp_bar 어두운 배경 체력바
	Image hp_bar;						// hp_bar 현재 체력바

	public float height = 1.7f;			// 체력바 Y 높이

    private void SetEnemyStatus(int _maxHP)
    {
        maxHP = _maxHP;
        nowHP = _maxHP;
    }

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		player = FindObjectOfType<Player>();// 무조건 해줘야됨 (초기화)
		SetEnemyStatus(100);				// 체력 수치 설정

		// prfHpBar 프리팹을 이용해 canvas에다가 체력바 생성.
		bghp_bar = Instantiate(prfHpBar, GameObject.Find("Canvas").transform).GetComponent<RectTransform>(); // bghp_bar생성
		hp_bar = bghp_bar.transform.GetChild(0).GetComponent<Image>(); // bghp_bar에 자식 오브젝트 컴포넌트 가져오기
	}

	void Update()
	{
		// 카메라 보는 기준 체력바 좌표 위치 설정
		Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
		bghp_bar.position = _hpBarPos; // 해당 좌표의 위치 적용하기

		hp_bar.fillAmount = (float)nowHP / (float)maxHP; // 체력 수치 적용하기
	}

	void Awake()
    {
		rigid = GetComponent<Rigidbody2D>();
        target = GetComponent<Rigidbody2D>();
		spriter = GetComponent<SpriteRenderer>();
        scanner = GetComponent<Scanner>(); //근거리 공격용 스캔
        scanner2 = GetComponent<Scanner2>(); //원거리 공격용 스캔
		playerAction = FindObjectOfType<Player>().GetComponent<PlayerAction>();
		prefabSpawner = FindAnyObjectByType<PrefabSpawner>();
	}

	void FixedUpdate()
    {
		if(player.EnmeyDown == true)
        {
			EnemyDead();
			//GetComponent<Image>();     // 체력바(색) 파괴
			//GetComponent<RectTransform>();   // 체력바(배경) 파괴
			//Destroy(gameObject); // 적 (자기자신) 파괴
		}

		Vector2 direction = player.transform.position - transform.position;

		int X = Math.Abs(Mathf.RoundToInt(direction.x));
		int Y = Math.Abs(Mathf.RoundToInt(direction.y));

		if (X <= SenserRangeX && Y <= SenserRangeY)//가까이 있을때
		{
			//따라간다.
			Vector2 nextVec = direction.normalized * speed * Time.fixedDeltaTime;
			rigid.MovePosition(rigid.position + nextVec);
			rigid.velocity = Vector2.zero;

			//왼쪽 오른쪽 플립해주는 것
			if(direction.x > 0)
			{
				spriter.flipX = false;
			}
			else if(direction.x < 0)
			{
				spriter.flipX = true;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
            nowHP = nowHP - player.Atk;
		}
        if(nowHP < 0)							   // 체력이 0보다 적을시
        {
			EnemyDead();
		}
	}

	void EnemyDead()
    {
		nowHP = 0;
		Destroy(gameObject);
		prefabSpawner.RoomEnemyCount++;        // 적 죽은 횟수 1 늘어남
		Destroy(bghp_bar.gameObject);          // 체력바 삭제
	}
}
