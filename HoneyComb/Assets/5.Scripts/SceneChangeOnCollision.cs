using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOnCollision : MonoBehaviour
{
	// �浹���� �� ��ȯ�� ���� �̸�
	public string sceneName;

	public bool isSideViewScene = false;

	private DirectingCameraManager directingCameraManager;

	private void Start()
	{
		directingCameraManager = FindObjectOfType<DirectingCameraManager>();

		// ���̵� �� �� ����
		if (isSideViewScene)
		{
			StartCoroutine(OnlySideView());

		}
	}

	IEnumerator OnlySideView()
	{
		directingCameraManager.darkBg_On();
		yield return new WaitForSeconds(0.1f);
		directingCameraManager.SetOrthoSize(1);
		yield return new WaitForSeconds(0.1f);
		directingCameraManager.darkBg_Off();

		directingCameraManager.ZoomOut();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// �浹�� ��ü�� Ư�� ������ �����ϸ� �� ��ȯ
		if (collision.gameObject.CompareTag("Player")) // ��: "Player" �±װ� ���� ��ü�� �浹 ��
		{
			// �� ��ȯ
			SceneChangeHamSu();
		}
	}

	public void SceneChangeHamSu()
	{
		StartCoroutine(ZoomInAndSceneChange());
	}

	IEnumerator ZoomInAndSceneChange()
	{
		// Zoom-in ���� (1�� ���)
		directingCameraManager.ZoomIn();

		yield return new WaitForSeconds(1f);

		directingCameraManager.darkBg_On();

		yield return new WaitForSeconds(0.4f);

		SceneManager.LoadScene(sceneName);
	}

	IEnumerator ZoomOut()
	{
		// Zoom-in ���� (1�� ���)
		directingCameraManager.ZoomOut();
		yield return new WaitForSeconds(1f);

		directingCameraManager.darkBg_On();
	}
}
