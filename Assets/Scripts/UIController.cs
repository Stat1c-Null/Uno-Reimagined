using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject GameManager;
    public int maxCardsPerTurn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawCard(){
        if(maxCardsPerTurn > 0) {
            GameManager.GetComponent<CardSelector>().DrawCardFromDeck();
            maxCardsPerTurn -= 1;
        }
    }
}
