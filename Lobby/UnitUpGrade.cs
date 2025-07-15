using UnityEngine;
using UnityEngine.UI;

public class UnitUpGrade : MonoBehaviour
{
    public RandomCardSelect randomCardSelect;
    public LobbyManager lobbyManager;
    public Button[] unitUpgradeButtons;
    public Image[] unitEXPBars;
    public Text[] duplicationCards;
    public Text[] unitLevelText;
    public int MaxEXP = 100;

    public int[] unitCurrentEXP;
    public int[] unitLevels;
    public int[] cardCounts;

    void Start()
    {
        unitCurrentEXP = new int[unitEXPBars.Length];
        unitLevels = new int[unitEXPBars.Length];
        cardCounts = new int[duplicationCards.Length];

        for (int i = 0; i < unitLevels.Length; i++)
            unitLevels[i] = 1;

        // 버튼 리스너 설정
        for (int i = 0; i < unitUpgradeButtons.Length; i++)
        {
            int index = i;
            unitUpgradeButtons[i].onClick.AddListener(() => OnUnitUpgradeButtonsClick(index));
        }

        // 불러오기
        LoadUnitData();

        CountDuplicationCards();
        UpdateUnitLevel();
    }

    void OnUnitUpgradeButtonsClick(int unitIndex)
    {
        if (lobbyManager.gold < 1000)
        {
            Debug.Log("골드가 부족합니다.강화를 할 수 없습니다.");
            return;
        }
        // 버튼 인덱스(unitIndex) -> 1~9
        // 마지막 버튼 인덱스 9 -> Id = 0 에 대응
        // 나머지는 인덱스 + 1
        int targetCardId = (unitIndex == unitUpgradeButtons.Length - 1) ? 0 : unitIndex + 1;
        int count = 0;
        bool usedCard = false;

        foreach (var card in randomCardSelect.result)
        {
            if (card.id == targetCardId)
                count++;
        }

        if (count > 1)
        {
            for (int i = 0; i < randomCardSelect.result.Count; i++)
            {
                if (randomCardSelect.result[i].id == targetCardId)
                {
                    randomCardSelect.result.RemoveAt(i);
                    Debug.Log($"ID:{targetCardId} 카드 1개 제거됨 (최소 1장 유지)");
                    usedCard = true;
                    break;
                }
            }
        }
        else
        {
            Debug.Log($"ID:{targetCardId} 카드는 1장뿐이므로 제거하지 않음");
        }

        if (lobbyManager.gold >= 1000 && count > 1)
        {
            lobbyManager.gold -= 1000;
            lobbyManager.UpdateGoldText();
        }
        CountDuplicationCards();
        SaveUnitData();
        AddEXP(unitIndex, usedCard ? 20 : 0);
    }

    void AddEXP(int unitIndex, int amount)
    {
        if (unitIndex < 0 || unitIndex >= unitCurrentEXP.Length) return;

        unitCurrentEXP[unitIndex] += amount;

        if (unitCurrentEXP[unitIndex] >= MaxEXP)
        {
            unitCurrentEXP[unitIndex] -= MaxEXP;
            unitLevels[unitIndex]++;

            int targetCardId = (unitIndex == unitUpgradeButtons.Length - 1) ? 0 : unitIndex + 1;

            foreach (var card in randomCardSelect.result)
            {
                if (card.id == targetCardId)
                {
                    card.level = unitLevels[unitIndex];
                }
            }

            UnitLevelManager.Instance.SetLevel(targetCardId, unitLevels[unitIndex]);
            Debug.Log($"유닛 {targetCardId} 레벨업! 현재 레벨: {unitLevels[unitIndex]}");
            UpdateUnitLevel();
        }

        UpdateExpUI(unitIndex);

        SaveUnitData(); // UI, 경험치, 레벨 저장
        randomCardSelect.SaveUnitData(); // 카드 내부 레벨 저장 추가
        lobbyManager.SaveProgress();     // 골드 등 저장
    }



    // 경험치UI 업데이트
    void UpdateExpUI(int unitIndex)
    {
        if (unitIndex < 0 || unitIndex >= unitEXPBars.Length) return;

        float fillRatio = (float)unitCurrentEXP[unitIndex] / MaxEXP;
        unitEXPBars[unitIndex].fillAmount = Mathf.Min(fillRatio, 1.0f);
    }

    // 중복카드 갯수 표기
    public void CountDuplicationCards()
    {
        cardCounts = new int[duplicationCards.Length];

        foreach (var card in randomCardSelect.result)
        {
            int index = card.id;
            if (index >= 0 && index < cardCounts.Length)
            {
                cardCounts[index]++;
            }
        }

        for (int i = 0; i < duplicationCards.Length; i++)
        {
            duplicationCards[i].text = $"{cardCounts[i]}";
        }
    }

    public void UpdateUnitLevel()
    {
        for (int i = 0; i < unitLevelText.Length; i++)
        {
            unitLevelText[i].text = $"Lv.{unitLevels[i]}";
        }
    }
    public void SaveUnitData()
    {
        for (int i = 0; i < unitLevels.Length; i++)
        {
            PlayerPrefs.SetInt($"UnitLevel_{i}", unitLevels[i]);
            PlayerPrefs.SetInt($"UnitEXP_{i}", unitCurrentEXP[i]);
            PlayerPrefs.SetInt($"CardCounts_{i}", cardCounts[i]);
        }

        PlayerPrefs.Save();
    }

    public void LoadUnitData()
    {
        for (int i = 0; i < unitLevels.Length; i++)
        {
            unitLevels[i] = PlayerPrefs.GetInt($"UnitLevel_{i}", unitLevels[i]);
            unitCurrentEXP[i] = PlayerPrefs.GetInt($"UnitEXP_{i}", unitCurrentEXP[i]);
            cardCounts[i] = PlayerPrefs.GetInt($"CardCounts_{i}", cardCounts[i]);
            UpdateExpUI(i);
        }

        UpdateUnitLevel();

        for (int i = 0; i < duplicationCards.Length; i++)
        {
            duplicationCards[i].text = $"{cardCounts[i]}";
        }
    }
}
