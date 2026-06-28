using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public GameObject door;

    void OnMouseDown()
    {
        door.SetActive(false);
    }
}
