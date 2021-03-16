using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

namespace RRTestTask
{
    public class CardController : MonoBehaviour
    {
        [SerializeField] GameObject cardPrefab;
        [SerializeField] Button randomizeBtn;
        [SerializeField] Transform usedCardsPoint;
        [SerializeField] Transform actualDeckPont;

        List<GameObject> cards = new List<GameObject>();
        Transform removedCardTr;
        Transform newCartTr;

        float offceеExtremeCards = 3.5f;
        float offsetRotation = 90f;
        float offsetTarget = -20f;
        int cardIndx = 0;
        int numOfCards;
        int minCardNum = 4;
        int maxCardNum = 7;
        
        void Start()
        {
            numOfCards = Random.Range(minCardNum, maxCardNum);
            FulfilPlayersHand(numOfCards);
            SetCardsPosition(cards);
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

        #region SetCardsPosition

        void SetCardsPosition(List<GameObject> cards)
        {
            Vector3 _startPos = new Vector3(-offceеExtremeCards, 0, 0);
            Vector3 _endPos = new Vector3(offceеExtremeCards, 0, 0);

            for (int i = 0; i < cards.Count; i++)
            {
                float _t = 1f / numOfCards * i;
                cards[i].transform.position = Vector3.Lerp(_startPos, _endPos, _t);
            }

            RotateAllCards(cards);
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
            Vector3 _targetPos = new Vector3(_middlePosX, offsetTarget);
            return _targetPos;
        }

        void RotateCard(GameObject cardToRotate, Vector3 targetPos)
        {
            Vector3 _targetPos = targetPos;
            Vector3 _thisPos = cardToRotate.transform.position;
            float _posYOffcet = 0.1f;
            float _minAngle = 0.02f;

            _targetPos.x -= _thisPos.x;
            _targetPos.y -= _thisPos.y;

            float _angle = Mathf.Atan2(_targetPos.y, _targetPos.x) * Mathf.Rad2Deg;
            cardToRotate.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle + offsetRotation));
            float _maxAngle = Mathf.Abs(cards[0].transform.rotation.normalized.z);
            float _insideAngle = Mathf.Abs(cardToRotate.transform.rotation.normalized.z);
            float _t = (_maxAngle - _insideAngle) / (_maxAngle - _minAngle);
            float _yLocalPos = Mathf.Lerp(0, 1, _t);
            _posYOffcet *= numOfCards;
            float _newYPos = _yLocalPos * _posYOffcet;

            cardToRotate.transform.position = new Vector3(cardToRotate.transform.position.x, _newYPos);
        }

        #endregion

        #region ChangeRandomCardValue
        void ChangeRandomCardValue()
        {
            if (cardIndx >= cards.Count)
                cardIndx = 0;

            Card _card = cards[cardIndx].GetComponent<Card>();
            _card.ChangeRandomValue();

            if (_card.HealthValue <= 0)
            {
                RemoveCardFromHand(cardIndx);
                RepositeOtherCards(removedCardTr);
                CreateNewCardInstead();
            }

            cardIndx++;
        }

        private void RemoveCardFromHand(int cardIndx)
        {
            float _moveDuration = 2f;
            
            removedCardTr = cards[cardIndx].transform;
            cards[cardIndx].transform.DOMove(usedCardsPoint.position, _moveDuration);
            cards[cardIndx].transform.DORotate(Vector3.zero, _moveDuration);
            cards.Remove(cards[cardIndx]);
        }

        private void RepositeOtherCards(Transform removedCard)
        {
            Transform _prevCardTransf = removedCard;
            Transform _actualCardTr;
            float _moveDuration = 1f;

            for (int i = cardIndx; i < cards.Count; i++)
            {
                _actualCardTr = cards[i].transform;
                cards[i].transform.DOMove(_prevCardTransf.position, _moveDuration);
                cards[i].transform.DORotateQuaternion(_prevCardTransf.rotation, _moveDuration);
                _prevCardTransf = _actualCardTr;
            }

            newCartTr = _prevCardTransf;
        }

        private void CreateNewCardInstead()
        {
            float _moveDuration = 2f;

            var actualCard = Instantiate(cardPrefab, actualDeckPont.position, Quaternion.identity, transform);
            actualCard.transform.DOMove(newCartTr.position, _moveDuration);
            actualCard.transform.DORotateQuaternion(newCartTr.rotation, _moveDuration);
            cards.Add(actualCard);
        }

        #endregion
    }
}
