using UnityEngine;

public class BuffSide : MonoBehaviour
{
    public GameObject buffEffectPrefab;
    public float cellSize = 1.5f;
    public float effectHeightOffset = 0.7f;

    public int gridWidth = 3;
    public int gridHeight = 3;
    public Vector3 gridOrigin = Vector3.zero;

    private GameObject leftEffect;
    private GameObject rightEffect;
    private GameObject forwardEffect;
    private GameObject backwardEffect;

    void Update()
    {
        Vector3 worldPos = transform.position;

        int x = Mathf.RoundToInt((worldPos.x - gridOrigin.x) / cellSize);
        int z = Mathf.RoundToInt((worldPos.z - gridOrigin.z) / cellSize);

        Quaternion flatRotation = Quaternion.Euler(-90, 0, 0);

        // ＄ 哭率
        if (x - 1 >= 0)
        {
            Vector3 pos = new Vector3((x - 1) * cellSize + gridOrigin.x, worldPos.y + effectHeightOffset, z * cellSize + gridOrigin.z);
            if (leftEffect == null)
                leftEffect = Instantiate(buffEffectPrefab, pos, flatRotation);
            else
                leftEffect.transform.position = pos;
        }
        else if (leftEffect != null)
        {
            Destroy(leftEffect);
            leftEffect = null;
        }

        // ℃ 坷弗率
        if (x + 1 < gridWidth)
        {
            Vector3 pos = new Vector3((x + 1) * cellSize + gridOrigin.x, worldPos.y + effectHeightOffset, z * cellSize + gridOrigin.z);
            if (rightEffect == null)
                rightEffect = Instantiate(buffEffectPrefab, pos, flatRotation);
            else
                rightEffect.transform.position = pos;
        }
        else if (rightEffect != null)
        {
            Destroy(rightEffect);
            rightEffect = null;
        }

        // ¤ 菊率 (z+1)
        if (z + 1 < gridHeight)
        {
            Vector3 pos = new Vector3(x * cellSize + gridOrigin.x, worldPos.y + effectHeightOffset, (z + 1) * cellSize + gridOrigin.z);
            if (forwardEffect == null)
                forwardEffect = Instantiate(buffEffectPrefab, pos, flatRotation);
            else
                forwardEffect.transform.position = pos;
        }
        else if (forwardEffect != null)
        {
            Destroy(forwardEffect);
            forwardEffect = null;
        }

        // ￠ 第率 (z-1)
        if (z - 1 >= 0)
        {
            Vector3 pos = new Vector3(x * cellSize + gridOrigin.x, worldPos.y + effectHeightOffset, (z - 1) * cellSize + gridOrigin.z);
            if (backwardEffect == null)
                backwardEffect = Instantiate(buffEffectPrefab, pos, flatRotation);
            else
                backwardEffect.transform.position = pos;
        }
        else if (backwardEffect != null)
        {
            Destroy(backwardEffect);
            backwardEffect = null;
        }
    }

    void OnDestroy()
    {
        if (leftEffect != null) Destroy(leftEffect);
        if (rightEffect != null) Destroy(rightEffect);
        if (forwardEffect != null) Destroy(forwardEffect);
        if (backwardEffect != null) Destroy(backwardEffect);
    }
}
