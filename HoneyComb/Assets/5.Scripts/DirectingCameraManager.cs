using Cinemachine;
using System.Collections;
using UnityEngine;

public class DirectingCameraManager : MonoBehaviour
{
	public CinemachineVirtualCamera virtualCamera; // CMvcam1�� �Ҵ�

	[Header("��ǥ �ܰ� ����")]
	public float zoomSize = 1f;

	[Header("���� �ð�")]
	public float delayTime = 1f;

	private float originalSize; // ī�޶��� �ʱ� ũ�� ����


	[Header("���� �� ���� � �Ƕ���")]
	public GameObject darkBg;

	void Start()
	{
		// ī�޶� �ʱ� ũ�⸦ ����
		originalSize = virtualCamera.m_Lens.OrthographicSize;

		// �ܾƿ� ���� ����
		SetOrthoSize(1);
		ZoomOut();
	}

	// � �Ƕ���� ��ü ȭ�� ������
	public void darkBg_On()
	{
		darkBg.SetActive(true);
	}

	// � �Ƕ���� ��ü ȭ�� ������ �� ���ֱ�
	public void darkBg_Off()
	{
		darkBg.SetActive(false);
	}

	// ī�޶� �� ������ ũ�� ���� �޼���
	public void SetOrthoSize(float size)
	{
		virtualCamera.m_Lens.OrthographicSize = size;
	}

	public void ZoomIn() // ���� �޼���
	{
		StartCoroutine(SmoothZoomTo(zoomSize, delayTime));
	}

	public void ZoomOut() // �ܾƿ� �޼���
	{
		StartCoroutine(SmoothZoomToOrigin(delayTime));
	}


	// �ڿ������� ���� �ϴ� ����
	public IEnumerator SmoothZoomTo(float targetSize, float duration)
	{
		Debug.Log("���� ���� ����");
		float startSize = virtualCamera.m_Lens.OrthographicSize; // ���� ũ��
		float elapsedTime = 0f; // ��� �ð�

		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;

			// ���� ũ�⸦ ���������� ���
			float newSize = Mathf.Lerp(startSize, targetSize, elapsedTime / duration);
			SetOrthoSize(newSize);

			yield return null; // ���� �����ӱ��� ���
		}

		// ��Ȯ�� ��ǥ ������ ����
		SetOrthoSize(targetSize);

	}

	// �ڿ������� �� �ƿ� �ϴ� ����
	public IEnumerator SmoothZoomToOrigin(float duration)
	{
		Debug.Log("�ܾƿ� ���� ����");
		float startSize = virtualCamera.m_Lens.OrthographicSize; // ���� ũ��
		float elapsedTime = 0f; // ��� �ð�

		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;

			// ���� ũ�⸦ ���������� ���
			float newSize = Mathf.Lerp(startSize, originalSize, elapsedTime / duration);
			SetOrthoSize(newSize);

			yield return null; // ���� �����ӱ��� ���
		}

		// ��Ȯ�� ���� ������ ����
		SetOrthoSize(originalSize);
	}
}