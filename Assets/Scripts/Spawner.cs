using UnityEngine;
using Meta.XR.MRUtilityKit;

public class Spawner : MonoBehaviour
{
    public MRUKAnchor.SceneLabels labelFilters = MRUKAnchor.SceneLabels.WALL_FACE | MRUKAnchor.SceneLabels.FLOOR;
    public MRUKAnchor.SceneLabels excludedLabels = MRUKAnchor.SceneLabels.COUCH | MRUKAnchor.SceneLabels.TABLE | MRUKAnchor.SceneLabels.STORAGE;

    private MRUKRoom room;
    [HideInInspector] public Vector3 safePosition;
    public float maxRayDistance = 10f;
    private float fixedHeight; // Set your desired fixed height here
    private void Start()
    {
        fixedHeight=transform.position.y;
    }
    public void FindCenterSafePositionInRoom()
    {
        room = MRUK.Instance.GetCurrentRoom();

        if (room != null)
        {
            Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
            Vector3 totalHitPoint = Vector3.zero;
            int hitCount = 0;

            foreach (Vector3 dir in directions)
            {
                if (TryFindPositionWithRay(transform.position, dir, maxRayDistance, out Vector3 hitPoint))
                {
                    totalHitPoint += hitPoint;
                    hitCount++;
                }
            }

            if (hitCount > 0)
            {
                safePosition = totalHitPoint / hitCount;

                // Ensure the safe position maintains a fixed height (y-coordinate)
                safePosition.y = fixedHeight;

                // Set the object's position to the calculated safe position
                transform.position = safePosition;

                Debug.Log("Spawner positioned at the center of the room at fixed height.");
            }
            else
            {
                Debug.LogError("No safe position found.");
            }
        }
        else
        {
            Debug.LogError("MRUK room not found.");
        }
    }

    private bool TryFindPositionWithRay(Vector3 origin, Vector3 direction, float distance, out Vector3 hitPoint)
    {
        Ray ray = new Ray(origin, direction);
        bool hasHit = room.Raycast(ray, distance, new LabelFilter(labelFilters), out RaycastHit hit, out MRUKAnchor anchor);

        if (hasHit && !IsExcludedLabel(anchor.Label) && (anchor.Label == MRUKAnchor.SceneLabels.WALL_FACE || anchor.Label == MRUKAnchor.SceneLabels.FLOOR))
        {
            hitPoint = hit.point;
            return true;
        }

        hitPoint = Vector3.zero;
        return false;
    }

    private bool IsExcludedLabel(MRUKAnchor.SceneLabels label)
    {
        return (excludedLabels & label) != 0;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
#endif
}
