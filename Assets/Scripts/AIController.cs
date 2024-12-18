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
    private Vector3 tablePosition;
    private GameObject table;
    private CardSelector cardSelector;
    // Start is called before the first frame update
    void Start()
    {
        cardSelector = GameManager.GetComponent<CardSelector>();
        aiHand = cardSelector.aiHand;
        table = GameObject.FindWithTag("Table");
        tablePosition = table.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(cardSelector.isAiTurn) {
            lastPlacedCard = cardSelector.lastPlacedCard;
            string[] extract = lastPlacedCard.Split('_');
            lastPlacedColor = extract[0];
            lastPlacedNumber = int.Parse(extract[1]);

            foreach(Texture2D card in aiHand) {
                string[] extractCard = card.name.Split('_');
                string cardColor = extractCard[0];

                // Check if card has number in it
                if(int.TryParse(extractCard[1], out int cardNumber)) {

                    //If it has number, check for higher number then the one on the table
                    if(cardColor == lastPlacedColor && cardNumber > lastPlacedNumber) {
                        placeCardOnTable(card);
                        break;
                    }
                    else if(cardNumber == lastPlacedNumber && cardColor != lastPlacedColor) {
                        placeCardOnTable(card);
                        break;
                    } /*else if(lastPlacedNumber == "picker" && lastPlacedColor == cardColor) {
                        placeCardOnTable(card);
                        break;
                    }*/
                    else { 
                        Debug.Log("I dont have a card");
                    } 

                } 
                else {
                    Debug.Log("This is not an integer");
                }

                //int cardNumber = int.Parse(extractCard[1]);
                
            }
        }
    }

    public void placeCardOnTable(Texture2D card) {
        cardSelector.PlaceCard(card.name, aiHand);//Place ai card on table
        cardSelector.isAiTurn = false;
        cardSelector.isPlayerTurn = true;
        //Create ai card on the screen
        GameObject aiCard = cardSelector.InstantiateCard(card, card.name);
        aiCard.transform.position = tablePosition + new Vector3(2, 3, -1);
        //Destroy previous cards on the table
        foreach (Transform child in table.transform)
        {
            Destroy(child.gameObject);
        }
        aiCard.transform.SetParent(table.transform);
    }
}
