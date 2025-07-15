using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardDexUI : MonoBehaviour
{
    public GameObject cardDexPanel;         // 도감 패널
    public Transform contentParent;         // 카드들이 들어갈 부모 (GridLayoutGroup)
    public GameObject cardSlotPrefab;       // 카드 슬롯 프리팹
    public RandomCardSelect randomCardSelect;  // 랜덤카드셀렉트 참조

    public void OpenDexWithResults()
    {
        cardDexPanel.SetActive(true);

        // 기존 카드 UI 제거
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // RandomCardSelect의 result 리스트에 저장된 카드만 보여줌
        foreach (var card in randomCardSelect.result)
        {
            GameObject slot = Instantiate(cardSlotPrefab, contentParent);
            CardSlotUI slotUI = slot.GetComponent<CardSlotUI>();
            slotUI.Setup(card, true);  // 소유한 카드임을 표시
        }
    }
}
