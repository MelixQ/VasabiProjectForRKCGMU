using UnityEngine;

public class CustomDontDestroyOnLoad : MonoBehaviour
{
    private static CustomDontDestroyOnLoad _instance;
    public static CustomDontDestroyOnLoad GetInstance() => _instance;
    
    private void Awake()
    {
        if (_instance is not null && _instance != this)
        {
            Debug.LogWarning($"More than one instance of {this} found in the scene");
            Destroy(gameObject);
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
