using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

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
    public Dictionary<string, string> handNames = new Dictionary<string, string>();
    public Dictionary<string, int> handScores = new Dictionary<string, int>();
    int winningPlayer = -1;
    public float[] times = new float[4];
    public string[] player1Cards = new string[3] { "", "", "" };
    public string[] player2Cards = new string[3] { "", "", "" };
    public string[] streetCards = new string[4] { "", "", "", "" };
    public int numStartCards = 2;
    Card touchingP1 = null;  // the card Player would receive if they called AcceptCard()
    Card touchingP2 = null;
    int totalSpawned;
    public int maxTotalSpawned = 3;

    // Start is called before the first frame update
    void Start()
    {
        // load the hands hashing
        IEnumerable<String> hands_data_file = System.IO.File.ReadLines("Assets/Resources/hands.csv");
        foreach (string line in hands_data_file) {
            string[] tokens = line.Split('\t');
            string hand = tokens[0]; 
            int score = Int32.Parse(tokens[1]);
            string name = tokens[2]; 
            handNames.Add(hand, name);
            handScores.Add(hand, score);
        }

        // spawn in 3 cards at the beginning of the game
        for (int i = 0; i < numStartCards; i++)
        {
            SpawnCard();
        }
        totalSpawned = numStartCards;
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
        if (totalSpawned >= maxTotalSpawned) return;
        cardSpawner.SpawnCard(DrawCard());
        totalSpawned++;
    }

    public void UnloadCard()
    {
        totalSpawned--;
    }

    public void AddCard()
    {
        totalSpawned++;
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
        HandsUpdated();
    }

    public bool TryGivePlayer(string playerName, string card)
    {
        // try to add card to the first empty slot in player's inventory
        // returns true if there was space in inventory else false
        string[] cards = (playerName == "Player 1") ? player1Cards : player2Cards;
        for (int i = 0; i < 3; i++)
        {
            if (cards[i] == "")
            {
                cards[i] = card;
                pokerUIManager.ChangeCard(playerName, i, card);
                SpawnCard();
                HandsUpdated();
                return true;
            }
        }
        return false;
    }

    public void SetTouching(string playerName, Card card)
    {
        if (playerName == "Player 1" && !touchingP1) touchingP1 = card;
        if (playerName == "Player 2" && !touchingP2) touchingP2 = card;
    }

    public void StopTouching(string playerName, Card card)
    {
        if (playerName == "Player 1" && touchingP1 == card) touchingP1 = null;
        if (playerName == "Player 2" && touchingP2 == card) touchingP2 = null;
    }

    public void AcceptCard(string playerName, int index)
    {
        if (playerName == "Player 1" && touchingP1)
        {
            RemoveCard(playerName, index);
            string card = touchingP1.card;
            touchingP1.Collect();
            TryGivePlayer(playerName, card);
            touchingP1 = null;
        }
        if (playerName == "Player 2" && touchingP2)
        {
            RemoveCard(playerName, index);
            string card = touchingP2.card;
            touchingP2.Collect();
            TryGivePlayer(playerName, card);
            touchingP2 = null;
        }
    }

    public string RemoveCard(string playerName, int index)
    {
        string[] cards = (playerName == "Player 1") ? player1Cards : player2Cards;
        string oldCard = cards[index];
        cards[index] = "";
        pokerUIManager.ChangeCard(playerName, index, "");
        return oldCard;
    }

    public void Punch(GameObject player)
    {
        string[] cards = player.name == "Player 1" ? player1Cards : player2Cards;
        List<int> toSelect = new List<int>();
        for (int i = 0; i < 3; i++) if (cards[i] != "") toSelect.Add(i);
        if (toSelect.Count == 0) return;
        int index = toSelect[Random.Range(0, toSelect.Count)];
        string oldCard = RemoveCard(player.name, index);
        cardSpawner.SpawnCard(oldCard);
        totalSpawned++;
        HandsUpdated();
    }

    // get the winning player and their hand
    public (int WinningPlayer, string WinningHandName) CompareDecks()
    {
        (string HandName, int HandScore) player1Hand = GetHandInfo(1);
        (string HandName, int HandScore) player2Hand = GetHandInfo(2);
        string player1HandName = player1Hand.HandName;
        string player2HandName = player2Hand.HandName;
        int player1Score = player1Hand.HandScore;
        int player2Score = player2Hand.HandScore;

        var (wP, handName) = WinningPlayer(player1Score, player1HandName, player2Score, player2HandName); 
        return (wP, handName);
    }

    public List<string> GetCards(string[] hand) 
    {
        List<string> allCards = new List<string>();
        foreach (string card in streetCards) 
            if (card != "") allCards.Add(card); 
        foreach (string card in hand) 
            if (card != "") allCards.Add(card); 
        allCards.Sort();
        return allCards;
    }

    (string HandName, int HandScore) GetHandInfo(int player)
    {
        string[] cards = (player == 1) ? player1Cards : player2Cards;
        List<string> allCards = GetCards(cards);
        string hand = String.Join(",", allCards);
        string handName;
        int handScore;
        handNames.TryGetValue(hand, out handName);
        handScores.TryGetValue(hand, out handScore);
        return (handName, handScore);
    }

    // winner logic
    (int Player, string HandName) WinningPlayer(int player1Score, string player1HandName, int player2Score, string player2HandName)
    {
        if (player1Score == 0 && player2Score == 0) {
            // neither player has a hand
            return (-1, null);
        }
        else if (player2Score == 0 || player1Score < player2Score) {
            return (1, player1HandName);
        }
        else if (player1Score == 0 || player1Score > player2Score) {
            return (2, player2HandName);
        }
        else {
            // true tie
            return (0, null);
        }
    }

    public void HandsUpdated() 
    {
        (string HandName, int HandScore) player1Hand = GetHandInfo(1);
        (string HandName, int HandScore) player2Hand = GetHandInfo(2);
        string player1HandName = player1Hand.HandName;
        string player2HandName = player2Hand.HandName;
        int player1Score = player1Hand.HandScore;
        int player2Score = player2Hand.HandScore;

        var (wP, _) = WinningPlayer(player1Score, player1HandName, player2Score, player2HandName); 
        winningPlayer = wP;

        pokerUIManager.UpdateHandFeedback(player1HandName, player2HandName, winningPlayer);
    }
}
