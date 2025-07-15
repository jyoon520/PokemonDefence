using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaUI : MonoBehaviour
{
    [Header("참조")]
    public RandomCardSelect gachaSystem;
    public LobbyManager lobbyManager;
    public Transform resultPanelParent;
    public GameObject cardSlotPrefab;

    [Header("UI 오브젝트")]
    public GameObject cardResultUI;
    public GameObject cardgetUI;

    public Button clearButton;

    public void OnGachaTenButtonClicked()
    {
        if (lobbyManager.crystal < 5)
        {
            Debug.Log("크리스탈이 부족합니다. 가챠를 할 수 없습니다.");
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

        // 여기에서 도감에 한 번에 ID 등록
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
