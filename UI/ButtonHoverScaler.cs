using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 originalScale;
    Vector3 targetScale;
    bool isHovering = false;
    float scaleSpeed = 8f;

    Button button; // 버튼 참조 추가

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale * 1.1f;
        button = GetComponent<Button>();
    }

    void Update()
    {
        // 버튼이 null이 아니고 비활성화되어 있으면 원래 크기로 복귀
        if (button != null && !button.interactable)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * scaleSpeed);
            return;
        }

        // 일반적인 호버 효과 처리
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
