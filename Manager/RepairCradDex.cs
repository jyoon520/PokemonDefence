using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepairCradDex : MonoBehaviour
{
    RepairCradDex repairCardDex;
    GameManager gameManager;
    SpawnManager spawnManager;

    public int unitId;
    public int unitId2;
    public int unitId3;

    public int baseATK;
    public int baseATK2;
    public int baseATK3;

    public TMP_Text levelText;
    public TMP_Text levelText2;
    public TMP_Text levelText3;

    public TMP_Text atkText;
    public TMP_Text atkText2;
    public TMP_Text atkText3;

    public Text goldText;

    [Header("유닛 데이터")]
    public UnitData[] allUnits = new UnitData[10]; // 전체 10개 유닛 정보

    [Header("UI 참조")]
    public TMP_Text[] levelTexts = new TMP_Text[3];
    public TMP_Text[] atkTexts = new TMP_Text[3];
    public TMP_Text[] nameTexts = new TMP_Text[3];
    public Image[] unitImages = new Image[3];

    private int[] selectedUnitIds = new int[3];
    private UnitData[] selectedUnits = new UnitData[3];

    [Header("가격 및 구매 UI")]
    public TMP_Text[] priceTexts = new TMP_Text[3];

    [Header("구매 버튼")]
    public Button[] buyButtons = new Button[3];

    public Vector3 gridOrigin = Vector3.zero;

    // 세레나 구매
    public GameObject serenaPrefeb;
    public Button serenaBuyButton;
    int serenaFirstBuy = 0;

    // 정인
    // 구매 체크
    [Header("구매 체크")]
    public TMP_Text buyText;
    public GameObject buySerena;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        spawnManager = FindObjectOfType<SpawnManager>();
        UpdateLevelUI();
        UpdateRanDomCardUI();

        for (int i = 0; i < buyButtons.Length; i++)
        {
            int index = i;
            buyButtons[i].onClick.AddListener(() => BuySelectedUnit(index));
        }

        serenaBuyButton.onClick.AddListener(OnSerenaBuyButtonClick);

        goldText.text = $"{gameManager.gold}";
    }

    private void Update()
    {
        goldText.text = $"{gameManager.gold}";
    }

    List<UnitData> GetAvailableUnitData()
    {
        List<UnitData> result = new List<UnitData>();
        int[] starterCardIds = { 1, 3, 7 };

        // starterCardIds 유닛을 먼저 추가
        foreach (int id in starterCardIds)
        {
            UnitData unit = System.Array.Find(allUnits, u => u != null && u.id == id);
            if (unit != null && !result.Any(u => u.id == id))
                result.Add(unit);
        }

        // CardDex에서 가진 카드들 중 중복 없이 추가
        foreach (var unit in allUnits)
        {
            if (CardDexManager.Instance.HasCard(unit.id) && !result.Any(u => u.id == unit.id))
            {
                result.Add(unit);
            }
        }

        return result;
    }


    public void UpdateLevelUI()
    {
        int level = UnitLevelManager.Instance.GetLevel(unitId);
        if (levelText != null)
        {
            levelText.text = $"Level: {level}";
        }

        int level2 = UnitLevelManager.Instance.GetLevel(unitId2);
        if (levelText2 != null)
        {
            levelText2.text = $"Level: {level2}";
        }

        int level3 = UnitLevelManager.Instance.GetLevel(unitId3);
        if (levelText3 != null)
        {
            levelText3.text = $"Level: {level3}";
        }

        int atk = AttackPower(level, baseATK);
        if (atkText != null)
        {
            atkText.text = $"ATK: {atk}";
        }
        int atk2 = AttackPower(level2, baseATK2);
        if (atkText2 != null)
        {
            atkText2.text = $"ATK: {atk2}";
        }
        int atk3 = AttackPower(level3, baseATK3);
        if (atkText3 != null)
        {
            atkText3.text = $"ATK: {atk3}";
        }
    }

    public void UpdateRanDomCardUI()
    {
        List<UnitData> available = GetAvailableUnitData();

        if (available.Count < 3)
        {
            UnityEngine.Debug.LogWarning("도감에 등록된 유닛이 3개 미만입니다.");
            return;
        }

        List<UnitData> selected = new List<UnitData>();
        while (selected.Count < 3)
        {
            var candidate = available[Random.Range(0, available.Count)];
            if (!selected.Contains(candidate))
                selected.Add(candidate);
        }

        for (int i = 0; i < 3; i++)
        {
            var unit = selected[i];
            int level = UnitLevelManager.Instance.GetLevel(unit.id);
            int atk = AttackPower(level, unit.baseATK);

            // 기존 UI 갱신
            if (levelTexts[i] != null) levelTexts[i].text = $"Level: {level}";
            if (atkTexts[i] != null) atkTexts[i].text = $"ATK: {atk}";
            if (nameTexts[i] != null) nameTexts[i].text = unit.unitName;
            if (unitImages[i] != null) unitImages[i].sprite = unit.unitSprite;
            if (priceTexts[i] != null) priceTexts[i].text = $"{unit.price}";

            selectedUnitIds[i] = unit.id;
            selectedUnits[i] = unit;
        }

    }

    void BuySelectedUnit(int index)
    {
        var unit = selectedUnits[index];
        int price = unit.price;

        if (gameManager.gold < price)
        {
            UnityEngine.Debug.Log("골드 부족");
            buyText.text = "코인이 부족합니다!";
            return;
        }
        else
        {
            buyText.text = "";
        }

        Vector3? spawnPos = FindEmptyCell();

        if (!spawnPos.HasValue)
        {
            UnityEngine.Debug.Log("빈 칸이 없습니다!");
            buyText.text = "자리가 없습니다!";
            return;
        }
        else
        {
            buyText.text = "";
        }

            Instantiate(unit.prefab, spawnPos.Value, Quaternion.Euler(0, 180, 0));
        gameManager.gold -= price;
    }

    void OnSerenaBuyButtonClick()
    {
        if (serenaFirstBuy > 0)
        {
            UnityEngine.Debug.Log("더이상 구매 할 수 없는 유닛입니다.");
        }

        if (gameManager.gold < 30 && serenaFirstBuy == 0)
        {
            UnityEngine.Debug.Log("골드 부족");
            buyText.text = "코인이 부족합니다!";
            return;
        }
        else
        {
            buyText.text = "";
        }

        Vector3? spawnPos = FindEmptyCell();

        if (!spawnPos.HasValue)
        {
            UnityEngine.Debug.Log("빈 칸이 없습니다!");
            buyText.text = "자리가 없습니다!";
            return;
        }
        else
        {
            buyText.text = "";
        }

        if (gameManager.gold >= 30 && serenaFirstBuy == 0)
        {
            Instantiate(serenaPrefeb, spawnPos.Value, Quaternion.Euler(0, 180, 0));
            gameManager.gold -= 30;
            serenaFirstBuy++;
            buySerena.SetActive(true);
            serenaBuyButton.interactable = false;
        }
    }
    Vector3? FindEmptyCell()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

        int gridWidth = 3;
        int gridHeight = 3;
        float cellSize = 1.5f;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector3 cellPos = new Vector3
                (
                    x * cellSize + gridOrigin.x,
                    gridOrigin.y,
                    z * cellSize + gridOrigin.z
                );

                if (!IsCellOccupied(cellPos, units))
                    return cellPos;
            }
        }

        return null;
    }

    bool IsCellOccupied(Vector3 targetPosition, GameObject[] units)
    {
        int targetX = Mathf.RoundToInt(targetPosition.x);
        int targetZ = Mathf.RoundToInt(targetPosition.z);

        foreach (GameObject unit in units)
        {
            Vector3 pos = unit.transform.position;
            int unitX = Mathf.RoundToInt(pos.x);
            int unitZ = Mathf.RoundToInt(pos.z);

            if (unitX == targetX && unitZ == targetZ)
                return true;
        }

        return false;
    }

    public void OpenRepairUI()
    {
        UpdateLevelUI();
        UpdateRanDomCardUI();
    }


    int AttackPower(int level, int baseATK)
    {
        return baseATK + (level - 1) * 3;
    }
}

