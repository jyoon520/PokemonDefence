using UnityEngine;
using UnityEngine.UI;

public class BuyUnitManager : MonoBehaviour
{
    public GameObject Unit1;
    public GameObject Unit2;
    public GameObject Unit3;
    public GameObject firstRepairUI;
    SpawnManager spawnManager;

    public Button buyUnit1Button;
    public Button buyUnit2Button;
    public Button buyUnit3Button;

    public int gridWidth = 3;
    public int gridHeight = 3;
    public float cellSize = 1.5f;
    public Vector3 gridOrigin = Vector3.zero;

    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        spawnManager = FindObjectOfType<SpawnManager>();
        firstRepairUI.SetActive(true);

        buyUnit1Button.onClick.AddListener(() => TryBuyUnit(Unit1, 5));
        buyUnit2Button.onClick.AddListener(() => TryBuyUnit(Unit2, 5));
        buyUnit3Button.onClick.AddListener(() => TryBuyUnit(Unit3, 5));
    }

    void TryBuyUnit(GameObject unitPrefab, int cost)
    {
        if (gameManager != null && gameManager.gold >= cost)
        {
            Vector3? emptyCell = FindEmptyCell();
            if (emptyCell.HasValue)
            {
                Instantiate(unitPrefab, emptyCell.Value, Quaternion.Euler(0,180,0));
                gameManager.gold -= cost;
            }
            else
            {
                Debug.Log("ºó Ä­ÀÌ ¾ø½À´Ï´Ù!");
            }
            firstRepairUI.SetActive(false);
            spawnManager.waitingForRepair = false;
        }
        else
        {
            Debug.Log("°ñµå ºÎÁ·");
        }
    }

    Vector3? FindEmptyCell()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

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
}
