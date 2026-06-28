using UnityEngine;

public class FinalSceneManager : MonoBehaviour
{
    public Transform exitPoint;
    public FinalDialogueManager dialogueManager;
    public Animator professorAnimator;
    public FinalMessageManager finalMessageManager;

    private GameObject player;
    private AutoWalkToPoint autoWalk;

    private void Start()
    {
        if (professorAnimator != null)
        {
            professorAnimator.SetBool("isClapping", true);
        }
    }

    public void RegisterPlayer(GameObject spawnedPlayer, AutoWalkToPoint playerAutoWalk)
    {
        player = spawnedPlayer;
        autoWalk = playerAutoWalk;

        autoWalk.OnArrived += OnPlayerReachedProfessor;
    }

    private void OnPlayerReachedProfessor()
    {
        Debug.Log("Player reached professor. Stopping clap and starting dialogue.");

        if (professorAnimator != null)
        {
            professorAnimator.SetBool("isClapping", false);
        }

        if (dialogueManager != null)
        {
            dialogueManager.OnDialogueFinished += SendPlayerToExit;
            dialogueManager.StartDialogue();
        }
        else
        {
            Debug.LogError("DialogueManager is not assigned in FinalSceneManager.");
        }
    }

    private void SendPlayerToExit()
    {
        Debug.Log("Dialogue finished. Player walking to exit.");

        if (autoWalk != null && exitPoint != null)
        {
            autoWalk.StartWalking(exitPoint, OnPlayerReachedExit);
        }
    }

    private void OnPlayerReachedExit()
{
    Debug.Log("Player reached gate. Final motivational message should appear.");

    if (finalMessageManager != null)
    {
        finalMessageManager.ShowFinalMessage();
    }
    else
    {
        Debug.LogError("FinalMessageManager is not assigned in FinalSceneManager.");
    }
}
}