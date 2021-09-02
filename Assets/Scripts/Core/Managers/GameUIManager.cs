using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private TextMeshProUGUI playerLivesCount;
    
    private PlayerCombat _playerCombat;


    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += () => playerCanvas.SetActive(true);
    }

    private void Awake()
    {
        _playerCombat = FindObjectOfType<PlayerCombat>();
    }

    private void Start()
    {
        playerCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameStart) return;
        playerLivesCount.text = $"x {_playerCombat.Health}";
    }
}
