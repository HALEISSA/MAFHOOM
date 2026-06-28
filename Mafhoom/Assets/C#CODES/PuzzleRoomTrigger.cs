using UnityEngine;
using TMPro;

public class PuzzleRoomTrigger : MonoBehaviour
{
    [Header("Optional Puzzle UI")]
    public TextMeshProUGUI puzzleTextUI;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        hasTriggered = true;

        // Start tracking how long the student takes
        if (StudentSessionManager.Instance != null)
        {
            StudentSessionManager.Instance.StartPuzzleTimer();
        }

        if (GameManager.instance == null)
        {
            Debug.LogWarning("GameManager instance is missing.");
            return;
        }

        string selectedTerm = GameManager.instance.GetCurrentWord();

        if (string.IsNullOrEmpty(selectedTerm))
        {
            Debug.LogWarning("No term was selected for puzzle generation.");
            return;
        }

        if (PuzzleGenerator.Instance == null)
        {
            Debug.LogWarning("PuzzleGenerator instance is missing.");
            return;
        }

        string generatedPuzzle = PuzzleGenerator.Instance.GenerateScrambledPuzzle(selectedTerm);

        Debug.Log("Original term: " + selectedTerm);
        Debug.Log("Generated puzzle: " + generatedPuzzle);

        if (puzzleTextUI != null)
        {
            puzzleTextUI.text = generatedPuzzle;
        }
    }
}