using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaUI : MonoBehaviour
{
    [Header("����")]
    public RandomCardSelect gachaSystem;
    public LobbyManager lobbyManager;
    public Transform resultPanelParent;
    public GameObject cardSlotPrefab;

    [Header("UI ������Ʈ")]
    public GameObject cardResultUI;
    public GameObject cardgetUI;

    public Button clearButton;

    public void OnGachaTenButtonClicked()
    {
        if (lobbyManager.crystal < 5)
        {
            Debug.Log("ũ����Ż�� �����մϴ�. ��í�� �� �� �����ϴ�.");
            return;
        }

        lobbyManager.crystal -= 5;
        lobbyManager.UpdateCrystalText();

        cardResultUI.SetActive(true);
        cardgetUI.SetActive(false);

        List<RandomCard> results = gachaSystem.ResultSelect5();

        ClearResultUI();

        foreach (RandomCard card in results)
        {
            GameObject slot = Instantiate(cardSlotPrefab, resultPanelParent);
            CardSlotUI slotUI = slot.GetComponent<CardSlotUI>();
            slotUI.Setup(card, true);
        }

        // ���⿡�� ������ �� ���� ID ���
        if (CardDexManager.Instance != null)
            CardDexManager.Instance.RegisterMultipleCards(results);

        lobbyManager.SaveProgress();
    }

    public void ClearResultUI()
    {
        foreach (Transform child in resultPanelParent)
            Destroy(child.gameObject);
    }

    public void OnClearAllButtonClicked()
    {
        gachaSystem.ClearResults();

        if (CardDexManager.Instance != null)
            CardDexManager.Instance.ClearDex();

        ClearResultUI();
    }
}
