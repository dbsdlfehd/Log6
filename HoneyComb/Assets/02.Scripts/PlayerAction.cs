using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [Header("속도")]
    public float speed;

    [Header("실선 추적기")]
    public float Length = 0.7f;

    [Header("공격 범위")]
    public BoxCollider2D left; // 왼쪽 콜라이더
    public BoxCollider2D right; // 오른쪽 콜라이더

    private SpriteRenderer sp;
    private Rigidbody2D rigid;
    private Animator animator;
    private float garo;
    private float sero;
    private GameObject scanObject;

    private Vector3 dirVec;

    public bool attacked; // 공격 중인지 확인하는 플래그
    private float specialAttackCooldown = 2f; // 특수 공격 쿨타임
    private float nextSpecialAttackTime = 0f; // 다음 특수 공격 가능 시간

    private Collider2D playerCollider; // 플레이어의 콜라이더

    void AttackTrue()
    {
        attacked = true;
    }

    void AttackFalse()
    {
        attacked = false;
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>(); // 플레이어의 콜라이더 가져오기
    }

    void Update()
    {
        garo = Input.GetAxisRaw("Horizontal");
        sero = Input.GetAxisRaw("Vertical");

        // 플레이어의 방향과 애니메이션 업데이트
        if (garo < 0) // 왼쪽
        {
            sp.flipX = true;
            animator.SetTrigger("run");
        }
        else if (garo > 0) // 오른쪽
        {
            sp.flipX = false;
            animator.SetTrigger("run");
        }
        else if (sero != 0)
        {
            animator.SetTrigger("run");
        }

        // 방향 변수 설정
        if (garo < 0) dirVec = Vector3.left; // 왼쪽
        else if (garo > 0) dirVec = Vector3.right; // 오른쪽

        // 오브젝트 스캔
        if (Input.GetButtonDown("Jump") && scanObject != null)
        {
            UnityEngine.Debug.Log("이것은: " + scanObject.name);
        }

        // 공격 입력 (좌클릭)
        if (Input.GetMouseButtonDown(0) && !attacked)
        {
            Attack();
        }

        // 스킬 입력 (우클릭)
        if (Input.GetMouseButtonDown(1) && Time.time >= nextSpecialAttackTime) // Right Mouse Button
        {
            animator.SetTrigger("Skill1"); // "Skill1" 애니메이션 트리거
            nextSpecialAttackTime = Time.time + specialAttackCooldown; // 쿨타임 설정
        }

        // 스킬 입력 (R키)
        if (Input.GetKeyDown(KeyCode.R) && Time.time >= nextSpecialAttackTime) // R Key
        {
            animator.SetTrigger("Skill2"); // "Skill2" 애니메이션 트리거
            nextSpecialAttackTime = Time.time + specialAttackCooldown; // 쿨타임 설정
        }

        // 슬라이드 입력 (스페이스 바)
        if (Input.GetKeyDown(KeyCode.Space)) // Space Key
        {
            StartCoroutine(Slide()); // 슬라이드 코루틴 시작
        }
    }

    private IEnumerator Slide()
    {
        animator.SetTrigger("Slide"); // "Slide" 애니메이션 트리거
        playerCollider.enabled = false; // 플레이어의 콜라이더 비활성화
        yield return new WaitForSeconds(1f); // 1초 대기
        playerCollider.enabled = true; // 플레이어의 콜라이더 활성화
    }

    void Attack()
    {
        AttackTrue(); // 공격 중 플래그 설정
        animator.SetTrigger("Attack"); // 공격 애니메이션 트리거 실행

        // 마우스 클릭 위치 계산
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 2D 게임이므로 z축을 0으로 설정

        // 클릭 방향으로 스프라이트 플립
        if (mousePos.x < transform.position.x)
        {
            sp.flipX = true; // 왼쪽으로 클릭
        }
        else
        {
            sp.flipX = false; // 오른쪽으로 클릭
        }

        // 방향에 따라 적절한 BoxCollider2D 활성화
        if (sp.flipX)
        {
            left.enabled = true;
            StartCoroutine(DisableCollider(left));
        }
        else
        {
            right.enabled = true;
            StartCoroutine(DisableCollider(right));
        }
    }

    IEnumerator DisableCollider(BoxCollider2D collider)
    {
        yield return new WaitForSeconds(0.1f); // 콜라이더를 0.1초 동안 활성화
        collider.enabled = false;
        AttackFalse(); // 공격 중 플래그 리셋
    }

    void FixedUpdate()
    {
        // X, Y축 이동 처리
        Vector2 moveDirection = new Vector2(garo, sero).normalized; // 정규화하여 방향만 유지
        rigid.velocity = moveDirection * speed;

        // 플레이어 주위의 얇은 선으로 감지
        UnityEngine.Debug.DrawRay(rigid.position, dirVec * Length, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, Length, LayerMask.GetMask("Enemy"));

        // 무언가 감지됨!
        if (rayHit.collider != null) scanObject = rayHit.collider.gameObject;
        else scanObject = null;
    }
}
