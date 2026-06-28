using UnityEngine;

public class PuzzleGenerator : MonoBehaviour
{
    public static PuzzleGenerator Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public string GenerateScrambledPuzzle(string originalWord)
    {
        if (string.IsNullOrEmpty(originalWord))
        {
            Debug.LogWarning("Cannot generate puzzle from an empty word.");
            return "";
        }

        char[] letters = originalWord.ToCharArray();

        for (int i = 0; i < letters.Length; i++)
        {
            int randomIndex = Random.Range(0, letters.Length);

            char temp = letters[i];
            letters[i] = letters[randomIndex];
            letters[randomIndex] = temp;
        }

        string scrambled = new string(letters);

        // Optional safety check so the scrambled word is not exactly the same
        if (scrambled == originalWord && originalWord.Length > 1)
        {
            return GenerateScrambledPuzzle(originalWord);
        }

        return scrambled;
    }
}