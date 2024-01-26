using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SearchService;

public class NovelManager : MonoBehaviour
{
    private static NovelManager instance;

    [Header("Dialogue UI")]

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Other UI")]

    [SerializeField] private GameObject bgPanel;
    [SerializeField] private GameObject[] choices;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject startButton;

    private Story currentStory; //The Story object used to advance each line

    public TextAsset dialogueScript; //This is where the JSON file goes
    public InputAction submitAction; //This is controlling the user's action to select choices/advance dialogue


    //Current state
    public bool dialogueIsPlaying { get; private set; }
    private Coroutine displayLineCoroutine;
    private bool isLineScrolling = false;
    private bool desireSkipLineScrolling = false;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Multiple instances of NovelManager detected");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        startPanel.SetActive(true);
        StartCoroutine(SelectStart());

        foreach (GameObject button in choices)
        {
            button.SetActive(false);
        }
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        if (!dialogueIsPlaying) { return; }

        if (currentStory.currentChoices.Count == 0 && !isLineScrolling)
        {
            ContinueStory();
        }
        else if (isLineScrolling)
        {
            desireSkipLineScrolling = true;
        }
    }

    public void StartDialogue(TextAsset inkJSON)
    {
        startPanel.SetActive(false);
        AudioManager.GetInstance().SwitchTheme(" ");

        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;

        ContinueStory();
    }

    void EndDialogue()
    {
        dialogueIsPlaying = false;
        SceneManager.LoadScene("Jester Room");
    }

    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (isLineScrolling)
            {
                return;
            }

            if (displayLineCoroutine != null) StopCoroutine(displayLineCoroutine);
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            HandleTags(currentStory.currentTags);
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        HideChoices();

        isLineScrolling = true;

        foreach (char letter in line.ToCharArray())
        {
            if (desireSkipLineScrolling)
            {
                desireSkipLineScrolling = false;
                dialogueText.maxVisibleCharacters = line.Length;
                break;
            }

            dialogueText.maxVisibleCharacters++;
            AudioManager.GetInstance().PlaySound("Text");
            yield return new WaitForSeconds(0.03f);
        }
        DisplayChoices();
        isLineScrolling = false;
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');

            if (splitTag.Length != 2)
            {
                Debug.LogError("tag could not be parsed correctly. Tag: " + splitTag);
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            //handle tags
            switch (tagKey)
            {
                case "speaker":
                    nameText.text = (tagValue == "NONE") ? " " : tagValue + ':';
                    break;
                case "switchTheme":
                    AudioManager.GetInstance().SwitchTheme(tagValue);
                    break;
                case "playSound":
                    AudioManager.GetInstance().PlaySound(tagValue);
                    break;
                case "splash":
                    ImageManager.GetInstance().SetSplash(tagValue);
                    break;
            }
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("Too many choices requested than can be supported. Number of choices requested: " + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice option in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choices[index].GetComponentInChildren<TextMeshProUGUI>().text = option.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private void HideChoices()
    {
        foreach(GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        if (isLineScrolling) { return; }
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    private void OnEnable()
    {
        submitAction.Enable();
        submitAction.started += OnSubmit;
    }

    private void OnDisable()
    {
        submitAction.started -= OnSubmit;
        submitAction.Disable();
    }

    private IEnumerator SelectStart()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(startButton);
    }
}
