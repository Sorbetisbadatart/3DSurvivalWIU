using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Text : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float fadingTime;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        Explode();

    }

    public void Initialize(float speed, Vector3 direction, float fadingTime)
    {
        this.speed = speed;
        this.fadingTime = fadingTime;
        this.direction = direction;

        StartCoroutine(nameof(FadeOut));
    }
    public void Explode()
    {
        if (transform.position.y > Screen.height + 100)
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator FadeOut()
    {
        float startAlpha = GetComponent<TMP_Text>().color.a;

        float rate = 1.0f / fadingTime;
        float percentFinish = 0.0f;

        while (percentFinish < 1.0f)
        {
            Color TempColour = GetComponent<TMP_Text>().color;
            GetComponent<TMP_Text>().color = new Color(TempColour.r, TempColour.g, TempColour.b, Mathf.Lerp(startAlpha, 0, percentFinish));
            percentFinish += rate * Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }
}
