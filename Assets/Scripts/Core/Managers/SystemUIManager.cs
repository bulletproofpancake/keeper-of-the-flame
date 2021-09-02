using UnityEngine;
using TMPro;

public class SystemUIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += OnGameStart;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart += OnGameStart;
    }
    
    private void OnGameStart()
    {
        mainMenuUI.SetActive(false);
    }

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void ShowInstructions()
    {
        
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
