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
    public float[] times = new float[4];
    string[] player1Cards = new string[3] { "", "", "" };
    string[] player2Cards = new string[3] { "", "", "" };
    string[] streetCards = new string[4] { "", "", "", "" };
    public int numStartCards = 2;
    Card touchingP1 = null;  // the card Player would receive if they called AcceptCard()
    Card touchingP2 = null;
    int totalSpawned;
    public int maxTotalSpawned = 3;

    // Start is called before the first frame update
    void Start()
    {
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
        Debug.Log(totalSpawned);
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
    }

    public int CompareDecks()
    {
        List<string> p1cards = new List<string>();
        List<string> p2cards = new List<string>();

        foreach (string card in player1Cards) {
            if (card != "")
                p1cards.Add(card.Substring(card.Length-2,2) + card.Substring(0, card.Length-2));
        }
        foreach (string card in player2Cards) {
            if (card != "")
                p2cards.Add(card.Substring(card.Length-2,2) + card.Substring(0, card.Length-2));
        }
        foreach (string card in streetCards) {
            if (card != "") {
                p1cards.Add(card.Substring(card.Length-2,2) + card.Substring(0, card.Length-2));
                p2cards.Add(card.Substring(card.Length-2,2) + card.Substring(0, card.Length-2));
            }
        }
        p1cards.Sort();
        p2cards.Sort();

        List<string> suits1 = new List<string>();
        List<int> values1 = new List<int>();
        List<string> suits2 = new List<string>();
        List<int> values2 = new List<int>();

        foreach (string card in p1cards) {
            suits1.Add(card.Substring(2, card.Length-2));
            values1.Add(Int32.Parse(card.Substring(0,2)));
        }
        foreach (string card in p2cards) {
            suits2.Add(card.Substring(2, card.Length-2));
            values2.Add(Int32.Parse(card.Substring(0,2)));
        }

        if (p1cards.Count == 4) {
            suits1.Insert(0, "Null");
            values1.Insert(0, 0);
        }
        else if (p1cards.Count == 6) {
            List<string> bestSuits = new List<string>();
            List<int> bestVals = new List<int>();
            bestSuits.AddRange(suits1);
            bestSuits.RemoveAt(0);
            bestVals.AddRange(values1);
            bestVals.RemoveAt(0);
            for (int i=1; i<6; i++) {
                List<string> newSuits = new List<string>();
                List<int> newVals = new List<int>();
                newSuits.AddRange(suits1);
                newSuits.RemoveAt(i);
                newVals.AddRange(values1);
                newVals.RemoveAt(i);
                int x = CompareHands(bestSuits, bestVals, newSuits, newVals);
                if (x == 2) {
                    bestSuits = new List<string>();
                    bestSuits.AddRange(newSuits);
                    bestVals = new List<int>();
                    bestVals.AddRange(newVals);
                }
            }
            suits1 = bestSuits;
            values1 = bestVals;
        }
        else if (p1cards.Count == 7) {
            List<string> bestSuits = new List<string>();
            List<int> bestVals = new List<int>();
            bestSuits.AddRange(suits1);
            bestSuits.RemoveAt(0);
            bestSuits.RemoveAt(1);
            bestVals.AddRange(values1);
            bestVals.RemoveAt(0);
            bestVals.RemoveAt(1);
            for (int i=0; i<6; i++) {
                for (int j=i+1; j<7; j++) {
                    List<string> newSuits = new List<string>();
                    List<int> newVals = new List<int>();
                    newSuits.AddRange(suits1);
                    newSuits.RemoveAt(i);
                    newSuits.RemoveAt(j-1);
                    newVals.AddRange(values1);
                    newVals.RemoveAt(i);
                    newVals.RemoveAt(j-1);
                    int x = CompareHands(bestSuits, bestVals, newSuits, newVals);
                    if (x == 2) {
                        bestSuits = new List<string>();
                        bestSuits.AddRange(newSuits);
                        bestVals = new List<int>();
                        bestVals.AddRange(newVals);
                    }
                }
            }
            suits1 = bestSuits;
            values1 = bestVals;
        }

        if (p2cards.Count == 4) {
            suits2.Insert(0, "Null");
            values2.Insert(0, 0);
        }
        else if (p2cards.Count == 6) {
            List<string> bestSuits = new List<string>();
            List<int> bestVals = new List<int>();
            bestSuits.AddRange(suits2);
            bestSuits.RemoveAt(0);
            bestVals.AddRange(values2);
            bestVals.RemoveAt(0);
            for (int i=1; i<6; i++) {
                List<string> newSuits = new List<string>();
                List<int> newVals = new List<int>();
                newSuits.AddRange(suits2);
                newSuits.RemoveAt(i);
                newVals.AddRange(values2);
                newVals.RemoveAt(i);
                int x = CompareHands(bestSuits, bestVals, newSuits, newVals);
                if (x == 2) {
                    bestSuits = new List<string>();
                    bestSuits.AddRange(newSuits);
                    bestVals = new List<int>();
                    bestVals.AddRange(newVals);
                }
            }
            suits2 = bestSuits;
            values2 = bestVals;
        }
        else if (p2cards.Count == 7) {
            List<string> bestSuits = new List<string>();
            List<int> bestVals = new List<int>();
            bestSuits.AddRange(suits2);
            bestSuits.RemoveAt(0);
            bestSuits.RemoveAt(1);
            bestVals.AddRange(values2);
            bestVals.RemoveAt(0);
            bestVals.RemoveAt(1);
            for (int i=0; i<6; i++) {
                for (int j=i+1; j<7; j++) {
                    List<string> newSuits = new List<string>();
                    List<int> newVals = new List<int>();
                    newSuits.AddRange(suits2);
                    newSuits.RemoveAt(i);
                    newSuits.RemoveAt(j-1);
                    newVals.AddRange(values2);
                    newVals.RemoveAt(i);
                    newVals.RemoveAt(j-1);
                    int x = CompareHands(bestSuits, bestVals, newSuits, newVals);
                    if (x == 2) {
                        bestSuits = new List<string>();
                        bestSuits.AddRange(newSuits);
                        bestVals = new List<int>();
                        bestVals.AddRange(newVals);
                    }
                }
            }
            suits2 = bestSuits;
            values2 = bestVals;
        }

        return CompareHands(suits1, values1, suits2, values2);
    }

    public int CompareHands(List<string> suits1, List<int> values1, List<string> suits2, List<int> values2) {
        // 0: Tie   1: Player1 wins     2: Player2 wins
        int player1rank = rankHand(suits1, values1);
        int player2rank = rankHand(suits2, values2);
        if (player1rank == player2rank) {
            int i = 4;
            while (values1[i] == values2[i] && i > 0)
                i--;
            if (values1[i] == values2[i])
                return 0;
            if (values1[i] > values2[i])
                return 1;
            return 2;
        } 
        if (player1rank < player2rank)
            return 1;
        return 2;
    }

    public int rankHand(List<string> suits, List<int> values)
    {
        // 1. Royal flush   2. Straight flush   3. Four of kind
        // 4. Full house    5. Flush    6. Straight     7. Three of kind
        // 8. Two pair      9. Pair     10. High Card
        bool straight = true;
        for (int i=1; i < 5; i++) {
            if (values[i] != values[i-1] + 1)
                straight = false;
        }
        bool flush = true;
        for (int i=1; i < 5; i++) {
            if (suits[i] != suits[i-1])
                flush = false;
        }

        if (straight && flush) {
            if (values[4] == 14) {
                return 1;
            }
            return 2;
        }
        if (values[0] == values[3] || values[1] == values[4])
            return 3;
        if ((values[0] == values[2] && values[3] == values[4]) || (values[0] == values[1] && values[2] == values[4]))
            return 4;
        if (flush)
            return 5;
        if (straight)
            return 6;
        if (values[0] == values[2] || values[1] == values[3] || values[2] == values[4])
            return 7;
        int numPairs = 0;
        for (int i=1; i<5; i++) {
            if (values[i] == values[i-1])
                numPairs++;
        }
        if (numPairs == 2)
            return 8;
        if (numPairs == 1)
            return 9;
        return 10;
    }
}
