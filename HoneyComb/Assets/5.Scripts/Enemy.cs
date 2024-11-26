using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("�ӵ�")]
    public float speed;
	private Rigidbody2D rb;//�߷�
	private Player player;//�÷��̾�
	private PlayerAction playerAction;
	private PrefabSpawner prefabSpawner;

    private Rigidbody2D target;

	[Header("���� ����")]
    public int SenserRangeX = 3;
    public int SenserRangeY = 3;

	[Header("ü��")]
	private int maxHP = 0; // �ִ� ü�� ����
    public int nowHP; // ���� ü�� ����

	[Header("�÷��̾� ������ �ε�")]
    public Scanner scanner;
    public Scanner2 scanner2;

    bool isLive;

	[Header("�̰��� �����ΰ�?")]
	public bool isBoss;

	Rigidbody2D rigid;
    SpriteRenderer spriter;

	//ü�¹� ������ 
	[SerializeField]					// private�� ������ �ܺο��� ������ �� �ְ� �ٲ���
	private GameObject prfHpBar;		// ������ ü�¹�

	RectTransform bghp_bar;				// bghp_bar ��ο� ��� ü�¹�
	Image hp_bar;						// hp_bar ���� ü�¹�

	public float height = 1.7f;         // ü�¹� Y ����

	[Header("������ ��ġ ǥ��")]
	public TextMeshProUGUI damage_text;
	public GameObject damage_text_prf;

	private void SetEnemyStatus(int _maxHP) // �� ü�� ���� �Լ�
    {
        maxHP = _maxHP;
        nowHP = _maxHP;
    }

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		player = FindObjectOfType<Player>();// ������ ����ߵ� (�ʱ�ȭ)
		SetEnemyStatus(maxHP);				// ü�� ��ġ ����

		// prfHpBar �������� �̿��� canvas���ٰ� ü�¹� ����.
		bghp_bar = Instantiate(prfHpBar, GameObject.Find("Canvas").transform).GetComponent<RectTransform>(); // bghp_bar����
		hp_bar = bghp_bar.transform.GetChild(0).GetComponent<Image>(); // bghp_bar�� �ڽ� ������Ʈ ������Ʈ ��������
	}

	void Update()
	{
		if (bghp_bar == null)
		{
			//Debug.LogWarning("bghp_bar�� �ı��Ǿ����ϴ�.");
			return;
		}

		// ī�޶� ���� ���� ü�¹� ��ǥ ��ġ ����
		Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
		bghp_bar.position = _hpBarPos; // �ش� ��ǥ�� ��ġ �����ϱ�

		hp_bar.fillAmount = (float)nowHP / (float)maxHP; // ü�� ��ġ �����ϱ�
	}

	void Awake()
    {
		if (!isBoss)// �Ϲݸ���
		{
			maxHP = TableManager.Enemy1HP; // �ִ� ü�� ����
		}
		else if (isBoss) // ����
		{
			maxHP = TableManager.BossHP; // �ִ� ü�� ����
		}
		rigid = GetComponent<Rigidbody2D>();
        target = GetComponent<Rigidbody2D>();
		spriter = GetComponent<SpriteRenderer>();
        scanner = GetComponent<Scanner>(); //�ٰŸ� ���ݿ� ��ĵ
        scanner2 = GetComponent<Scanner2>(); //���Ÿ� ���ݿ� ��ĵ
		playerAction = FindObjectOfType<Player>().GetComponent<PlayerAction>();
		prefabSpawner = FindAnyObjectByType<PrefabSpawner>();
	}

	void FixedUpdate()
    {
		if(player.EnmeyDown == true)
        {
			EnemyDead();
			//GetComponent<Image>();     // ü�¹�(��) �ı�
			//GetComponent<RectTransform>();   // ü�¹�(���) �ı�
			//Destroy(gameObject); // �� (�ڱ��ڽ�) �ı�
		}

		Vector2 direction = player.transform.position - transform.position;

		int X = Math.Abs(Mathf.RoundToInt(direction.x));
		int Y = Math.Abs(Mathf.RoundToInt(direction.y));

		if (X <= SenserRangeX && Y <= SenserRangeY)//������ ������
		{
			//���󰣴�.
			Vector2 nextVec = direction.normalized * speed * Time.fixedDeltaTime;
			rigid.MovePosition(rigid.position + nextVec);
			rigid.velocity = Vector2.zero;

			//���� ������ �ø����ִ� ��
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

	// �÷��̾����� ���� ������ ����� ǥ��
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			int damage = player.Atk;				// �÷��̾� �����


			StartCoroutine(ShowDamageText(damage)); // ������� ǥ��
			nowHP = nowHP - damage;
		}

		// �Ϲݸ� �״� �Լ�
        if(nowHP < 0 && isBoss == false)			// ü���� 0���� ������
        {
			EnemyDead();
		}
		// �� �״� �Լ�
		else if(nowHP < 0 && isBoss == true)
		{
			BossDead();
		}
	}

	void BossDead()
	{
		Destroy(gameObject);
		prefabSpawner.RoomEnemyCount++;        // �� ���� Ƚ�� 1 �þ
		Destroy(bghp_bar.gameObject);          // ü�¹� ����
	}

	void EnemyDead()
    {
		nowHP = 0;
		if (isBoss == false)
		{
			Destroy(gameObject);
		}
		prefabSpawner.RoomEnemyCount++;        // �� ���� Ƚ�� 1 �þ

		if(bghp_bar != null) // ������ ��
		{
			Destroy(bghp_bar.gameObject);          // ü�¹� ����
		}
	}

	// ����� �ؽ�Ʈ ���� �� 1�� �� ����
	IEnumerator ShowDamageText(int damage)
	{
		// �ؽ�Ʈ ������ ����
		GameObject dmgText = Instantiate(damage_text_prf, GameObject.Find("Canvas").transform);
		TextMeshProUGUI dmgTextComponent = dmgText.GetComponent<TextMeshProUGUI>();

		// �ؽ�Ʈ ����� ��ġ ����
		dmgTextComponent.text = damage.ToString();

		// �ʱ� ��ġ ���� (���� -> ��ũ�� ��ǥ ��ȯ)
		Vector3 startPosition = Camera.main.WorldToScreenPoint(transform.position);
		dmgText.transform.position = startPosition;

		// �̵��� y�� ����
		float moveDistance = 50f; // y�� �̵� �Ÿ� (Canvas ����)
		float duration = 0.2f;    // ���� �ð�
		float elapsedTime = 0f;   // ��� �ð�

		// �ڷ�ƾ���� �ؽ�Ʈ ��ġ �̵�
		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;

			// ���� ��ġ ���
			Vector3 currentPosition = dmgText.transform.position;
			currentPosition.y = Mathf.Lerp(startPosition.y, startPosition.y + moveDistance, elapsedTime / duration);
			dmgText.transform.position = currentPosition;

			yield return null; // ���� �����ӱ��� ���
		}

		// �ؽ�Ʈ ����
		Destroy(dmgText);
	}

}
