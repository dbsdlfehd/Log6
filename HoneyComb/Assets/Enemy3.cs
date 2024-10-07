using UnityEngine;

public class Enemy3 : MonoBehaviour
{
	public float speed;//�ӵ�
	private Rigidbody2D rb;//�߷�
	private Player player;//�÷��̾�

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		// Scene���� Player �±׸� ���� ������Ʈ�� ã���ϴ�.
		player = FindObjectOfType<Player>();
	}

	void FixedUpdate()
	{
		// �÷��̾� �������� ���󰩴ϴ�.
		Vector2 direction = player.transform.position - transform.position;
		rb.velocity = direction.normalized * speed;
	}
}