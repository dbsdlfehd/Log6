using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    public float walkSpeed = 5f;        // 걷기 속도
    public float defaultSpeed = 5f;     // 기본 속도(걷기 속도와 동일해야 함)
    public float slideSpeed = 10f;      // 슬라이드 속도
    public float slideDuration = 0.3f;  // 슬라이드 지속 시간 (0.3초)
    public float slideCooldown = 1f;    // 슬라이드 후 쿨타임 (1초)
    private float slideTime = 0f;       // 남은 슬라이드 시간
    private float cooldownTime = 0f;    // 남은 쿨타임 시간
    public float skillAttackCooldown = 2f;    // 첫 번째 스킬 쿨타임 2초
    public float skillAttack2Cooldown = 3f;   // 두 번째 스킬 쿨타임 3초
    private float skillAttackTime = 0f;       // 첫 번째 스킬 사용 가능 시간
    private float skillAttack2Time = 0f;      // 두 번째 스킬 사용 가능 시간

    Vector2 moveInput;  // 플레이어의 입력을 저장하는 변수
    Vector3 dirVec;     // 플레이어의 방향을 저장하는 변수

    [Header("실선 추적기")]
    public float Length = 0.7f;  // 감지할 거리

    [Header("공격 범위")]
    public BoxCollider2D left;   // 왼쪽 공격 범위 콜라이더
    public BoxCollider2D right;  // 오른쪽 공격 범위 콜라이더

    [Header("대화창")]
    //public TextMeshProUGUI Dialog_UI_text;
    //private string dialog_text;
    public TalkManager talkManager;  // 대화 매니저 참조
    public GameObject DialogSet;     // 대화창

    Rigidbody2D rigid;        // Rigidbody2D 컴포넌트 참조
    Animator animator;        // Animator 컴포넌트 참조
    SpriteRenderer sp;        // SpriteRenderer 컴포넌트 참조

    private GameObject scanObject;   // 감지된 오브젝트

    private Collider2D playerCollider; // 플레이어의 콜라이더

    // 이동 상태 확인 변수 (애니메이터와 연동)
    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);  // 애니메이션 상태 변경
        }
    }

    // 슬라이딩 상태 확인 변수 (애니메이터와 연동)
    [SerializeField]
    private bool _isSliding = false;
    public bool IsSliding
    {
        get
        {
            return _isSliding;
        }
        private set
        {
            _isSliding = value;
            animator.SetBool(AnimationStrings.isSliding, value);  // 애니메이션 상태 변경
        }
    }
    private bool isCooldown = false; // 슬라이드 쿨타임 중인지 여부

    // 캐릭터가 오른쪽을 보고 있는지 확인하는 변수
    public bool _isFacingRight = true;


    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1); // 캐릭터의 방향을 뒤집음
            }

            _isFacingRight = value;
        }
    }

    // canMove: 플레이어가 이동할 수 있는지 여부를 나타냄
    public bool canMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    void Awake()
    {
        // 필요한 컴포넌트들을 가져옴
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>(); // 플레이어의 콜라이더 가져오기
    }

    void Update()
    {
        if (isCooldown)
        {
            cooldownTime -= Time.deltaTime;
            if (cooldownTime <= 0)
            {
                isCooldown = false;  // 쿨타임 종료
            }
        }

        if (IsSliding)
        {
            slideTime -= Time.deltaTime;
            if (slideTime <= 0)
            {
                StopSliding();  // 슬라이드가 끝나면 슬라이딩 종료
            }
        }

        // 스킬 쿨타임 감소
        if (skillAttackTime > 0)
        {
            skillAttackTime -= Time.deltaTime;
        }

        if (skillAttack2Time > 0)
        {
            skillAttack2Time -= Time.deltaTime;
        }

        // 대화창 UI에 보여주기
        //Dialog_UI_text.text = dialog_text.ToString();
    }

    // 대화창 E키
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (scanObject == null)
            {
                // 대상을 찾지 못했을 때의 대사
                //dialog_text = "가까이 가서 대상을 바라보며 \n E키를 누르세요.";
            }
            else if (scanObject != null)
            {
				talkManager.DialogAction(scanObject);
				// 대상을 찾았을 때의 대사
				//dialog_text = "이것은 " + scanObject.name + " 입니다.";
				//Debug.Log("이것은 " + scanObject.name + " 입니다.");
			}
        }
    }

    // 콜라이더를 비활성화하는 코루틴
    IEnumerator DisableCollider(BoxCollider2D collider)
    {
        yield return new WaitForSeconds(0.1f); // 콜라이더를 0.1초 동안 활성화
        collider.enabled = false;
    }

    // 물리 업데이트 처리 (이동 및 감지)
    void FixedUpdate()
    {
        //canMove는 true,
        //isDialoging(대화창 열린 여부)가 false 일때 움직일수 있음
        if (canMove && talkManager.isDialoging == false)
        {
            rigid.velocity = new Vector2(moveInput.x * walkSpeed, moveInput.y * walkSpeed);  // 이동 처리
        }
        else
        {
            rigid.velocity = new Vector2(0, 0);  // 이동을 막을 때 속도를 0으로 설정
        }

        // 플레이어 주위의 얇은 선으로 감지
        Debug.DrawRay(rigid.position, dirVec * Length, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, Length, LayerMask.GetMask("Object"));

        // 무언가 감지됨!
        if (rayHit.collider != null) scanObject = rayHit.collider.gameObject;
        else scanObject = null;
    }

    // 이동 입력 처리
    public void OnMove(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            moveInput = context.ReadValue<Vector2>();

            IsMoving = moveInput != Vector2.zero;  // 움직임 여부 확인

            SetFacingDirection(moveInput);  // 방향 설정
        }
    }

    // 이동 방향 설정
    void SetFacingDirection(Vector2 moveInput)
    {
        //x값이 0보다 큼 && 오른쪽 안바라봄 && 대화가 끝남
        if (moveInput.x > 0 && !IsFacingRight && talkManager.isDialoging == false)
        {
            IsFacingRight = true;
            dirVec = Vector3.right;  // 오른쪽 방향 설정
        }
		//x값이 0보다 큼 && 오른쪽 바라봄 && 대화가 끝남
		else if (moveInput.x < 0 && IsFacingRight && talkManager.isDialoging == false)
        {
            IsFacingRight = false;
            dirVec = Vector3.left;  // 왼쪽 방향 설정
        }
    }

    // 슬라이딩 입력 처리
    public void OnSlide(InputAction.CallbackContext context)
    {
        if (context.started && !isCooldown && !IsSliding)
        {
            StartSliding();  // 슬라이딩 시작
        }
        else if (context.canceled && IsSliding)
        {
            StopSliding();  // 슬라이딩 종료
        }
    }

    // 슬라이딩 시작
    private void StartSliding()
    {
        IsSliding = true;
        slideTime = slideDuration;
        walkSpeed = slideSpeed;  // 슬라이드 속도 적용
    }

    // 슬라이딩 종료
    private void StopSliding()
    {
        IsSliding = false;
        walkSpeed = defaultSpeed;  // 기본 속도로 복귀
        isCooldown = true;
        cooldownTime = slideCooldown;  // 슬라이드 쿨타임 적용
    }

    // 공격 입력 처리
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // 방향에 따라 적절한 BoxCollider2D 활성화
            if (sp.flipX)
            {
                left.enabled = true;
                StartCoroutine(DisableCollider(left));  // 왼쪽 공격 활성화
            }
            else
            {
                right.enabled = true;
                StartCoroutine(DisableCollider(right));  // 오른쪽 공격 활성화
            }
            animator.SetTrigger(AnimationStrings.attackTrigger);  // 공격 애니메이션 실행
        }
    }

    // 첫 번째 스킬 공격 입력 처리
    public void OnSkillAttack(InputAction.CallbackContext context)
    {
        if (context.started)  // 쿨타임이 0일 때만 스킬 발동
        {
            if (skillAttackTime <= 0)
            {
                animator.SetTrigger(AnimationStrings.skillAttackTrigger);  // 스킬 애니메이션 실행
                skillAttackTime = skillAttackCooldown;  // 쿨타임 적용
            }
        }
    }

    // 두 번째 스킬 공격 입력 처리
    public void OnSkillAttack2(InputAction.CallbackContext context)
    {
        if (context.started)  // 쿨타임이 0일 때만 스킬 발동
        {
            if (skillAttack2Time <= 0)
            {
                animator.SetTrigger(AnimationStrings.skillAttackTrigger2);  // 스킬 애니메이션 실행
                skillAttack2Time = skillAttack2Cooldown;  // 쿨타임 적용
            }
        }
    }

    //void Dead()
    //{
    //   // 플레이어 사망 처리
    //   gameObject.SetActive(false);//플레이어 오브젝트 숨기기
    //}
}
