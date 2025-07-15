using TMPro;
using UnityEngine;

public class BonusGoldUI : MonoBehaviour
{
    public TMP_Text goldText;

    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    void Update()
    {
        transform.position += new Vector3(1 * Time.deltaTime, 1 * Time.deltaTime, 1 * Time.deltaTime);
    }

    public void AddGold(int gold)
    {
        goldText.text = $"{gold}";
    }
}
