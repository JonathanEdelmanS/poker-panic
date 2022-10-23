using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerUIManager : MonoBehaviour
{
    public Image[] cards = new Image[10];
    public Text player1HandText;
    public Text player2HandText;
    public int winningPlayer = 0;

    public void UpdateHandFeedback(string player1Text, string player2Text, int betterHand)
    {
        player1HandText.text = (player1Text == null) ? "" : player1Text;
        player2HandText.text = (player2Text == null) ? "" : player2Text;
    }

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
