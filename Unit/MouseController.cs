using UnityEngine;

public class MouseController : MonoBehaviour
{
    public int gridWidth = 3;
    public int gridHeight = 3;
    public float cellSize = 1.5f;
    public Vector3 gridOrigin = Vector3.zero;

    public GameObject highlightPrefab; // 하이라이트 프리팹
    private GameObject highlightInstance;

    private Transform selectedObject;

    void Start()
    {
        if (highlightPrefab != null)
        {
            highlightInstance = Instantiate(highlightPrefab);
            highlightInstance.SetActive(false);
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("Unit"))
                {
                    selectedObject = hit.transform;

                    if (highlightInstance != null)
                        highlightInstance.SetActive(true);
                }
            }
        }

        if (Input.GetMouseButton(0) && selectedObject != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit groundHit))
            {
                Vector3 point = groundHit.point;

                int x = Mathf.RoundToInt((point.x - gridOrigin.x) / cellSize);
                int z = Mathf.RoundToInt((point.z - gridOrigin.z) / cellSize);

                if (x >= 0 && x < gridWidth && z >= 0 && z < gridHeight)
                {
                    Vector3 snappedPosition = new Vector3(
                        x * cellSize + gridOrigin.x,
                        selectedObject.position.y,
                        z * cellSize + gridOrigin.z
                    );

                    // 셀 점유 여부 검사
                    bool isOccupied = IsCellOccupied(snappedPosition, selectedObject);

                    // 하이라이트 처리
                    if (highlightInstance != null)
                    {
                        if (isOccupied)
                        {
                            highlightInstance.SetActive(false);
                        }
                        else
                        {
                            highlightInstance.transform.position = new Vector3(
                                snappedPosition.x,
                                gridOrigin.y + 0.01f,
                                snappedPosition.z
                            );
                            highlightInstance.SetActive(true);
                        }
                    }

                    // 유닛 이동
                    if (!isOccupied)
                    {
                        selectedObject.position = snappedPosition;
                    }
                }
                else
                {
                    // 경계 밖이면 하이라이트 숨기기
                    if (highlightInstance != null)
                        highlightInstance.SetActive(false);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectedObject = null;

            if (highlightInstance != null)
                highlightInstance.SetActive(false);
        }
    }

    public Vector3 SnapToGrid(Vector3 pos)
    {
        float x = Mathf.Round(pos.x);
        float z = Mathf.Round(pos.z);
        return new Vector3(x, 0, z);
    }

    bool IsCellOccupied(Vector3 targetPosition, Transform self)
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

        int targetX = Mathf.RoundToInt(targetPosition.x);
        int targetZ = Mathf.RoundToInt(targetPosition.z);

        foreach (GameObject unit in units)
        {
            if (unit.transform == self) continue;

            Vector3 unitPos = unit.transform.position;
            int unitX = Mathf.RoundToInt(unitPos.x);
            int unitZ = Mathf.RoundToInt(unitPos.z);

            if (unitX == targetX && unitZ == targetZ)
            {
                return true;
            }
        }

        return false;
    }
}
