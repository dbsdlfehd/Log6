using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHpShow : MonoBehaviour
{
	public GameObject prefabHP_bar;//체력바(프리펩) 끌어치기
	GameObject canvas;//canvas끌어치기
	private Enemy enemy;

	RectTransform hpBar;//hp바 변환하기 위한 어쩌구

	Image nowHpBar;//현재 체력바

	public float height = 1.7f;//모름

	void Start()
	{
		canvas = GameObject.FindWithTag("UI"); // Canvas를 태그로 검색
		if (canvas == null)
		{
			Debug.LogError("Canvas를 찾을 수 없습니다. 'UI' 태그가 지정되어 있는지 확인하세요.");
			return;
		}
		enemy = GetComponent<Enemy>();// 적 오브젝트 찾기
		if (enemy != null) // 적이 있을 경우에만 체력바 생성
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
