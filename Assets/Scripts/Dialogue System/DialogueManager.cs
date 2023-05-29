using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
// ReSharper disable InconsistentNaming

public class DialogueManager : MonoBehaviour
{
    [field: SerializeField]
    public GameObject HintPopup { get; private set; }
    public bool DialogueIsPlaying { get; private set; }

    private static DialogueManager _instance;

    [Header("Dialogue UI")] [SerializeField]
    private GameObject _dialoguePanel;
    [SerializeField] private GameObject _notePanel;
    [SerializeField] private Animator _portraitAnimator;
    [SerializeField] private TextMeshProUGUI _displayNameText;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    private Animator _layoutAnimator;

    [Header("Choices UI")] [SerializeField]
    private GameObject[] _choices;
    private TextMeshProUGUI[] _choicesText;

    [Header("Note texts")] [SerializeField]
    private TextAsset[] _notes;
    private Dictionary<string, TextAsset> _notesDictionary;

    private Story _currentStory;
    
    public static DialogueManager GetInstance() => _instance;

    private const string SpeakerTag = "speaker";
    private const string PortraitTag = "portrait";
    private const string LayoutTag = "layout";
    private const string NoteTag = "note";

    private void Awake()
    {
        if (_instance is not null)
            Debug.LogWarning($"More than one instance of {this} found in the scene");
        _instance = this;
    }

    private void Start()
    {
        DialogueIsPlaying = false;
        _dialoguePanel.SetActive(false);
        _layoutAnimator = _dialoguePanel.GetComponent<Animator>();
        _notesDictionary = new Dictionary<string, TextAsset>();
        foreach (var note in _notes)
            _notesDictionary.Add(note.name, note);

        _choicesText = new TextMeshProUGUI[_choices.Length];
        var index = 0;
        foreach (var choice in _choices)
        {
            _choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!DialogueIsPlaying) return;

        if (InputManager.GetInstance().GetSubmitPressed() && _currentStory.currentChoices.Count == 0)
            ContinueStory();
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        _currentStory = new Story(inkJSON.text);
        DialogueIsPlaying = true;
        _dialoguePanel.SetActive(true);
        
        // Reset Portrait, Layout and Speaker
        _displayNameText.text = "???";
        _portraitAnimator.Play(AnimatorParameterIdList.Default);
        _layoutAnimator.Play(AnimatorParameterIdList.Default);
        ContinueStory();
    }

    public void MakeChoice(int choiceIndex)
    {
        _currentStory.ChooseChoiceIndex(choiceIndex);
        InputManager.GetInstance().RegisterSubmitPressed();
        ContinueStory();
    }

    public void TraverseBack()
    {
        _currentStory.ResetState();
        _layoutAnimator.Play(AnimatorParameterIdList.Default);
        ContinueStory();
    }

    public void SetDialoguePanelActiveState(bool value) => _dialoguePanel.SetActive(value);

    private void ContinueStory()
    {
        if (_currentStory.canContinue)
        {
            _dialogueText.text = _currentStory.Continue();
            DisplayChoices();
            // Handle Tags
            HandleTags(_currentStory.currentTags);
        }
        else
            StartCoroutine(ExitDialogueMode());
    }

    private void HandleTags(List<string> currentTags)
    {
        var interrupted = false;
        foreach (var inkTag in currentTags.ToList())
        {
            if (interrupted) break;
            var splitTag = inkTag.Split(':');
            if (splitTag.Length != 2)
                Debug.LogError($"Tag parse fail. Please check provided string for errors: {inkTag}");
            
            var tagKey = splitTag[0].Trim();
            var tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case NoteTag:
                    interrupted = true;
                    SetDialoguePanelActiveState(false);
                    _notePanel.SetActive(true);
                    var noteTextField = _notePanel.GetComponentInChildren<TextMeshProUGUI>();
                    noteTextField.text = _notesDictionary[tagValue].text;
                    break;
                case SpeakerTag:
                    _displayNameText.text = tagValue;
                    break;
                case PortraitTag:
                    _portraitAnimator.Play(tagValue);
                    break;
                case LayoutTag:
                    _layoutAnimator.Play(tagValue);
                    break;
                default:
                    Debug.LogWarning($"Tag cane correct but cannot being handled: {inkTag}");
                    break;
            }
        }
    }

    private void ResetTags(ref List<string> currentTags)
    {
        currentTags.RemoveRange(2, currentTags.Count - 2);
    }

    private void DisplayChoices()
    {
        var currentChoices = _currentStory.currentChoices;

        if (currentChoices.Count > _choices.Length)
            Debug.LogError($"More choices were given than the UI can support. Choices given: {currentChoices.Count}");

        var index = 0;
        foreach(var choice in currentChoices) 
        {
            _choices[index].gameObject.SetActive(true);
            _choicesText[index].text = choice.text;
            index++;
        }
        
        // Hide remaining choices that UI supports.
        for (var i = index; i < _choices.Length; i++)
            _choices[i].gameObject.SetActive(false);
        
        //StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(_choices[0].gameObject);
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(.2f);
        DialogueIsPlaying = false;
        _dialoguePanel.SetActive(false);
        _dialogueText.text = "";
    }
}
