using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [Header("�ӵ�")]
    public float speed;

    [Header("�Ǽ� ������")]
    public float Length = 0.7f;

    [Header("���� ����")]
    public BoxCollider2D left; // ���� �ݶ��̴�
    public BoxCollider2D right; // ������ �ݶ��̴�

    private SpriteRenderer sp;
    private Rigidbody2D rigid;
    private Animator animator;
    private float garo;
    private float sero;
    private GameObject scanObject;

    private Vector3 dirVec;

    public bool attacked; // ���� ������ Ȯ���ϴ� �÷���

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
    }

    void Update()
    {
        garo = Input.GetAxisRaw("Horizontal");
        sero = Input.GetAxisRaw("Vertical");

        // �÷��̾��� ����� �ִϸ��̼� ������Ʈ
        if (garo < 0) // ����
        {
            sp.flipX = true;
            animator.SetTrigger("run");
        }
        else if (garo > 0) // ������
        {
            sp.flipX = false;
            animator.SetTrigger("run");
        }
        else if (sero != 0)
        {
            animator.SetTrigger("run");
        }

        // ���� ���� ����
        if (garo < 0) dirVec = Vector3.left; // ����
        else if (garo > 0) dirVec = Vector3.right; // ������

        // ������Ʈ ��ĵ
        if (Input.GetButtonDown("Jump") && scanObject != null)
        {
            UnityEngine.Debug.Log("�̰���: " + scanObject.name);
        }

        // ���� �Է� (��Ŭ��)
        if (Input.GetMouseButtonDown(0) && !attacked)
        {
            Attack();
        }
    }

    void Attack()
    {
        AttackTrue(); // ���� �� �÷��� ����
        animator.SetTrigger("Attack"); // ���� �ִϸ��̼� Ʈ���� ����

        // ���콺 Ŭ�� ��ġ ���
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 2D �����̹Ƿ� z���� 0���� ����

        // Ŭ�� �������� ��������Ʈ �ø�
        if (mousePos.x < transform.position.x)
        {
            sp.flipX = true; // �������� Ŭ��
        }
        else
        {
            sp.flipX = false; // ���������� Ŭ��
        }

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
    }

    IEnumerator DisableCollider(BoxCollider2D collider)
    {
        yield return new WaitForSeconds(0.1f); // �ݶ��̴��� 0.1�� ���� Ȱ��ȭ
        collider.enabled = false;
        AttackFalse(); // ���� �� �÷��� ����
    }

    void FixedUpdate()
    {
        // X, Y�� �̵� ó��
        Vector2 moveDirection = new Vector2(garo, sero).normalized; // ����ȭ�Ͽ� ���⸸ ����
        rigid.velocity = moveDirection * speed;

        // �÷��̾� ������ ���� ������ ����
        UnityEngine.Debug.DrawRay(rigid.position, dirVec * Length, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, Length, LayerMask.GetMask("Enemy"));

        // ���� ������!
        if (rayHit.collider != null) scanObject = rayHit.collider.gameObject;
        else scanObject = null;
    }
}
