using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField] TMP_Text text;

    [Header("Stats")]
    [SerializeField] TMP_Text manaTxt;
    [SerializeField] TMP_Text attackTxt;
    [SerializeField] TMP_Text healthTxt;

    public int ManaValue { get; private set; }
    public int AttackValue { get; private set; }
    public int HealthValue { get; private set; }

    TMP_Text _tweenTxt;

    private void Start()
    {
        SetRandomStats();
        StartCoroutine(SetImageFromWebToCard());
    }

    IEnumerator SetImageFromWebToCard()
    {
        using (var uwr = new UnityWebRequest(IMAGE_PATH, UnityWebRequest.kHttpVerbGET))
        {
            uwr.downloadHandler = new DownloadHandlerTexture();
            yield return uwr.SendWebRequest();
            Texture2D _texture = DownloadHandlerTexture.GetContent(uwr);
            Sprite _sprite = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            icon.sprite = _sprite;
        }
    }

    void SetRandomStats() 
    {
        int _rndValue = Random.Range(1, 10);
        SetRandomStat(_rndValue, CardStats.Health, healthTxt);

        _rndValue = Random.Range(1, 10);
        SetRandomStat(_rndValue, CardStats.Mana, manaTxt); 
        
        _rndValue = Random.Range(1, 10);
        SetRandomStat(_rndValue, CardStats.Attack, attackTxt);

        Debug.Log("HealthValue" + HealthValue );
        Debug.Log("ManaValue" + ManaValue);
        Debug.Log("AttackValue" + AttackValue);
    }

    //void SetRandomMana(int rndValue) 
    //{
    //    int diff = rndValue - ManaValue;

    //    if (rndValue != ManaValue)
    //    {
    //        CreateTextTween(diff, manaTxt);
    //    }

    //    ManaValue = rndValue;
    //    manaTxt.text = ManaValue.ToString();       
    //}

    //void SetRandomAttack(int rndValue)
    //{
    //    int diff = rndValue - AttackValue;

    //    if (rndValue != ManaValue)
    //    {
    //        CreateTextTween(diff, attackTxt);
    //    }

    //    AttackValue = rndValue;
    //    attackTxt.text = AttackValue.ToString();
    //}

    //void SetRandomHealth(int rndValue)
    //{
    //    int diff = rndValue - HealthValue;

    //    if (rndValue != ManaValue)
    //    {
    //        CreateTextTween(diff, healthTxt);
    //    }

    //    HealthValue = rndValue;
    //    healthTxt.text = HealthValue.ToString();
    //}

    void SetRandomStat(int rndValue, CardStats cardStats, TMP_Text textToShow) 
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
                break;
        }
       
        int _diff = rndValue - _property;

        if (rndValue != _property)
        {
            CreateTextTween(_diff, textToShow);
        }

        _property = rndValue;
        textToShow.text = _property.ToString();
    }

    private void CreateTextTween(int diff ,TMP_Text txt)
    {
        _tweenTxt = Instantiate(txt, transform);
        float _duration = 2f;
        float _scale = 2f;
        float _alfa = 0f;

        if (diff < 0)
        {
            _tweenTxt.text = diff.ToString();
            _tweenTxt.color = Color.red;
            Debug.Log("MANA" + _tweenTxt.text);
        }
        else
        {
            _tweenTxt.text = "+" + diff;
            _tweenTxt.color = Color.blue;
        }

        _tweenTxt.transform.DOScale(_scale, _duration);
        _tweenTxt.DOFade(_alfa, _duration).OnComplete(DestroyText);
    }

    void DestroyText() 
    {
        Destroy(_tweenTxt);
    }


    public void SetIcon(Sprite cardSprite) 
    {
        icon.sprite = cardSprite;
    }

    public void ChangeRandomValue() 
    {
        int _rndIndx = Random.Range(0, (int)CardStats.Lenght);
        int _rndValue = Random.Range(-2, 10);

        switch (_rndIndx)
        {
            case (int)CardStats.Mana:
                SetRandomStat(_rndValue, CardStats.Mana, manaTxt);
                break;
            case (int)CardStats.Attack:
                SetRandomStat(_rndValue, CardStats.Attack, attackTxt);
                break;
            case (int)CardStats.Health:
                SetRandomStat(_rndValue, CardStats.Health, healthTxt);
                break;
        }
    }
}
