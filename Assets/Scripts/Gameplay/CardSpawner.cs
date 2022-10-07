using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public List<string> cardNames = new List<string> {
        "Club01", "Club02", "Club03", "Club04", "Club05", "Club06", "Club07", "Club08", "Club09", "Club10", "Club11", "Club12", "Club13",
        "Heart01", "Heart02", "Heart03", "Heart04", "Heart05", "Heart06", "Heart07", "Heart08", "Heart09", "Heart10", "Heart11", "Heart12", "Heart13",
        "Spade01", "Spade02", "Spade03", "Spade04", "Spade05", "Spade06", "Spade07", "Spade08", "Spade09", "Spade10", "Spade11", "Spade12", "Spade13",
        "Diamond01", "Diamond02", "Diamond03", "Diamond04", "Diamond05", "Diamond06", "Diamond07", "Diamond08", "Diamond09", "Diamond10", "Diamond11", "Diamond12", "Diamond13"
    };
    public GameObject prefab;
    public int numCards;
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;

    // Start is called before the first frame update
    void Start()
    {
        int index;
        string cardName;
        GameObject card;
        Vector3 position;
        LoadSprite cardProps;
        for (int i = 0; i < numCards; i++)
        {
            index = Random.Range(0, cardNames.Count);
            cardName = cardNames[index];
            cardNames.RemoveAt(index);
            position = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), -1.5f);
            card = Instantiate(prefab, position, Quaternion.identity);
            cardProps = card.GetComponent<LoadSprite>();
            cardProps.SetTexture(cardName);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
