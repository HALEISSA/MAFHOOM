using UnityEngine;

public class MazeRandomSpawner : MonoBehaviour
{
    [Header("Objects")]
    public GameObject door;
    public GameObject button;

    [Header("Maze Area")]
    public float minX = -28f;
    public float maxX = 28f;
    public float minY = -28f;
    public float maxY = 28f;

    [Header("Layers")]
    public LayerMask wallLayer;

    [Header("Spawn Settings")]
    public float objectCheckRadius = 0.45f;
    public float wallDetectDistance = 2f;
    public float surfaceOffset = 0.05f;
    public float minDistanceBetweenDoorAndButton = 8f;

    private void Start()
    {
        RandomizePositions();
    }

    public void RandomizePositions()
    {
        SpawnObjectOnWall(door.transform, true);

        int tries = 0;
        do
        {
            SpawnObjectOnWall(button.transform, false);
            tries++;
        }
        while (Vector2.Distance(door.transform.position, button.transform.position) < minDistanceBetweenDoorAndButton && tries < 100);
    }

    private void SpawnObjectOnWall(Transform obj, bool isDoor)
    {
        Vector2 randomPos;
        RaycastHit2D hit;
        Vector2 direction;

        int tries = 0;

        do
        {
            randomPos = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            hit = FindNearestWall(randomPos, out direction);
            tries++;
        }
        while (!IsValidWallHit(randomPos, hit) && tries < 400);

        PlaceAndRotate(obj, hit, direction, isDoor);
    }

    private RaycastHit2D FindNearestWall(Vector2 pos, out Vector2 direction)
    {
        Vector2[] directions =
        {
            Vector2.left,
            Vector2.right,
            Vector2.up,
            Vector2.down
        };

        RaycastHit2D bestHit = new RaycastHit2D();
        direction = Vector2.zero;
        float closestDistance = Mathf.Infinity;

        foreach (Vector2 dir in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(pos, dir, wallDetectDistance, wallLayer);

            if (hit.collider != null && hit.distance < closestDistance)
            {
                closestDistance = hit.distance;
                bestHit = hit;
                direction = dir;
            }
        }

        return bestHit;
    }

    private bool IsValidWallHit(Vector2 pos, RaycastHit2D hit)
    {
        if (hit.collider == null)
            return false;

        bool insideWall = Physics2D.OverlapCircle(pos, objectCheckRadius, wallLayer) != null;

        return !insideWall;
    }

    private void PlaceAndRotate(Transform obj, RaycastHit2D hit, Vector2 direction, bool isDoor)
    {
        obj.position = hit.point + hit.normal * surfaceOffset;

        if (isDoor)
        {
            if (direction == Vector2.left || direction == Vector2.right)
            {
                obj.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (direction == Vector2.up || direction == Vector2.down)
            {
                obj.rotation = Quaternion.Euler(0, 0, 90);
            }
        }
        else
        {
            obj.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
