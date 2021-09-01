using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerLivesCount;
    
    private PlayerCombat _playerCombat;

    private void Awake()
    {
        _playerCombat = FindObjectOfType<PlayerCombat>();
    }

    private void Update()
    {
        playerLivesCount.text = $"x {_playerCombat.Health}";
    }
}
