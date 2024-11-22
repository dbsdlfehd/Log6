using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    // ������ �Լ�
    public void ExecuteFunction()
    {
        Debug.Log($"{gameObject.name}��(��) Ŭ���Ǿ����ϴ�!");
        // ���⿡ �����ϰ��� �ϴ� ���� �߰�
    }
}

public class InputManager : MonoBehaviour
{
    void Update()
    {
        // ���콺 ��Ŭ�� ����
        if (Input.GetMouseButtonDown(0))
        {
            CheckObjectUnderMouse();
        }
    }

    void CheckObjectUnderMouse()
    {
        // ī�޶󿡼� ���콺 ��ġ�� Ray ����
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.Log("�� Ŭ�� ����");
        // Ray�� Collider�� �浹�ϸ�
        if (Physics.Raycast(ray, out hit))
        {
            //�浹�� ������Ʈ�� ��ũ��Ʈ ��������
            ClickableObject clickable = hit.collider.GetComponent<ClickableObject>();
            if (clickable != null)
            {
                // �ش� ������Ʈ�� �Լ� ����
                clickable.ExecuteFunction();
            }
        }
    }
}
