using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitLevelManager : MonoBehaviour
{
    public static UnitLevelManager Instance;

    // �� ���� ID�� ���� ����
    private Dictionary<int, int> unitLevels = new Dictionary<int, int>();

    void Awake()
    {
        // �̱��� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ���� �ÿ��� ����
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

        return 1; // �⺻ ����
    }

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    public void SetLevel(int id, int level)
    {
        if (level < 1) level = 1;

        unitLevels[id] = level;
        Debug.Log($"[UnitLevelManager] ID {id} ������ {level}�� ������");
    }

    public void ResetAllLevels()
    {
        unitLevels.Clear();
        Debug.Log("[UnitLevelManager] ��� ���� ���� �ʱ�ȭ��");
    }


    public void PrintAllLevels()
    {
        foreach (var pair in unitLevels)
        {
            Debug.Log($"ID: {pair.Key}, ����: {pair.Value}");
        }
    }
}
