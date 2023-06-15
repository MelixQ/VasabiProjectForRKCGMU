using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
	public GameObject[] characters;
	private int selectedCharacter;

	[Space] [Header("Correct character index in hierarchy")] [SerializeField]
	private int _correctCharacter;
	[Header("Build index of main game scene")] [SerializeField]
	private int _correctSceneName;
	[Header("Build index of incorrect scene")] [SerializeField] 
	private int _incorrectSceneName;
	[Header("Note settings")] [SerializeField] 
	private TextAsset _noteContent;
	[SerializeField] private GameObject _noteCanvas;
	[SerializeField] private GameObject _selectCanvas;

	public void NextCharacter()
	{
		characters[selectedCharacter].SetActive(false);
		selectedCharacter = (selectedCharacter + 1) % characters.Length;
		characters[selectedCharacter].SetActive(true);
	}

	public void PreviousCharacter()
	{
		characters[selectedCharacter].SetActive(false);
		selectedCharacter--;
		if (selectedCharacter < 0)
		{
			selectedCharacter += characters.Length;
		}
		characters[selectedCharacter].SetActive(true);
	}

	public void StartGame()
	{
		PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
		if (selectedCharacter == _correctCharacter)
			SceneManager.LoadScene(_correctSceneName, LoadSceneMode.Single);
		else
		{
			_noteCanvas.SetActive(true);
			_selectCanvas.SetActive(false);
			NoteHandler.GetInstance().SetupNoteContent(_noteContent.text);
		}
	}

	public void EnableSelection()
	{
		_selectCanvas.SetActive(true);
		_noteCanvas.SetActive(false);
	}
}
