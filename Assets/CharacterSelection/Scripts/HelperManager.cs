using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class HelperManager : MonoBehaviour
{
    public delegate void EnterExitHandler();
    public static event EnterExitHandler Entered;
    public static event EnterExitHandler Exited;
    
    [SerializeField] private GameObject _firstStageHelper;
    [SerializeField] private GameObject _secondStageHelper;
    [SerializeField] private PlayableDirector _timelineCutscene;
    [SerializeField] private GameObject _selectCanvas;
    private Collider _secondStageHelperCollider;
    
    public bool DialogueIsPlaying { get; private set; }
    [Header("Dialogue UI")] [SerializeField]
    private GameObject _dialoguePanel;
    [SerializeField] private Animator _portraitAnimator;
    [SerializeField] private TextMeshProUGUI _displayNameText;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    private Animator _layoutAnimator;

    [Header("Choices UI")] [SerializeField]
    private GameObject[] _choices;
    private TextMeshProUGUI[] _choicesText;
    
    private const string SpeakerTag = "speaker";
    private const string PortraitTag = "portrait";
    private const string LayoutTag = "layout";
    private Story _currentStory;
    
    private static HelperManager _instance;
    public static HelperManager GetInstance() => _instance;
    private void Awake()
    {
        if (_instance is not null)
            Debug.LogWarning($"More than one instance of {this} found in the scene");
        _instance = this;
    }

    private void Start()
    {
        _secondStageHelperCollider = _secondStageHelper.GetComponent<Collider>();
        _firstStageHelper.SetActive(true);
        _secondStageHelper.SetActive(false);
        Entered += DisableSelectCanvas;
        Exited += ManageDialogue;
        
        DialogueIsPlaying = false;
        _dialoguePanel.SetActive(false);
        _layoutAnimator = _dialoguePanel.GetComponent<Animator>();
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
        Entered?.Invoke();
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
        Exited?.Invoke();
    }
    
    private void DisableSelectCanvas()
    {
        if (_selectCanvas.activeSelf)
            _selectCanvas.SetActive(false);
    }
    private void ManageDialogue()
    {
        _selectCanvas.SetActive(true);
        _secondStageHelperCollider.enabled = true;
        if (_secondStageHelper.activeSelf) return;
        
        _firstStageHelper.SetActive(false);
        _secondStageHelper.SetActive(true);
        _timelineCutscene.Play();
        _selectCanvas.SetActive(true);
    }
    
    //private void OnDestroy() => DialogueManager.Exited -= ManageDialogue;
}
