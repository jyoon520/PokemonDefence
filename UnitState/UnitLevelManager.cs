using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitLevelManager : MonoBehaviour
{
    public static UnitLevelManager Instance;

    // 각 유닛 ID별 레벨 저장
    private Dictionary<int, int> unitLevels = new Dictionary<int, int>();

    void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 변경 시에도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetLevel(int id)
    {
        if (unitLevels.TryGetValue(id, out int level))
        {
            return level;
        }

        return 1; // 기본 레벨
    }

    /// <summary>
    /// 유닛 레벨 설정
    /// </summary>
    public void SetLevel(int id, int level)
    {
        if (level < 1) level = 1;

        unitLevels[id] = level;
        Debug.Log($"[UnitLevelManager] ID {id} 레벨이 {level}로 설정됨");
    }

    public void ResetAllLevels()
    {
        unitLevels.Clear();
        Debug.Log("[UnitLevelManager] 모든 유닛 레벨 초기화됨");
    }


    public void PrintAllLevels()
    {
        foreach (var pair in unitLevels)
        {
            Debug.Log($"ID: {pair.Key}, 레벨: {pair.Value}");
        }
    }
}
