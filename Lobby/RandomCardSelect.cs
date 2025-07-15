using System.Collections.Generic;
using UnityEngine;

public class RandomCardSelect : MonoBehaviour
{
    public List<RandomCard> deck = new List<RandomCard>();
    public List<RandomCard> result = new List<RandomCard>();
    private int totalWeight = 0;

    private const string FirstLaunchKey = "HasInitialCards";

    private void Awake()
    {
        LoadUnitData();
    }

    void Start()
    {
        // 전체 확률 합산
        totalWeight = 0;
        foreach (var card in deck)
            totalWeight += card.weight;

        // 최초 실행 시에만 스타터 카드 추가
        if (PlayerPrefs.GetInt(FirstLaunchKey, 0) == 0)
        {
            int[] starterCardIds = { 1, 3, 7 };
            foreach (int id in starterCardIds)
            {
                RandomCard found = deck.Find(c => c.id == id);
                if (found != null)
                {
                    result.Add(new RandomCard(found));
                }
            }

            SaveUnitData(); // 저장
            PlayerPrefs.SetInt(FirstLaunchKey, 1); // 다시 추가되지 않도록 설정
            PlayerPrefs.Save();
        }
    }

    public RandomCard RCard()
    {
        int selectNum = Mathf.RoundToInt(totalWeight * Random.Range(0f, 1f));
        int weightSum = 0;

        foreach (var card in deck)
        {
            weightSum += card.weight;
            if (selectNum <= weightSum)
                return new RandomCard(card);
        }

        return null;
    }

    public List<RandomCard> ResultSelect5()
    {
        List<RandomCard> fiveResults = new List<RandomCard>();
        for (int i = 0; i < 5; i++)
        {
            RandomCard card = RCard();
            result.Add(card);
            fiveResults.Add(card);
        }
        return fiveResults;
    }

    public void ClearResults()
    {
        result.Clear();
    }

    public void SaveUnitData()
    {
        PlayerPrefs.SetInt("ResultCount", result.Count);

        for (int i = 0; i < result.Count; i++)
        {
            PlayerPrefs.SetInt($"UnitResult_{i}_ID", result[i].id);
            PlayerPrefs.SetInt($"UnitResult_{i}_Level", result[i].level);
        }

        PlayerPrefs.Save();
    }

    public void LoadUnitData()
    {
        result.Clear();
        int count = PlayerPrefs.GetInt("ResultCount", 0);

        for (int i = 0; i < count; i++)
        {
            int id = PlayerPrefs.GetInt($"UnitResult_{i}_ID", -1);
            int level = PlayerPrefs.GetInt($"UnitResult_{i}_Level", 1);

            if (id != -1)
            {
                RandomCard found = deck.Find(c => c.id == id);
                if (found != null)
                {
                    RandomCard newCard = new RandomCard(found);
                    newCard.level = level;
                    result.Add(newCard);
                }
            }
        }
    }

}
