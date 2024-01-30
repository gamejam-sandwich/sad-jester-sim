using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Makeup : MonoBehaviour
{
    [SerializeField] private Image eyes;
    [SerializeField] private Image mouth;
    [SerializeField] private TextMeshProUGUI textBox;

    public void SetEyes(string imgFileName)
    {
        eyes.sprite = Resources.Load<Sprite>("Sprites/Characters/" + imgFileName);
    }

    public void SetMouth(string imgFileName)
    {
        mouth.sprite = Resources.Load<Sprite>("Sprites/Characters/" + imgFileName);
    }

    public void SetText(string desiredText)
    {
        textBox.text = desiredText;
    }
}
