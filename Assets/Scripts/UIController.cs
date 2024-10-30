using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject GameManager;
    public int maxCardsPerTurn;
    public bool skipMove;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void DrawCard(){
        if(GameManager.GetComponent<CardSelector>().isPlayerTurn && maxCardsPerTurn > 0) {
            GameManager.GetComponent<CardSelector>().DrawCardFromDeck();
            maxCardsPerTurn -= 1;
        }
    }

    public void EndTurn() {
        skipMove = GameManager.GetComponent<CardSelector>().isPlayerTurn;
        if (skipMove == true && maxCardsPerTurn == 0) {
            GameManager.GetComponent<CardSelector>().BurnCards();
            maxCardsPerTurn = 3;
            GameManager.GetComponent<CardSelector>().isPlayerTurn = false;
        }else if (skipMove == false) { 
            GameManager.GetComponent<CardSelector>().isAiTurn = true;
            GameManager.GetComponent<CardSelector>().isPlayerTurn = false;
        }
    }
}
