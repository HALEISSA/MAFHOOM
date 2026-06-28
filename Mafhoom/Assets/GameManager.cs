using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<string> words = new List<string>();
    public int currentIndex = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
{
    if (words.Count == 0)
    {
        words = new List<string> { "ARRAY", "STACK", "QUEUE" };
    }
}



    public void SetWords(List<string> newWords)
    {
        words = new List<string>(newWords);
        currentIndex = 0;
    }

    public string GetCurrentWord()
    {
        if (words == null || words.Count == 0)
            return "";

        if (currentIndex >= words.Count)
            return "";

        return words[currentIndex];
    }

    public void NextWord()
    {
        currentIndex++;
    }

    public bool HasMoreWords()
    {
        return words != null && currentIndex < words.Count;
    }
}