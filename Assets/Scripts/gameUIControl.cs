using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIControl : MonoBehaviour
{
    public static GameUIControl instance;

    public GameObject ingameUI;
    public Image trafficImage;
    public TMP_Text displayText;
    public Sprite[] trafficSprite;

    private float fadeTimeSpeed = 1f;
    private Coroutine fadeRoutine = null;
    private float maxAlpha = 0.75f;

    public void Start()
    {
        instance = this;

        ToggleIngameUI(false);
        ToggleGameText(false, false);
        ToggleTrafficLight(false);
    }

    //Enable Game Text
    public void ToggleGameText(bool state, bool useFade = true)
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            Image pImage = displayText.transform.parent.GetComponent<Image>();
            Color c = pImage.color;
            pImage.color = new Color(c.r, c.g, c.b, maxAlpha);
            c = displayText.color;
            displayText.color = new Color(c.r, c.g, c.b, 1);
            fadeRoutine = null;
        }
           

        if(useFade)
            if (state)
                fadeRoutine = StartCoroutine(FadeImage(displayText.transform.parent.GetComponent<Image>() , false, displayText));
            else
                fadeRoutine = StartCoroutine(FadeImage(displayText.transform.parent.GetComponent<Image>(), true, displayText));
        else
            displayText.transform.parent.gameObject.SetActive(state);

        if (state)
            displayText.text = "";
    }

    //Update text here
    public void GameText(string text)
    {
        displayText.text = text;
    }

    //Enable light
    public void ToggleTrafficLight(bool state)
    {
        trafficImage.gameObject.SetActive(state);
        trafficImage.sprite = trafficSprite[0];
    }

    public void ChangeTrafficLight(int spriteValue)
    {
        trafficImage.sprite = trafficSprite[(trafficSprite.Length - 1)- spriteValue];
    }

    IEnumerator FadeImage(MaskableGraphic img, bool fadeAway, MaskableGraphic text = null)
    {
        Color c;
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = maxAlpha; i >= 0; i -= (Time.deltaTime * fadeTimeSpeed))
            {
                // set color with i as alpha
                c = img.color;
                img.color  = new Color(c.r, c.g, c.b, i);

                if(text != null)
                {
                    c = text.color;
                    text.color = new Color(c.r, c.g, c.b, i);
                }
                    
                //img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= maxAlpha; i += (Time.deltaTime * fadeTimeSpeed))
            {
                // set color with i as alpha
                c = img.color;
                img.color = new Color(c.r, c.g, c.b, i);

                if (text != null)
                {
                    c = text.color;
                    text.color = new Color(c.r, c.g, c.b, i);
                }
                yield return null;
            }

            c = text.color;
            text.color = new Color(c.r, c.g, c.b, 1);
        }
    }

    public void ToggleIngameUI(bool enableUI)
    {
        ingameUI.SetActive(enableUI);
    }
}