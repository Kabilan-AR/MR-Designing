using UnityEngine;
using Meta.XR.MRUtilityKit;
public class WallArtsPlacement : MonoBehaviour
{
    //public GameObject _Object;
    public Transform rayStartTransform;
    public MRUKAnchor.SceneLabels labelFilters;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(rayStartTransform.position, Vector3.forward);
        if(MRUK.Instance.IsInitialized)
        {
            MRUKRoom currentRoom = MRUK.Instance.GetCurrentRoom();

            if (currentRoom != null)
            {
                bool hasHit = currentRoom.Raycast(ray, 10, new LabelFilter(labelFilters), out RaycastHit hit, out MRUKAnchor anchor);

                if (hasHit)
                {
                    Vector3 HitPoint = hit.point;
                    Vector3 hitRotation = hit.normal;

                    gameObject.transform.position = HitPoint;
                    gameObject.transform.rotation = Quaternion.LookRotation(hitRotation);
                }
                else
                {
                    Debug.Log("Is not hit");
                }

            }
            else
            {
                Debug.LogError("Room is Null");
            }
        }
        else
        {
            Debug.LogError("MRUK is not intiaizlized");
        }
        
    }
}
