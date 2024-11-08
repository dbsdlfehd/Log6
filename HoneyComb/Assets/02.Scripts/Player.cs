using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class Player : MonoBehaviour
{
    PlayerHpShow PlayerHpShow;

    public List<_Object> npcObjects = new List<_Object>();


    public PrefabSpawner prefabSpawner;

    [Header("죽은 횟수")]
    public int DeadCount = 0;
    public TextMeshProUGUI DeadCount_UI;

    //적 모두 킬
    public bool EnmeyDown = false;

    //플레이어 위치
    public Transform player;

    //집 위치
    public Transform Home;

    //지옥 위치
    public Transform DeadPoint;

    public bool isDead = false;
    public GameObject Dead_set; // 죽음 연출

    [Header("체력")]
    public float maxHP = 1000f; // 최대 체력
    public int nowHP; // 체력을 int로 변경
    public TextMeshProUGUI HP_UI;

    [Header("공격")]
    public int Atk;
    public TextMeshProUGUI Atk_UI;

    [Header("방어")]
    public int Defense; // 방어력
    public TextMeshProUGUI Defense_UI;

    public int atkNum; // 콤보 번호

    [Header("체력 디버프 퍼센트")]
    public float healthDebuff1 = 0.35f;       // 체력 감소 디버프 1 (35%) 공황장애
    public float healthDebuff2 = 0.3f;        // 체력 감소 디버프 2 (30%)  긴장성 두통
    public float healthDebuff3 = 0.2f;        // 체력 감소 디버프 3 (20%) 수면장애
    public float healthDebuff4 = 0.1f;        // 체력 감소 디버프 4 (10%) 탈모증

    [Header("방어력 디버프 퍼센트")]
    public float defenseDebuff1 = 0.25f;      // 방어력 감소 디버프 1 (25%) 사회적 불안장애
    public float defenseDebuff2 = 0.14f;      // 방어력 감소 디버프 2 (14%) 자존감 저하
    public float defenseDebuff3 = 0.26f;      // 방어력 감소 디버프 3 (26%) 회피행동
    public float defenseDebuff4 = 0.35f;      // 방어력 감소 디버프 4 (35%) 심장 두근거림


	Animator animator;        // Animator 컴포넌트 참조

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}
	private void Start()
    {
        nowHP = Mathf.FloorToInt(maxHP); // 최대 체력을 정수로 설정
        PlayerHpShow = GetComponent<PlayerHpShow>();

        // 디버프 적용
        //ApplyDebuff();
    }

    // 디버프를 적용하는 함수
    private void ApplyDebuff()
    {
        // 체력 디버프 적용: 95% 감소를 위해 계산
        nowHP = Mathf.FloorToInt(maxHP * 0.05f); // 최종 체력 = 1000 * 0.05

        // 방어력 디버프 적용
        float debuffedDefense = Defense * (1 - defenseDebuff1);
        debuffedDefense *= (1 - defenseDebuff2);
        debuffedDefense *= (1 - defenseDebuff3);
        debuffedDefense *= (1 - defenseDebuff4);
        Defense = Mathf.FloorToInt(debuffedDefense);

        Debug.Log("디버프 적용 완료: ");
        Debug.Log($"체력: {nowHP}");
        Debug.Log($"방어력: {Defense}");
    }

    // 죽음 함수
    public void Dead()
    {
		DeadCount++;
		StartCoroutine(DeadShow());
        
    }
    IEnumerator DeadShow()
    {
		animator.SetTrigger(AnimationStrings.attackTrigger);  // dead 애니메이션 실행

		isDead = true;
		Dead_set.SetActive(true);
		// 지옥 위치로 이동
		player.position = DeadPoint.position;
		prefabSpawner.HideTP();
		prefabSpawner.isSpawnned = false;
		EnmeyDown = true; // 적 모두 비활성화
		yield return null;
	}


	// Trigger 이벤트에서 방어력을 고려한 피해 처리
	void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("weapon"))
            return;

        // 방어력을 고려한 최종 피해량 계산
        float incomingDamage = collision.GetComponent<FarATK>().damage;
        float finalDamage = Mathf.Max(incomingDamage - Defense, 1); // 방어력 적용 후 최소 피해 1로 제한

        nowHP -= Mathf.FloorToInt(finalDamage); // 피해량을 정수로 적용

        if (nowHP <= 0)
        {
            Dead();
        }
    }

    // 업데이트 메소드
    private void Update()
    {
        // 죽은 상태에서 스페이스바를 누르면 부활 처리
        if (isDead && Input.GetKeyDown(KeyCode.Space))
        {
            nowHP = Mathf.FloorToInt(maxHP); // 부활 시 체력을 최대치로 설정
            isDead = false;
            player.position = Home.position;
            Dead_set.SetActive(false);

            foreach (var npc in npcObjects)
            {
                npc.isDialogged = true;
            }
            EnmeyDown = false;//적 죽음 상태 해제
            prefabSpawner.RoomEnemyCount = 0;// 적 죽인 수 0으로 초기화
        }

        if (isDead)
            player.position = DeadPoint.position;

        // UI 갱신
        HP_UI.text = nowHP.ToString() + "/" + maxHP.ToString();
        Atk_UI.text = Atk.ToString();

        DeadCount_UI.text = "죽은 횟수 : " + DeadCount.ToString();
    }
}
