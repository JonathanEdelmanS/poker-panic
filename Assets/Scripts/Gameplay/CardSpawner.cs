using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardObject;
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;

    // Spawn a random card at a random location within bounds (and remove card from deck)
    public void SpawnCard(string cardName)
    {
        Vector3 position = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), -1.5f);
        GameObject card = Instantiate(cardObject, position, Quaternion.identity);
        Card cardProps = card.GetComponent<Card>();
        cardProps.SetTexture(cardName);
    }
}
