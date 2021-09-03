using UnityEngine;
using TMPro;

public class SystemUIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject instructionsUI;
    [SerializeField] private GameObject gameEndUI;

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
        AudioManager.Instance.Play("Click");
        GameManager.Instance.StartGame();
    }

    public void ShowInstructions()
    {
        AudioManager.Instance.Play("Click");
        mainMenuUI.SetActive(false);
        instructionsUI.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        AudioManager.Instance.Play("Click");
        mainMenuUI.SetActive(true);
        instructionsUI.SetActive(false);
        gameEndUI.SetActive(false);
    }
    
    public void ExitGame()
    {
        AudioManager.Instance.Play("Click");
        Application.Quit();
    }
}
