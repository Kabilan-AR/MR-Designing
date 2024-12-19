using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.HandGrab;
using Meta.XR.MRUtilityKit;

public class PrefabHoverAndSpawn : MonoBehaviour
{
    
    public LayerMask furnitureLayerMask=7;  
    public MRUKAnchor.SceneLabels roomLabelsToAvoid = MRUKAnchor.SceneLabels.TABLE | MRUKAnchor.SceneLabels.COUCH;  
    public float objectOffsetFromFloor = 0.1f;
    public Collider objectCollider;
    public OVRHand hand;  
    private bool isPlacing = false;
    private MRUKRoom room;
    private Vector3 placementPosition;
    private Quaternion placementRotation;

    void Start()
    {
        // Initialize hand tracking
        //hand = FindFirstObjectByType<OVRHand>();

    }

    void Update()
    {
        if(MRUK.Instance.IsInitialized)
        {
            room = MRUK.Instance.GetCurrentRoom();
            if (isPlacing)
            {
                Ray handRay = new Ray(hand.PointerPose.position, Vector3.down);
                if (Physics.Raycast(handRay, out RaycastHit hit, Mathf.Infinity))
                {
                    // Get position and rotation for placing the object
                    placementPosition = hit.point + Vector3.up * objectOffsetFromFloor;
                    placementRotation = Quaternion.LookRotation(hit.normal);

                    if (!IsCollidingWithOtherFurniture() && !IsCollidingWithRoomObjects(placementPosition))
                    {
                        transform.position = placementPosition;
                        transform.rotation = placementRotation;
                    }
                    else
                    {
                        Debug.Log("Placement position collides with another object. Trying again...");
                    }
                }
            }
            if (hand.GetFingerIsPinching(OVRHand.HandFinger.Index) && isPlacing)
            {
                PlaceObject();
            }
        }
        
    }

    // Begin placing the object
    public void StartPlacing()
    {
        isPlacing = true;
    }

    private void PlaceObject()
    {
        if (!IsCollidingWithOtherFurniture() && !IsCollidingWithRoomObjects(placementPosition))
        {
            Debug.Log("Object placed successfully.");
            isPlacing = false;

        }
    }

    private bool IsCollidingWithOtherFurniture()
    {
        Collider[] colliders = Physics.OverlapBox(objectCollider.bounds.center, objectCollider.bounds.extents, transform.rotation, furnitureLayerMask);
        return colliders.Length > 0;
    }

    private bool IsCollidingWithRoomObjects(Vector3 position)
    {
        Ray ray = new Ray(position + Vector3.up * 0.5f, Vector3.down);
        bool hasHit = room.Raycast(ray, 10f, new LabelFilter(roomLabelsToAvoid), out RaycastHit hit, out MRUKAnchor anchor);
        return hasHit;
    }


}
