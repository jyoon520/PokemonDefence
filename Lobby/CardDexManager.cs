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

    // ī�� �ϳ� ���
    public void RegisterCard(RandomCard card, bool saveNow = true)
    {
        if (!ownedCardIDs.Contains(card.id))
        {
            ownedCardIDs.Add(card.id);
            if (saveNow)
                SaveDex();
        }
    }

    // ���� ī�� �� ���� ���
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

    // ī�� ���� ����
    public bool HasCard(int cardID)
    {
        return ownedCardIDs.Contains(cardID);
    }

    // ����
    private void SaveDex()
    {
        string saveData = string.Join(",", ownedCardIDs);
        PlayerPrefs.SetString(PlayerPrefsKey, saveData);
        PlayerPrefs.Save();
    }

    // �ҷ�����
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

    // ��ü ���� ī�� ID ��ȯ
    public List<int> GetOwnedCardIDs()
    {
        return new List<int>(ownedCardIDs);
    }

    // �ʱ�ȭ
    public void ClearDex()
    {
        ownedCardIDs.Clear();
        PlayerPrefs.DeleteKey(PlayerPrefsKey);
        PlayerPrefs.Save();
        Debug.Log("CardDex cleared");
    }
}
