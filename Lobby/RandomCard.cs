using UnityEngine;

public enum CardGrade { SSS = 1, AA = 2, B = 3}

[System.Serializable]
public class RandomCard
{
    public int id;                    
    public string cardName;
    public Sprite cardImage;
    public CardGrade cardGrade;
    public int weight;
    public int level;


    // 복사 생성자
    public RandomCard(RandomCard card)
    {
        this.id = card.id;
        this.cardName = card.cardName;
        this.cardImage = card.cardImage;
        this.cardGrade = card.cardGrade;
        this.weight = card.weight;
        this.level = card.level;
    }
}
