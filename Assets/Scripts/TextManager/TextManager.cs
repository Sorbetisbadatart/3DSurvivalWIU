using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TextManager : MonoBehaviour
{
    public static TextManager TextInstance;
    public GameObject textPrefab;

    public RectTransform canvasTransform;

    public float speed;
    public Vector3 direction;
    public float fadingTime;


    private void Awake()
    {
        if (!TextInstance)
        {
            TextInstance = this;
        }
    }




    public void CreateText(Vector3 position, string text, Color colour)
    {
        GameObject _Text = Instantiate(textPrefab, position, Quaternion.identity);
        _Text.transform.SetParent(canvasTransform);
        _Text.GetComponent<RectTransform>().localScale = new Vector3(2,2,2);
        _Text.GetComponent<Text>().Initialize(speed,direction,fadingTime);
        _Text.GetComponent<TMP_Text>().text = text;
        _Text.GetComponent<TMP_Text>().color = colour;
        
    }
}

    
