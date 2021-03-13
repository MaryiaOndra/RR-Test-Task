using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    const string IMAGE_PATH = "https://picsum.photos/200/300";

    [SerializeField] Image icon;
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text discription;
    [SerializeField] TMP_Text mana;
    [SerializeField] TMP_Text attack;
    [SerializeField] TMP_Text health;


    private void Start()
    {
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

    public void SetIcon(Sprite cardSprite) 
    {
        icon.sprite = cardSprite;
    }
}
