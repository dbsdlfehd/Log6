using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHpShow : MonoBehaviour
{
	public GameObject prefabHP_bar;//ü�¹�(������) ����ġ��
	GameObject canvas;//canvas����ġ��
	private Enemy enemy;

	RectTransform hpBar;//hp�� ��ȯ�ϱ� ���� ��¼��

	Image nowHpBar;//���� ü�¹�

	public float height = 1.7f;//��

	void Start()
	{
		canvas = GameObject.FindWithTag("UI"); // Canvas�� �±׷� �˻�
		if (canvas == null)
		{
			Debug.LogError("Canvas�� ã�� �� �����ϴ�. 'UI' �±װ� �����Ǿ� �ִ��� Ȯ���ϼ���.");
			return;
		}
		enemy = GetComponent<Enemy>();// �� ������Ʈ ã��
		if (enemy != null) // ���� ���� ��쿡�� ü�¹� ����
		{
			hpBar = Instantiate(prefabHP_bar, canvas.transform).GetComponent<RectTransform>();
			nowHpBar = hpBar.transform.GetChild(0).GetComponent<Image>();
		}
	}

	void Update()
	{
		if (hpBar != null && enemy != null)
		{
			Vector3 hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
			hpBar.position = hpBarPos;
			nowHpBar.fillAmount = (float)enemy.nowHP / (float)enemy.maxHP;

			if (enemy.nowHP <= 0)
			{
				Destroy(hpBar.gameObject);
			}
		}
	}

}
