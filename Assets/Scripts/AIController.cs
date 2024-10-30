using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    public GameObject GameManager;
    private List<Texture2D> aiHand = new List<Texture2D>();
    // Start is called before the first frame update
    void Start()
    {
        aiHand = GameManager.GetComponent<CardSelector>().aiHand;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
