using UnityEngine;

public class CallbackStoryState : MonoBehaviour
{
    public void Callback()
    {
        gameObject.SetActive(false);
        DialogueManager.GetInstance().SetDialoguePanelActiveState(true);
        DialogueManager.GetInstance().TraverseBack();
    }
}
