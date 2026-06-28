using UnityEngine;

public class MazeManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject doorPrefab;
    public Transform wallsParent;

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        Transform[] walls = wallsParent.GetComponentsInChildren<Transform>();

        // skip index 0 (parent itself)
        Transform randomWallForDoor = walls[Random.Range(1, walls.Length)];
        Transform randomWallForButton = walls[Random.Range(1, walls.Length)];

        while (randomWallForButton == randomWallForDoor)
        {
            randomWallForButton = walls[Random.Range(1, walls.Length)];
        }

        Vector2 doorPosition = GetPositionOnWall(randomWallForDoor, doorPrefab);
        Vector2 buttonPosition = GetPositionOnWall(randomWallForButton, buttonPrefab);

        Quaternion doorRotation = GetWallRotation(randomWallForDoor, doorPrefab);
        Quaternion buttonRotation = GetWallRotation(randomWallForButton, buttonPrefab);

        GameObject door = Instantiate(doorPrefab, doorPosition, doorRotation);
        GameObject button = Instantiate(buttonPrefab, buttonPosition, buttonRotation);

        button.GetComponent<ButtonController>().door =
            door.GetComponent<DoorController>();
    }

    Vector2 GetPositionOnWall(Transform wall, GameObject objPrefab)
    {
        BoxCollider2D wallCollider = wall.GetComponent<BoxCollider2D>();
        BoxCollider2D objCollider = objPrefab.GetComponent<BoxCollider2D>();

        float wallWidth = wallCollider.size.x * wall.localScale.x;
        float wallHeight = wallCollider.size.y * wall.localScale.y;

        float objHalfWidth = objCollider.size.x * objPrefab.transform.localScale.x / 2f;
        float objHalfHeight = objCollider.size.y * objPrefab.transform.localScale.y / 2f;

        Vector2 wallCenter = wall.position;
        Vector2 spawnPosition = Vector2.zero;
        bool validPosition = false;

        while (!validPosition)
        {
            if (wallWidth > wallHeight) // horizontal wall
            {
                float randomX = Random.Range(
                    wallCenter.x - wallWidth / 2 + objHalfWidth,
                    wallCenter.x + wallWidth / 2 - objHalfWidth
                );

                float offset = (wallHeight / 2) + objHalfHeight;

                Vector2 topPos = new Vector2(randomX, wallCenter.y + offset);
                Vector2 bottomPos = new Vector2(randomX, wallCenter.y - offset);

                spawnPosition = !Physics2D.OverlapCircle(topPos, 1.2f)
                    ? topPos
                    : bottomPos;
            }
            else // vertical wall
            {
                float randomY = Random.Range(
                    wallCenter.y - wallHeight / 2 + objHalfHeight,
                    wallCenter.y + wallHeight / 2 - objHalfHeight
                );

                float offset = (wallWidth / 2) + objHalfWidth;

                Vector2 rightPos = new Vector2(wallCenter.x + offset, randomY);
                Vector2 leftPos = new Vector2(wallCenter.x - offset, randomY);

                spawnPosition = !Physics2D.OverlapCircle(rightPos, 1.2f)
                    ? rightPos
                    : leftPos;
            }

            if (!Physics2D.OverlapCircle(spawnPosition, 1.2f))
                validPosition = true;
        }

        return spawnPosition;
    }

    Quaternion GetWallRotation(Transform wall, GameObject objPrefab)
    {
        BoxCollider2D wallCollider = wall.GetComponent<BoxCollider2D>();

        float wallWidth = wallCollider.size.x * wall.localScale.x;
        float wallHeight = wallCollider.size.y * wall.localScale.y;

        bool isHorizontalWall = wallWidth > wallHeight;

        // BUTTON prefab is vertical by default
        if (objPrefab == buttonPrefab)
        {
            if (isHorizontalWall)
                return Quaternion.Euler(0, 0, 90); // rotate to horizontal
            else
                return Quaternion.identity; // keep vertical
        }

        // DOOR prefab is horizontal by default
        if (objPrefab == doorPrefab)
        {
            if (isHorizontalWall)
                return Quaternion.identity; // keep horizontal
            else
                return Quaternion.Euler(0, 0, 90); // rotate to vertical
        }

        return Quaternion.identity;
    }
}
