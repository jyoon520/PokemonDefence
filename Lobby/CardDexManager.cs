using System.Collections.Generic;
using UnityEngine;

public class CardDexManager : MonoBehaviour
{
    public static CardDexManager Instance { get; private set; }

    private HashSet<int> ownedCardIDs = new HashSet<int>();
    private const string PlayerPrefsKey = "CardDex";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        LoadDex();
    }

    // 카드 하나 등록
    public void RegisterCard(RandomCard card, bool saveNow = true)
    {
        if (!ownedCardIDs.Contains(card.id))
        {
            ownedCardIDs.Add(card.id);
            if (saveNow)
                SaveDex();
        }
    }

    // 여러 카드 한 번에 등록
    public void RegisterMultipleCards(List<RandomCard> cards)
    {
        bool changed = false;
        foreach (var card in cards)
        {
            if (!ownedCardIDs.Contains(card.id))
            {
                ownedCardIDs.Add(card.id);
                changed = true;
            }
        }

        if (changed)
            SaveDex();
    }

    // 카드 소유 여부
    public bool HasCard(int cardID)
    {
        return ownedCardIDs.Contains(cardID);
    }

    // 저장
    private void SaveDex()
    {
        string saveData = string.Join(",", ownedCardIDs);
        PlayerPrefs.SetString(PlayerPrefsKey, saveData);
        PlayerPrefs.Save();
    }

    // 불러오기
    private void LoadDex()
    {
        ownedCardIDs.Clear();
        string saved = PlayerPrefs.GetString(PlayerPrefsKey, "");
        if (!string.IsNullOrEmpty(saved))
        {
            string[] ids = saved.Split(',');
            foreach (var idStr in ids)
            {
                if (int.TryParse(idStr, out int id))
                    ownedCardIDs.Add(id);
            }
        }
    }

    // 전체 소유 카드 ID 반환
    public List<int> GetOwnedCardIDs()
    {
        return new List<int>(ownedCardIDs);
    }

    // 초기화
    public void ClearDex()
    {
        ownedCardIDs.Clear();
        PlayerPrefs.DeleteKey(PlayerPrefsKey);
        PlayerPrefs.Save();
        Debug.Log("CardDex cleared");
    }
}
