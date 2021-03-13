using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CardController : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;

    Card card;
    GameObject actualCard;
    GameObject[] cards;

    float radius = 3;
    float angle;

    float increment = 12;

    void Start()
    {
        int numOfCards = Random.Range(4, 7);
        angle = 90 / numOfCards;
        FulfilPlayersHand(numOfCards);
    }

    void FulfilPlayersHand(int numOfCards) 
    {
        cards = new GameObject[numOfCards];
        Vector3 prevPos = new Vector3(-3, 0);
        Vector3 screenPoint = new Vector3(transform.position.x, transform.position.y - 10);

        for (int i = 0; i < numOfCards; i++)
        {
            var actualCard = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, transform);
            cards[i] = actualCard;

            //actualCard.transform.RotateAround(screenPoint, Vector3.forward, angle);
            //angle -= increment;            
        }

        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].transform.position = new Vector3(prevPos.x + 2, cards[i].transform.position.y);
            cards[i].transform.LookAt(screenPoint, Vector3.zero);
            prevPos = cards[i].transform.position;
        }
    }

    void Update()
    {
        
    }
}
