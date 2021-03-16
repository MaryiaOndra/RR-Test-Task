using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

namespace RRTestTask
{
    enum CardStats
    {
        Mana,
        Attack,
        Health,
        Lenght
    }
    public class Card : MonoBehaviour
    {
        const string IMAGE_PATH = "https://picsum.photos/200/300";

        [SerializeField] Image icon;
        [SerializeField] TMP_Text title;
        [SerializeField] TMP_Text discription;

        [Header("Stats")]
        [SerializeField] TMP_Text manaTxt;
        [SerializeField] TMP_Text attackTxt;
        [SerializeField] TMP_Text healthTxt;

        [Header("Tween Stats")]
        [SerializeField] TMP_Text tweenManaTxt;
        [SerializeField] TMP_Text tweenAttackTxt;
        [SerializeField] TMP_Text tweenHealthTxt;

        bool isFirsRandom = true;
        int statsMaxInt = 10;
        int statsMinInt = 1;

        public int ManaValue { get; private set; }
        public int AttackValue { get; private set; }
        public int HealthValue { get; private set; }
        public bool IsDead { get; private set; }


        private void Start()
        {
            SetRandomStats();
            isFirsRandom = false;
            StartCoroutine(SetImageFromWebToCard());
        }

        IEnumerator SetImageFromWebToCard()
        {
            Vector2 _pivotPoint = new Vector2(0.5f, 0.5f);
            float _pixelsPerUnit = 100f;

            using (var uwr = new UnityWebRequest(IMAGE_PATH, UnityWebRequest.kHttpVerbGET))
            {
                uwr.downloadHandler = new DownloadHandlerTexture();
                yield return uwr.SendWebRequest();
                Texture2D _texture = DownloadHandlerTexture.GetContent(uwr);
                Rect _rect = new Rect(0.0f, 0.0f, _texture.width, _texture.height);
                Sprite _sprite = Sprite.Create(_texture, _rect, _pivotPoint, _pixelsPerUnit);
                icon.sprite = _sprite;
            }
        }

        void SetRandomStats()
        {
            int _rndValue = Random.Range(statsMinInt, statsMaxInt);
            SetStat(_rndValue, CardStats.Health, healthTxt, tweenHealthTxt);

            _rndValue = Random.Range(statsMinInt, statsMaxInt);
            SetStat(_rndValue, CardStats.Mana, manaTxt, tweenManaTxt);

            _rndValue = Random.Range(statsMinInt, statsMaxInt);
            SetStat(_rndValue, CardStats.Attack, attackTxt, tweenAttackTxt);
        }

        void SetStat(int rndValue, CardStats cardStats, TMP_Text textToShow, TMP_Text textToTween)
        {
            int _property = 0;

            switch (cardStats)
            {
                case CardStats.Mana:
                    _property = ManaValue;
                    ManaValue = rndValue;
                    break;
                case CardStats.Attack:
                    _property = AttackValue;
                    AttackValue = rndValue;
                    break;
                case CardStats.Health:
                    _property = HealthValue;
                    HealthValue = rndValue;
                    if (HealthValue < 0)
                        IsDead = true;
                    break;
            }

            int _diff = rndValue - _property;

            if (!isFirsRandom && rndValue != _property)
            {
                CreateTextTween(_diff, textToTween);
            }

            _property = rndValue;
            textToShow.text = _property.ToString();
        }

        private void CreateTextTween(int diff, TMP_Text tweenTxt)
        {
            float _duration = 2.5f;
            float _alfa = 0f;

            if (diff < 0)
            {
                tweenTxt.text = diff.ToString();
                tweenTxt.color = Color.red;
            }
            else
            {
                tweenTxt.text = "+" + diff;
                tweenTxt.color = Color.green;
            }

            tweenTxt.DOFade(_alfa, _duration);
        }

        public void ChangeRandomValue()
        {
            int _rndIndx = Random.Range(0, (int)CardStats.Lenght);
            int _rndValue = Random.Range(-2, 10);

            switch (_rndIndx)
            {
                case (int)CardStats.Mana:
                    SetStat(_rndValue, CardStats.Mana, manaTxt, tweenManaTxt);
                    break;
                case (int)CardStats.Attack:
                    SetStat(_rndValue, CardStats.Attack, attackTxt, tweenAttackTxt);
                    break;
                case (int)CardStats.Health:
                    SetStat(_rndValue, CardStats.Health, healthTxt, tweenHealthTxt);
                    break;
            }
        }
    }
}
