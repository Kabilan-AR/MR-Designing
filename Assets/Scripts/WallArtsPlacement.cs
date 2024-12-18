using UnityEngine;
using System.Collections;
using Meta.XR.MRUtilityKit;
[RequireComponent(typeof(LineRenderer))]
public class WallArtsPlacement : MonoBehaviour
{
    public OVRHand _hand;
    public MRUKCustomManager _mrukManager;
    public Transform handIndexFingerTip; 
    public GameObject objectToPlace;
    public MRUKAnchor.SceneLabels labelFilters = MRUKAnchor.SceneLabels.WALL_FACE;
    private LineRenderer lineRenderer;     
   

    //private MRUKRoom room;
    private bool isPinching = false;
    //Pinch values
    private float positionLerpSpeed = 5f;  
    private float rotationLerpSpeed = 5f;
    private float pinchHoldTime = 0.1f;  
    private float pinchTimer = 0f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    void Start()
    {
        if (_mrukManager == null)
        {
            _mrukManager = FindFirstObjectByType<MRUKCustomManager>();
        }


        lineRenderer=GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
        }
       
    }

    void Update()
    {
        // Ray from the index finger
        Ray ray = new Ray(handIndexFingerTip.position, handIndexFingerTip.forward);

        
        

        // Ensure MRUK Room is not null
        if (_mrukManager._room != null)
        {
            bool hasHit = _mrukManager._room.Raycast(ray, 10, new LabelFilter(labelFilters), out RaycastHit hit, out MRUKAnchor anchor);
            
            if (hasHit)
            {
                if (lineRenderer != null)
                {
                    lineRenderer.SetPosition(0, ray.origin);    
                    lineRenderer.SetPosition(1, hit.point);    
                }
                targetPosition = hit.point;
                targetRotation = Quaternion.LookRotation(hit.normal);
                
                if (isPinching)
                {
                    ArtPositioning();
                }
                if ((_hand != null && _hand.GetFingerIsPinching(OVRHand.HandFinger.Index)))
                {
                    isPinching = true;  // Start positioning the object
                }
                
                else
                {
                    isPinching=false;
                }
            }
        }

    }
    private void ArtPositioning()
    {

        objectToPlace.transform.position = Vector3.Lerp(objectToPlace.transform.position, targetPosition, Time.deltaTime * positionLerpSpeed);
        objectToPlace.transform.rotation = Quaternion.Slerp(objectToPlace.transform.rotation, targetRotation, Time.deltaTime * rotationLerpSpeed);
    }
}
