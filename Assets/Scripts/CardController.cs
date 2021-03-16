using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class CardController : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] float offsetRotation;
    [SerializeField] float offsetTarget;
    [SerializeField] Button randomizeBtn;
    [SerializeField] Transform usedCardsPoint;
    [SerializeField] Transform actualDeckPont;

    List<GameObject> cards = new List<GameObject>();

    int cardNum = 0;
    int numOfCards;
    Transform _prevCardTr;

    void Start()
    {
        numOfCards = Random.Range(4, 7);
        FulfilPlayersHand(numOfCards);
        SetCardsPosition(cards);
        RotateAllCards(cards);
        randomizeBtn.onClick.AddListener(ChangeRandomCardValue);
    }

    void FulfilPlayersHand(int numOfCards) 
    {
        for (int i = 0; i < numOfCards; i++)
        {
            var actualCard = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, transform);
            cards.Add(actualCard);    
        }
    }

    void SetCardsPosition(List<GameObject> cards) 
    {
        Vector3 _startPos = new Vector3(-3, -3, 0);
        Vector3 _endPos = new Vector3(3, -3, 0);

        for (int i = 0; i < cards.Count; i++)
        {
            float _t = 1f / numOfCards * i;
            cards[i].transform.position = Vector3.Lerp(_startPos, _endPos, _t);
        }
    }

    void RotateAllCards(List<GameObject> cardsToRot) 
    {
       Vector3 _targetPos = FindRotateTarget();

        for (int i = 0; i < cardsToRot.Count; i++)
        {
           RotateCard(cardsToRot[i], _targetPos);
        }
    }

    Vector3 FindRotateTarget() 
    {
       Vector3 _startPos = cards[0].transform.position;
       Vector3 _finalPos = cards[cards.Count - 1].transform.position;
       float _middlePosX = (_startPos.x + _finalPos.x) * 0.5f;
       Vector3 _targetPos = new Vector3(_middlePosX,  offsetTarget);
       return _targetPos;
    }

    void RotateCard(GameObject cardToRotate, Vector3 targetPos)
    {
        Vector3 _targetPos = targetPos;
        Vector3 _thisPos = cardToRotate.transform.position;
        _targetPos.x -= _thisPos.x;
        _targetPos.y -= _thisPos.y;

        float _angle = Mathf.Atan2(_targetPos.y, _targetPos.x) * Mathf.Rad2Deg;
        cardToRotate.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle + offsetRotation));
        float _maxAngle = Mathf.Abs(cards[0].transform.rotation.normalized.z);
        float _minAngle = 0f;
        float _insideAngle = Mathf.Abs(cardToRotate.transform.rotation.normalized.z);
        float _t = (_maxAngle - _insideAngle) / 
           (_maxAngle - _minAngle);
        float _yLocalPos = Mathf.Lerp(0, 1, _t);
        float _posYOffcet = numOfCards * 0.1f - 0.1f;
        float _newYPos = _yLocalPos * _posYOffcet;

        cardToRotate.transform.position = new Vector3(cardToRotate.transform.position.x, _newYPos);
    }

    void ChangeRandomCardValue() 
    {
        cards[cardNum++].GetComponent<Card>().ChangeRandomValue();

        if (cardNum >= cards.Count)
            cardNum = 0;

        CheckCardsHealth();
    }

    void CheckCardsHealth()
    {
        int _removedCardIdnx;
        GameObject _removedCard;
        float _moveDuration = 0.5f;

        for (int i = 0; i < cards.Count; i++)
        {
            int cardHealth = cards[i].GetComponent<Card>().HealthValue;

            if (cardHealth <= 0)
            {
                _removedCardIdnx = i;
                _removedCard = cards[i];
                cards.Remove(cards[i]);

                _removedCard.transform.DOMove(usedCardsPoint.position, _moveDuration);
                _removedCard.transform.DORotate(Vector3.zero, _moveDuration);

                RepositeOtherCards(_removedCardIdnx, _removedCard);
                CreateNewCardInstead(_prevCardTr);
            }
        }       
    }

    void RepositeOtherCards(int cardIndx, GameObject cardTr)
    {
        _prevCardTr = cardTr.transform;
        Transform _actualCardTr;
        float _moveDuration = 1f;

        for (int i = cardIndx; i < cards.Count; i++)
        {
            _actualCardTr = cards[i].transform;
            cards[i].transform.DOMove(_prevCardTr.position, _moveDuration);
            cards[i].transform.DORotateQuaternion(_prevCardTr.rotation, _moveDuration);
            _prevCardTr = _actualCardTr;
        }
    }

    void CreateNewCardInstead(Transform prevCardTr) 
    {
        Debug.LogWarning("Creating NEW CARD");
        float _moveDuration = 1.5f;
        GameObject newCard = Instantiate(cardPrefab, actualDeckPont.position, Quaternion.identity, actualDeckPont);
        newCard.transform.DOMove(prevCardTr.position, _moveDuration);
        newCard.transform.DOLocalRotateQuaternion(prevCardTr.rotation, _moveDuration);
        cards.Add(newCard);
    }
    
}
