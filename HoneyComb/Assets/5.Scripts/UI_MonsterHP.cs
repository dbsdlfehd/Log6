using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MonsterHP : MonoBehaviour
{
	private Rigidbody2D rb;//�߷�
	private Player player;//�÷��̾�
	private MonsterHP monsterHP;

	//ü�¹� ������ 
	[SerializeField]                    // private�� ������ �ܺο��� ������ �� �ְ� �ٲ���
	private GameObject prfHpBar;        // ������ ü�¹�

	RectTransform bghp_bar;             // bghp_bar ��ο� ��� ü�¹�
	Image hp_bar;                       // hp_bar ���� ü�¹�

	public float height = 1.7f;         // ü�¹� Y ����

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		player = FindObjectOfType<Player>();    // ������ ����ߵ� (�ʱ�ȭ)
		monsterHP = GetComponent<MonsterHP>();  // �ڱ� ������Ʈ�� ���� �ִ� ��ũ��Ʈ ģ��

		// prfHpBar �������� �̿��� canvas���ٰ� ü�¹� ����.
		bghp_bar = Instantiate(prfHpBar, GameObject.Find("Canvas").transform).GetComponent<RectTransform>(); // bghp_bar����
		hp_bar = bghp_bar.transform.GetChild(0).GetComponent<Image>(); // bghp_bar�� �ڽ� ������Ʈ ������Ʈ ��������
		bghp_bar.pivot = new Vector2(0.9f, 0.5f); // �߾� ���� (�ʿ信 ���� ���� ����)

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

		hp_bar.fillAmount = (float)monsterHP.nowHP / (float)monsterHP.maxHP; // ü�� ��ġ �����ϱ�
	}
	public void DestroyHP_UI()
	{
		Destroy(bghp_bar.gameObject);
	}
}
