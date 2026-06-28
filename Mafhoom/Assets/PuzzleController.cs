using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PuzzleController : MonoBehaviour
{
    public Transform lettersParent;
    public GameObject letterPrefab;

    [Header("Final Scene")]
    public string finalSceneName = "FinalScene_Exit";

    private string correctWord;
    private List<LetterTile> tiles = new List<LetterTile>();
    private LetterTile firstSelectedTile;

    private void Start()
    {
        StartCoroutine(StartPuzzleWhenTermsAreReady());
    }

    private IEnumerator StartPuzzleWhenTermsAreReady()
    {
        yield return new WaitForSeconds(0.2f);

        if (StudentSessionManager.Instance != null)
            StudentSessionManager.Instance.StartPuzzleTimer();

        if (RoomTermsManager.Instance == null)
        {
            Debug.LogError("RoomTermsManager is missing.");
            yield break;
        }

        RoomTermsManager.Instance.LoadTermsFromRoom();

        yield return new WaitForSeconds(0.2f);

        correctWord = RoomTermsManager.Instance.GetRandomTerm();

        if (string.IsNullOrEmpty(correctWord))
        {
            Debug.LogError("No instructor term found for puzzle.");
            yield break;
        }

        correctWord = correctWord.Trim().ToUpper();

        CreatePuzzle(correctWord);

        Debug.Log("Correct instructor term: " + correctWord);
    }

    private void CreatePuzzle(string word)
    {
        foreach (Transform child in lettersParent)
        {
            Destroy(child.gameObject);
        }

        tiles.Clear();

        string scrambledWord = Shuffle(word);

        foreach (char letter in scrambledWord)
        {
            GameObject letterObject = Instantiate(letterPrefab, lettersParent);
            LetterTile letterTile = letterObject.GetComponent<LetterTile>();

            if (letterTile == null)
            {
                Debug.LogError("Letter prefab is missing LetterTile script.");
                return;
            }

            letterTile.SetLetter(letter.ToString());
            letterTile.controller = this;
            tiles.Add(letterTile);
        }

        Debug.Log("Scrambled word: " + scrambledWord);
    }

    public void SelectTile(LetterTile selectedTile)
    {
        if (selectedTile == null)
            return;

        if (firstSelectedTile == null)
        {
            firstSelectedTile = selectedTile;
            selectedTile.SetSelected(true);
            return;
        }

        if (firstSelectedTile == selectedTile)
        {
            firstSelectedTile.SetSelected(false);
            firstSelectedTile = null;
            return;
        }

        string tempLetter = firstSelectedTile.letter;
        firstSelectedTile.SetLetter(selectedTile.letter);
        selectedTile.SetLetter(tempLetter);

        firstSelectedTile.SetSelected(false);
        firstSelectedTile = null;

        CheckWin();
    }

    private void CheckWin()
    {
        string currentWord = "";

        foreach (LetterTile tile in tiles)
        {
            currentWord += tile.letter;
        }

        currentWord = currentWord.ToUpper();

        Debug.Log("Current word: " + currentWord);

        if (currentWord == correctWord)
        {
            Debug.Log("Puzzle solved. Going to final scene.");

            if (StudentSessionManager.Instance != null)
                StudentSessionManager.Instance.StopPuzzleTimer();

            SceneManager.LoadScene(finalSceneName);
        }
    }

    private string Shuffle(string word)
    {
        char[] letters = word.ToCharArray();

        for (int i = 0; i < letters.Length; i++)
        {
            int randomIndex = Random.Range(0, letters.Length);

            char temp = letters[i];
            letters[i] = letters[randomIndex];
            letters[randomIndex] = temp;
        }

        string shuffledWord = new string(letters);

        if (shuffledWord == word && word.Length > 1)
            return Shuffle(word);

        return shuffledWord;
    }
}