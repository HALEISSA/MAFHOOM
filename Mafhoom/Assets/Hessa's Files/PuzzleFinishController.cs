using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PuzzleFinishController : MonoBehaviour
{
    [Header("Puzzle")]
    public TMP_InputField playerInput;
    public string correctAnswer;

    public void CheckAnswerAndFinish()
    {
        if (playerInput == null)
        {
            Debug.LogWarning("Input field not assigned.");
            return;
        }

        string answer = playerInput.text.Trim().ToLower();
        string correct = correctAnswer.Trim().ToLower();

        if (answer == correct)
        {
            Debug.Log("Correct answer!");

            // 🔥 STOP TIMER (VERY IMPORTANT)
            if (StudentSessionManager.Instance != null)
            {
                StudentSessionManager.Instance.StopPuzzleTimer();
            }

            // 🚀 LOAD FINAL SCENE (your exact name)
            SceneManager.LoadScene("FinalScene_Exit");
        }
        else
        {
            Debug.Log("Wrong answer, try again.");
        }
    }
}