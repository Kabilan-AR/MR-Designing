using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPlacement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject anchorPrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            createSpatialAnchor();

        }
    }
    private void createSpatialAnchor()
    {
        GameObject prefab=Instantiate(anchorPrefab, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch));
       prefab.AddComponent<OVRSpatialAnchor>();
    }
}
