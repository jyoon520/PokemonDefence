using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 originalScale;
    Vector3 targetScale;
    bool isHovering = false;
    float scaleSpeed = 8f;

    Button button; // ��ư ���� �߰�

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale * 1.1f;
        button = GetComponent<Button>();
    }

    void Update()
    {
        // ��ư�� null�� �ƴϰ� ��Ȱ��ȭ�Ǿ� ������ ���� ũ��� ����
        if (button != null && !button.interactable)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * scaleSpeed);
            return;
        }

        // �Ϲ����� ȣ�� ȿ�� ó��
        if (isHovering)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * scaleSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button != null && button.interactable)
        {
            isHovering = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}
