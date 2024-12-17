using UnityEngine;

public class PrefabHoverandSpawn : MonoBehaviour
{
    public GameObject _Object;         // Object prefab to place
    public OVRHand rightHand;          // Reference to the right hand (set in inspector)

    private GameObject _PreviewObject; // Preview object to display during placement
    private bool isHoldingObject = false; // Track if we're holding an object

    void Start()
    {
        // Instantiate the preview object (inactive until grabbed)
        _PreviewObject = Instantiate(_Object);
        _PreviewObject.GetComponent<Renderer>().material.color = Color.green; // Preview color
        _PreviewObject.SetActive(false);  // Initially hidden
    }

    void Update()
    {
        // Detect if right hand is pinching (index finger)
        bool isPinching = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

        // If we are pinching (grabbing) and not yet holding an object
        if (isPinching && !isHoldingObject)
        {
            isHoldingObject = true;
            _PreviewObject.SetActive(true); // Show the preview object
        }
        // If we release the pinch and are holding an object
        else if (!isPinching && isHoldingObject)
        {
            // Place the object in the world at the preview's current position and rotation
            Instantiate(_Object, _PreviewObject.transform.position, _PreviewObject.transform.rotation);
            isHoldingObject = false;
            _PreviewObject.SetActive(false); // Hide preview after placing
        }

        // If currently holding an object, move it to the right hand's position
        if (isHoldingObject)
        {
            MovePreviewWithHand();
        }
    }

    private void MovePreviewWithHand()
    {
        // Move the preview object to follow the right hand's position and rotation
        _PreviewObject.transform.position = rightHand.transform.position;
        _PreviewObject.transform.rotation = rightHand.transform.rotation;
    }
}
