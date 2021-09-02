using UnityEngine;
using TMPro;

public class SystemUIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject instructionsUI;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += OnGameStart;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= OnGameStart;
    }


    private void Start()
    {
        mainMenuUI.SetActive(true);
        instructionsUI.SetActive(false);
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
        mainMenuUI.SetActive(false);
        instructionsUI.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        mainMenuUI.SetActive(true);
        instructionsUI.SetActive(false);
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
