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
        if (player == "Player 1") index = 0;
        else if (player == "Player 2") index = 3;
        else index = 6;
        index += offset;
        cards[index].sprite = Resources.Load<Sprite>(cardName);
    }
}
