using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyText : MonoBehaviour
{
    private float randomWaitTime;
    private float timer = 0f;
    private float fadeInTime = 0.5f;

    private Image icon;
    private TextMeshProUGUI text;

    void Start()
    {
        icon = GetComponentInChildren<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0f);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        randomWaitTime = UnityEngine.Random.Range(0.2f, 0.8f);

        Object.Destroy(gameObject, 2f);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < randomWaitTime)
            return;

        transform.Translate(0f, 0.5f, 0f);
        if (timer < randomWaitTime + fadeInTime)
        {
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, icon.color.a + 0.04f);
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + 0.04f);
        }
        else
        {
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, icon.color.a - 0.02f);
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.02f);
        }
    }
}
