using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using NUnit.Framework;

[RequireComponent(typeof(LineRenderer))]
public class WallArtsPlacement : MonoBehaviour
{
    public Dictionary<int,List<OVRSpatialAnchor>> spatialAnchorDic;
    public OVRHand _hand;
    public MRUKCustomManager _mrukManager;
    public Transform handIndexFingerTip;
    public GameObject objectToPlace;
    public MRUKAnchor.SceneLabels labelFilters = MRUKAnchor.SceneLabels.WALL_FACE;
    private LineRenderer lineRenderer;
    private GameObject _panelGlow;

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

       
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
        }
        lineRenderer.enabled = false;

    }

    void Update()
    {
        if(objectToPlace != null)
        {
            _panelGlow = objectToPlace.transform.Find("_panelGlow").gameObject;
        }
        else
        {
            lineRenderer.enabled = false;
        }
        Ray ray = new Ray(handIndexFingerTip.position, handIndexFingerTip.forward);

        if (_mrukManager._room != null)
        {
            lineRenderer.enabled = true;
            bool hasHit = _mrukManager._room.Raycast(ray, 10, new LabelFilter(labelFilters), out RaycastHit hit, out MRUKAnchor anchor);

            if (hasHit && objectToPlace != null)
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
                    _panelGlow.SetActive(true);
                    isPinching = true;
                }

                else
                {
                    _panelGlow.SetActive(false);
                    isPinching = false;
                }
            }
        }

    }
    private void ArtPositioning()
    {
        bool didHitPanel = Physics.Raycast(GetRaycastRay(), out var hit) && (hit.transform.gameObject.layer == LayerMask.NameToLayer("Wall_Arts"));


        _panelGlow.SetActive(didHitPanel);
        objectToPlace.transform.position = Vector3.Lerp(objectToPlace.transform.position, targetPosition, Time.deltaTime * positionLerpSpeed);
        objectToPlace.transform.rotation = Quaternion.Slerp(objectToPlace.transform.rotation, targetRotation, Time.deltaTime * rotationLerpSpeed);
    }
    private Ray GetRaycastRay()
    {
        return new Ray(handIndexFingerTip.position, handIndexFingerTip.forward);
    }
}