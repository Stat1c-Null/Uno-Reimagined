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
    // Start is called before the first frame update
    void Start()
    {
        aiHand = GameManager.GetComponent<CardSelector>().aiHand;
        table = GameObject.FindWithTag("Table");
        tablePosition = table.transform.position;
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

                // Check if card has number in it
                if(int.TryParse(extractCard[1], out int cardNumber)) {

                    //If it has number, check for higher number then the one on the table
                    if(cardColor == lastPlacedColor && cardNumber > lastPlacedNumber) {
                        Debug.Log(card.name);
                        GameManager.GetComponent<CardSelector>().PlaceCard(card.name, aiHand);//Place ai card on table
                        GameManager.GetComponent<CardSelector>().isAiTurn = false;
                        GameManager.GetComponent<CardSelector>().isPlayerTurn = true;
                        //Create ai card on the screen
                        GameObject aiCard = GameManager.GetComponent<CardSelector>().InstantiateCard(card, card.name);
                        aiCard.transform.position = tablePosition + new Vector3(2, 3, -1);
                        //Destroy previous cards on the table
                        foreach (Transform child in table.transform)
                        {
                            Destroy(child.gameObject);
                        }
                        aiCard.transform.SetParent(table.transform);
                        break;
                    } else { 
                        Debug.Log("I dont have a card");
                    }

                } else {
                    Debug.Log("This is not an integer");
                }

                //int cardNumber = int.Parse(extractCard[1]);
                
            }
        }
    }
}
