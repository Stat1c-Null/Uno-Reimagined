using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    public GameObject GameManager;
    private List<Texture2D> aiHand = new List<Texture2D>();
    private string lastPlacedCard;
    private string lastPlacedColor;
    private int lastPlacedNumber;
    // Start is called before the first frame update
    void Start()
    {
        aiHand = GameManager.GetComponent<CardSelector>().aiHand;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.GetComponent<CardSelector>().isAiTurn) {
            lastPlacedCard = GameManager.GetComponent<CardSelector>().lastPlacedCard;
            string[] extract = lastPlacedCard.Split('_');
            lastPlacedColor = extract[0];
            lastPlacedNumber = int.Parse(extract[1]);
            foreach(Texture2D card in aiHand) {
                string[] extractCard = card.name.Split('_');
                string cardColor = extractCard[0];
                int cardNumber = int.Parse(extractCard[1]);
                if(cardColor == lastPlacedColor && cardNumber > lastPlacedNumber) {
                    Debug.Log(card.name);
                    GameManager.GetComponent<CardSelector>().isAiTurn = false;
                } else { 
                    Debug.Log("I dont have a card");
                }
            }
        }
    }
}
