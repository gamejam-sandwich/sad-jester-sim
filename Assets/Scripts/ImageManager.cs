using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    private static ImageManager instance;

    [SerializeField] private GameObject splashPanel;
    private Image splashImage;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Multiple instances of ImageManager detected");
        }

        instance = this;
    }

    void Start()
    {
        splashImage = splashPanel.GetComponent<Image>();
    }

    public static ImageManager GetInstance()
    {
        return instance;
    }

    public void SetExpression(string charName, string imageName)
    {

    }

    public void SetSplash(string imageFileName)
    {
        if (imageFileName == "None") splashImage.sprite = null;
        splashImage.sprite = Resources.Load<Sprite>("Sprites/Splash/" + imageFileName);
        splashImage.color = (splashImage.sprite != null) ? splashImage.color = new Color(1, 1, 1, 100) : splashImage.color = new Color(255, 255, 255, 0);
    }
}
