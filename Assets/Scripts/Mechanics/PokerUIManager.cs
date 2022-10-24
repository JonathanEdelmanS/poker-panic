using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerUIManager : MonoBehaviour
{
    public Image[] cards = new Image[10];
    public Text player1HandText;
    public Text player2HandText;
    public Image player1Winning;
    public Image player2Winning;

    public void Reset()
    {
        player1HandText.text = "";
        player2HandText.text = "";
        player1Winning.enabled = false;
        player2Winning.enabled = false;

        for (int i = 0; i < 10; i++) {
            string cardBack = (i < 6) ? "EmptyCard" : "BackColor_Black";
            cards[i].sprite = Resources.Load<Sprite>(cardBack);
        }
    }

    public void Start() 
    {
        Reset();
    }

    // betterHand: -1 for neither player having 5 cards, 0 for true tie, 1 for player 1, 2 for player 2
    public void UpdateHandFeedback(string player1Text, string player2Text, int betterHand)
    {
        player1HandText.text = (player1Text == null) ? "" : player1Text;
        player2HandText.text = (player2Text == null) ? "" : player2Text;

        player1Winning.enabled = betterHand == 0 || betterHand == 1;
        player2Winning.enabled = betterHand == 0 || betterHand == 2;
    }

    public void ChangeCard(string player, int offset, string cardName)
    {
        int index;
        string cardBack;
        if (player == "Player 1")
        {
            index = 0;
            cardBack = "EmptyCard";
        }
        else if (player == "Player 2")
        {
            index = 3;
            cardBack = "EmptyCard";
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
