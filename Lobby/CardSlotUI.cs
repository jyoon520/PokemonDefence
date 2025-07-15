using UnityEngine;
using UnityEngine.UI;

public class CardSlotUI : MonoBehaviour
{
    public Image cardImage;
    public Image borderImage;
    public Text cardNameText;
    public Text cardGradeText;
    public GameObject ownedMark; // ī�� ���� ���� ǥ�ÿ� 

    public void Setup(RandomCard card, bool isOwned)
    {
        cardImage.sprite = card.cardImage;
        cardNameText.text = card.cardName;
        cardGradeText.text = card.cardGrade.ToString();
        ownedMark.SetActive(isOwned);

        borderImage.gameObject.SetActive(true);
    }
}
