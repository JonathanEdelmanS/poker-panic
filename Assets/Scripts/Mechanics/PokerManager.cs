using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerManager : MonoBehaviour
{
    List<string> deck = new List<string> {
        "Club01", "Club06", "Club07", "Club08", "Club09", "Club10", "Club11", "Club12", "Club13",
        "Heart01", "Heart06", "Heart07", "Heart08", "Heart09", "Heart10", "Heart11", "Heart12", "Heart13",
        "Spade01", "Spade06", "Spade07", "Spade08", "Spade09", "Spade10", "Spade11", "Spade12", "Spade13",
        "Diamond01", "Diamond06", "Diamond07", "Diamond08", "Diamond09", "Diamond10", "Diamond11", "Diamond12", "Diamond13"
    };
    public CardSpawner cardSpawner;
    public PokerUIManager pokerUIManager;
    public float[] times = new float[4];
    string[] player1Cards = new string[3] { "", "", "" };
    string[] player2Cards = new string[3] { "", "", "" };
    string[] streetCards = new string[4] { "", "", "", "" };

    // Start is called before the first frame update
    void Start()
    {
        // spawn in 3 cards at the beginning of the game
        for (int i = 0; i < 3; i++)
        {
            SpawnCard();
        }
    }

    void Update()
    {
        float thisFrame = Time.time;
        float lastFrame = Time.time - Time.deltaTime;
        for (int i = 0; i < 4; i++)
        {
            float time = times[i];
            if (thisFrame > time && lastFrame < time)
            {
                FlipStreetCard(i);
            }
        }
    }

    public void SpawnCard()
    {
        cardSpawner.SpawnCard(DrawCard());
    }

    string DrawCard()
    {
        int index = Random.Range(0, deck.Count - 1);
        string card = deck[index];
        deck.RemoveAt(index);
        return card;
    }

    void FlipStreetCard(int index)
    {
        string card = DrawCard();
        streetCards[index] = card;
        pokerUIManager.ChangeCard("Street", index, card);
    }

    public bool GivePlayer(string playerName, string card)
    {
        // add card to the first empty slot in player's inventory
        // returns true if there was space in inventory else false
        string[] cards = (playerName == "Player 1") ? player1Cards : player2Cards;
        for (int i = 0; i < 3; i++)
        {
            if (cards[i] == "")
            {
                cards[i] = card;
                pokerUIManager.ChangeCard(playerName, i, card);
                cardSpawner.SpawnCard(DrawCard());
                return true;
            }
        }
        return false;
    }

    public bool RemoveCard(string playerName, int index)
    {
        string[] cards = (playerName == "Player 1") ? player1Cards : player2Cards;
        bool output = cards[index] != "";
        cards[index] = "";
        pokerUIManager.ChangeCard(playerName, index, "");
        return output;
    }

    public void Punch(GameObject player)
    {
        string[] cards = player.name == "Player 1" ? player1Cards : player2Cards;
        List<int> toSelect = new List<int>();
        for (int i = 0; i < 3; i++) if (cards[i] != "") toSelect.Add(i);
        if (toSelect.Count == 0) return;
        int index = toSelect[Random.Range(0, toSelect.Count)];
        RemoveCard(player.name, index);
    }
}
