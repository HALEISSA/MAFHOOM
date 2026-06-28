using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public DoorController door;

    private bool playerInRange = false;
    private bool pressed = false;

    void Update()
    {
        if (playerInRange && !pressed && Input.GetKeyDown(KeyCode.E))
        {
            ActivateButton();
        }
    }

    private void OnMouseDown()
    {
        if (playerInRange && !pressed)
        {
            ActivateButton();
        }
    }

    private void ActivateButton()
    {
        pressed = true;

        if (door != null)
        {
            door.UnlockDoor();
        }

        if (MazeUIManager.Instance != null)
        {
            MazeUIManager.Instance.ShowMessage("The door has been opened! Find it, enter, and solve your way out!");
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
{
    Debug.Log("Something entered: " + other.name);

    if (other.CompareTag("Player") && !pressed)
    {
        Debug.Log("Player detected!");
        playerInRange = true;

        if (MazeUIManager.Instance != null)
        {
            MazeUIManager.Instance.ShowMessage("Press E or click the button to activate it.");
        }
    }
}

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}