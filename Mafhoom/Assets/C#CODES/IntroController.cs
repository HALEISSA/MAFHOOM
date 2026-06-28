using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    public TextMeshProUGUI mafhoomText;

    [Header("Typing Settings")]
    public float letterTypingSpeed = 0.12f;
    public float questionMarkTypingSpeed = 0.2f;
    public float questionMarkDeletingSpeed = 0.12f;
    public float pauseAfterFullText = 0.5f;
    public float pauseBetweenQuestionCycles = 0.35f;

    [Header("Scene Settings")]
    public string nextSceneName = "Scene_5 Soomi Maze";
    public float finalWaitBeforeLoad = 2.5f;

    [Header("Runners")]
    public Transform boyRunner;
    public Transform girlRunner;
    public float boySpeed = 3f;
    public float girlSpeed = 3f;
    public float girlStartDelay = 0.8f;

    private bool girlCanMove = false;

    private void Start()
    {
        StartCoroutine(IntroSequence());
        StartCoroutine(RunCharacters());
    }

    private IEnumerator IntroSequence()
    {
        mafhoomText.text = "";

        string mainWord = "MAFHOOM";
        string marks = "???";

        for (int i = 0; i < mainWord.Length; i++)
        {
            mafhoomText.text += mainWord[i];
            yield return new WaitForSeconds(letterTypingSpeed);
        }

        yield return new WaitForSeconds(pauseAfterFullText);

        for (int i = 0; i < marks.Length; i++)
        {
            mafhoomText.text = mainWord + marks.Substring(0, i + 1);
            yield return new WaitForSeconds(questionMarkTypingSpeed);
        }

        yield return new WaitForSeconds(pauseBetweenQuestionCycles);

        float timer = 0f;

        while (timer < finalWaitBeforeLoad)
        {
            for (int i = 3; i >= 0; i--)
            {
                mafhoomText.text = mainWord + marks.Substring(0, i);
                yield return new WaitForSeconds(questionMarkDeletingSpeed);
                timer += questionMarkDeletingSpeed;

                if (timer >= finalWaitBeforeLoad)
                    break;
            }

            if (timer >= finalWaitBeforeLoad)
                break;

            yield return new WaitForSeconds(pauseBetweenQuestionCycles);
            timer += pauseBetweenQuestionCycles;

            if (timer >= finalWaitBeforeLoad)
                break;

            for (int i = 1; i <= 3; i++)
            {
                mafhoomText.text = mainWord + marks.Substring(0, i);
                yield return new WaitForSeconds(questionMarkTypingSpeed);
                timer += questionMarkTypingSpeed;

                if (timer >= finalWaitBeforeLoad)
                    break;
            }

            if (timer >= finalWaitBeforeLoad)
                break;

            yield return new WaitForSeconds(pauseBetweenQuestionCycles);
            timer += pauseBetweenQuestionCycles;
        }

        Debug.Log("Loading maze scene: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator RunCharacters()
    {
        StartCoroutine(EnableGirlMovementAfterDelay());

        while (true)
        {
            if (boyRunner != null)
                boyRunner.Translate(Vector3.right * boySpeed * Time.deltaTime);

            if (girlCanMove && girlRunner != null)
                girlRunner.Translate(Vector3.right * girlSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator EnableGirlMovementAfterDelay()
    {
        yield return new WaitForSeconds(girlStartDelay);
        girlCanMove = true;
    }
}