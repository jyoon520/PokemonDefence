using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardDexUI : MonoBehaviour
{
    public GameObject cardDexPanel;         // ���� �г�
    public Transform contentParent;         // ī����� �� �θ� (GridLayoutGroup)
    public GameObject cardSlotPrefab;       // ī�� ���� ������
    public RandomCardSelect randomCardSelect;  // ����ī�弿��Ʈ ����

    public void OpenDexWithResults()
    {
        cardDexPanel.SetActive(true);

        // ���� ī�� UI ����
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // RandomCardSelect�� result ����Ʈ�� ����� ī�常 ������
        foreach (var card in randomCardSelect.result)
        {
            GameObject slot = Instantiate(cardSlotPrefab, contentParent);
            CardSlotUI slotUI = slot.GetComponent<CardSlotUI>();
            slotUI.Setup(card, true);  // ������ ī������ ǥ��
        }
    }
}
