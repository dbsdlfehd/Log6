using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    private Transform player;
    public float chaseSpeed; // �̼�
    public float attackCooldown; // ����
    public float attackPreparationTime = 0.5f; // ���� �غ� �ð�
    //public int damage = 10; // ���� ������
    public GameObject attackProjectile; // ���� ����ü
    public float projectileSpeed = 12f; // ����ü �ӵ�

    private bool isAttacking = false; // ���� ���� ������ Ȯ��
    private bool playerInRange = false; // �÷��̾ ���� ���� �ִ��� Ȯ��
    private bool isPreparingAttack = false; // ���� �غ� ������ Ȯ��

    private Vector2 lockedAttackDirection; // ���� �غ� �� ������ ���� ����

	private void Awake()
	{
		player = FindObjectOfType<Player>().transform;    // ������ ����ߵ� (�ʱ�ȭ)
	}

	void Update()
    {
        if (player == null) return;

        if (playerInRange && !isAttacking && !isPreparingAttack)
        {
            StartCoroutine(PrepareAndAttack());
        }
        else if (!isAttacking && !isPreparingAttack)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
    }

    System.Collections.IEnumerator PrepareAndAttack()
    {
        isPreparingAttack = true;
        //Debug.Log("���� �غ�");

        // ���� �غ� �� �÷��̾��� ���� ��ġ�� �������� ���� ����
        lockedAttackDirection = (player.position - transform.position).normalized;

        // ���� �غ� ���� �̵� ����
        yield return new WaitForSeconds(attackPreparationTime);

        isAttacking = true;
        LaunchAttack();
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        isPreparingAttack = false;
    }

    void LaunchAttack()
    {
        if (attackProjectile != null)
        {
            // ����ü ���� �� ������ �������� �߻�
            GameObject projectile = Instantiate(attackProjectile, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = lockedAttackDirection * projectileSpeed;
            }

            //Debug.Log("����!");
            Destroy(projectile, 0.1f); // n�� �� ����ü �ı�
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") )
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
