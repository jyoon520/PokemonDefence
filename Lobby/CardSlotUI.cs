using UnityEngine;
using UnityEngine.UI;

public class CardSlotUI : MonoBehaviour
{
    public Image cardImage;
    public Image borderImage;
    public Text cardNameText;
    public Text cardGradeText;
    public GameObject ownedMark; // 카드 보유 여부 표시용 

    public void Setup(RandomCard card, bool isOwned)
    {
        cardImage.sprite = card.cardImage;
        cardNameText.text = card.cardName;
        cardGradeText.text = card.cardGrade.ToString();
        ownedMark.SetActive(isOwned);

        borderImage.gameObject.SetActive(true);
    }
}
