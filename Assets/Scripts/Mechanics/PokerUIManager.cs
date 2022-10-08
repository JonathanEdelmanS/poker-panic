using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerUIManager : MonoBehaviour
{
    //public Image player1Card1;
    //public Image player1Card2;
    //public Image player1Card3;
    //public Image player2Card1;
    //public Image player2Card2;
    //public Image player2Card3;
    //public Image streetCard1;
    //public Image streetCard2;
    //public Image streetCard3;
    //public Image streetCard4;
    public Image[] cards = new Image[10];

    public void ChangeCard(string player, int offset, string cardName)
    {
        int index;
        string cardBack;
        if (player == "Player 1")
        {
            index = 0;
            cardBack = "BackColor_Blue";
        }
        else if (player == "Player 2")
        {
            index = 3;
            cardBack = "BackColor_Red";
        }
        else
        {
            index = 6;
            cardBack = "BackColor_Black";
        }
        index += offset;
        cards[index].sprite = Resources.Load<Sprite>(cardName != "" ? cardName : cardBack);
    }
}
