using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIController : MonoBehaviour
{

    public GameObject GameManager;
    private List<Texture2D> aiHand = new List<Texture2D>();
    private string lastPlacedCard;
    private string lastPlacedSuperCard;
    private string lastPlacedColor;
    private int lastPlacedNumber;
    private Vector3 tablePosition;
    private GameObject table;
    private CardSelector cardSelector;
    private int cardCount = 0;
    private int handLength;
    private int cardsGrabbed;
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
        if(cardSelector.isPlayerTurn) {
            lastPlacedSuperCard = "";
            cardsGrabbed = 0;
        }
        if(cardsGrabbed == 3) {
            cardSelector.isPlayerTurn = true;
            cardSelector.isAiTurn = false;
            
        }
        if(cardSelector.isAiTurn) {
            //Save how much cards ai has at the moment
            handLength = aiHand.Count;
            cardCount = 0;
            lastPlacedCard = cardSelector.lastPlacedCard;
            string[] extract = lastPlacedCard.Split('_');
            lastPlacedColor = extract[0];

            //If the last card ends up being wild, grab updated color from card selector
            if(lastPlacedColor == "wild"){
                lastPlacedColor = cardSelector.currentColor;
            }

            try {
                lastPlacedNumber = int.Parse(extract[1]);
            } catch (Exception ex) { //In case second word in card name is a string
                lastPlacedSuperCard = extract[1];
            }

            //TODO: Rework this logic for multiple players
            if(lastPlacedSuperCard == "skip" || lastPlacedSuperCard == "reverse") {
                cardSelector.isPlayerTurn = true;
                cardSelector.isAiTurn = false;
            } else {
                foreach(Texture2D card in aiHand) {
                    string[] extractCard = card.name.Split('_');
                    string cardColor = extractCard[0];
                    cardCount++;
                    Debug.Log(cardCount);
                    //Only pick card after looping through entire hand and making sure there are no cards that can be played
                    if(cardCount >= handLength && cardsGrabbed <= 3) {
                        cardsGrabbed += 1;
                        cardSelector.DrawCardFromDeck(cardSelector.aiHand, false);
                        break;
                    }
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
                        } 
                        else if(lastPlacedSuperCard == "picker" && lastPlacedColor == cardColor) {
                            placeCardOnTable(card);
                            break;
                        }
                        else if(lastPlacedSuperCard == "wild" && lastPlacedColor == cardColor) {
                            placeCardOnTable(card);
                            break;
                        }
                        else { 
                            Debug.Log("Not the right card");
                            //cardSelector.DrawCardFromDeck(cardSelector.aiHand, false);
                        }

                    } 
                    else {
                        Debug.Log("This is not an integer");
                    }
                    //int cardNumber = int.Parse(extractCard[1]);
                }
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
