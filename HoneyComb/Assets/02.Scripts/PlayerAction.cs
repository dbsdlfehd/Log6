using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    public float walkSpeed = 5f;        // �ȱ� �ӵ�
    public float defaultSpeed = 5f;     // �⺻ �ӵ�(�ȱ� �ӵ��� �����ؾ���)
    public float slideSpeed = 10f;      // �����̵� �ӵ�
    public float slideDuration = 0.3f;  // �����̵� ���� �ð� (0.3��)
    public float slideCooldown = 1f;    // �����̵� �� ��Ÿ�� (1��)
    private float slideTime = 0f;       // ���� �����̵� �ð�
    private float cooldownTime = 0f;    // ���� ��Ÿ�� �ð�
    public float skillAttackCooldown = 2f;    // ù ��° ��ų ��Ÿ�� 2��
    public float skillAttack2Cooldown = 3f;   // �� ��° ��ų ��Ÿ�� 3��
    private float skillAttackTime = 0f;       // ù ��° ��ų ��� ���� �ð�
    private float skillAttack2Time = 0f;      // �� ��° ��ų ��� ���� �ð�

    Vector2 moveInput;
    Vector3 dirVec;

    [Header("�Ǽ� ������")]
    public float Length = 0.7f;

    [Header("���� ����")]
    public BoxCollider2D left; // ���� �ݶ��̴�
    public BoxCollider2D right; // ������ �ݶ��̴�

    [Header("��ȭâ")]
    //public TextMeshProUGUI Dialog_UI_text;
    //private string dialog_text;
	public TalkManager talkManager;

    Rigidbody2D rigid;        // Rigidbody2D ������Ʈ ����
    Animator animator;     // Animator ������Ʈ ����
    SpriteRenderer sp;

    private GameObject scanObject;

    private Collider2D playerCollider; // �÷��̾��� �ݶ��̴�

    // �̵� ���� Ȯ�� ���� (�ִϸ����Ϳ� ����)
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
            animator.SetBool(AnimationStrings.isMoving, value);  // �ִϸ��̼� ���� ����
        }
    }

    // �����̵� ���� Ȯ�� ���� (�ִϸ����Ϳ� ����)
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
            animator.SetBool(AnimationStrings.isSliding, value);  // �ִϸ��̼� ���� ����
        }
    }
    private bool isCooldown = false; // �����̵� ��Ÿ�� ������ ����

    // ĳ���Ͱ� �������� ���� �ִ��� Ȯ���ϴ� ����
    public bool _isFacingRight = true;
    

    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;
        }
    }

    // canMove: �÷��̾ �̵��� �� �ִ��� ���θ� ��Ÿ��
    public bool canMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    void Awake()
    {
        //dialog_text = "�ƹ��͵� �߰ߵȰ� �����ϴ�.";
		rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>(); // �÷��̾��� �ݶ��̴� ��������
	}

    void Update()
    {
        if (isCooldown)
        {
            cooldownTime -= Time.deltaTime;
            if (cooldownTime <= 0)
            {
                isCooldown = false;  // ��Ÿ�� ����
            }
        }

        if (IsSliding)
        {
            slideTime -= Time.deltaTime;
            if (slideTime <= 0)
            {
                StopSliding();  // �����̵尡 ������ �����̵� ����
            }
        }

        // ��Ÿ�� ����
        if (skillAttackTime > 0)
        {
            skillAttackTime -= Time.deltaTime;
        }

        if (skillAttack2Time > 0)
        {
            skillAttack2Time -= Time.deltaTime;
        }

        // UI�� �����ֱ�
        //Dialog_UI_text.text = dialog_text.ToString();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (scanObject == null)
            {
                //dialog_text = "������ ���� ����� �ٶ󺸸� \n EŰ�� ��������.";
            }
            else if (scanObject != null)
            {
                talkManager.DialogAction(scanObject);
                //dialog_text = "�̰��� " + scanObject.name + " �Դϴ�.";
                //Debug.Log("�̰��� " + scanObject.name + " �Դϴ�.");
            }
        }
    }

    IEnumerator DisableCollider(BoxCollider2D collider)
    {
        yield return new WaitForSeconds(0.1f); // �ݶ��̴��� 0.1�� ���� Ȱ��ȭ
        collider.enabled = false;
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rigid.velocity = new Vector2(moveInput.x * walkSpeed, moveInput.y * walkSpeed);
        }
        else
        {
            rigid.velocity = new Vector2(0, 0);  // �̵��� ���� �� �ӵ��� 0���� ����
        }

        // �÷��̾� ������ ���� ������ ����
        Debug.DrawRay(rigid.position, dirVec * Length, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, Length, LayerMask.GetMask("Object"));

        // ���� ������!
        if (rayHit.collider != null) scanObject = rayHit.collider.gameObject;
        else scanObject = null;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            moveInput = context.ReadValue<Vector2>();

            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
    }

    void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
            dirVec = Vector3.right;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
            dirVec = Vector3.left;
        }
    }

    public void OnSlide(InputAction.CallbackContext context)
    {
        if (context.started && !isCooldown && !IsSliding)
        {
            StartSliding();
        }
        else if (context.canceled && IsSliding)
        {
            StopSliding();
        }
    }

    private void StartSliding()
    {
        IsSliding = true;
        slideTime = slideDuration;
        walkSpeed = slideSpeed;
    }

    private void StopSliding()
    {
        IsSliding = false;
        walkSpeed = defaultSpeed;
        isCooldown = true;
        cooldownTime = slideCooldown;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // ���⿡ ���� ������ BoxCollider2D Ȱ��ȭ
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
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnSkillAttack(InputAction.CallbackContext context)
    {
        if (context.started)  // ��Ÿ���� 0�� ���� ��ų �ߵ�
        {
            if (skillAttackTime <= 0)
            {
                animator.SetTrigger(AnimationStrings.skillAttackTrigger);
                skillAttackTime = skillAttackCooldown;  // ��Ÿ�� ����
            }
        }
    }

    public void OnSkillAttack2(InputAction.CallbackContext context)
    {
        if (context.started)  // ��Ÿ���� 0�� ���� ��ų �ߵ�
        {
            if (skillAttack2Time <= 0)
            {
                animator.SetTrigger(AnimationStrings.skillAttackTrigger2);
                skillAttack2Time = skillAttack2Cooldown;  // ��Ÿ�� ����
            }
        }
    }

    //void Dead()
    //{
    //       //Debug.Log("�÷��̾� �����");
    //	gameObject.SetActive(false);//�÷��̾� ������Ʈ �Ⱥ��̱� ó��
    //}
}
