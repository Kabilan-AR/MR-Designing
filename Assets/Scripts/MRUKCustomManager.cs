using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class MRUKCustomManager : MonoBehaviour
{
    //[HideInInspector] public MRUK _MRUK;
    [HideInInspector] public MRUKRoom _room;
    [HideInInspector] public List<OVRSpatialAnchor> _spatialAnchors=new List<OVRSpatialAnchor>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!MRUK.Instance.IsInitialized)
        {
            StartCoroutine(WaitforMRUKInitialization());
        }
    }
    private IEnumerator WaitforMRUKInitialization()
    {
        while(!MRUK.Instance.IsInitialized)
        {
            yield return null;
        }
        _room=MRUK.Instance.GetCurrentRoom();
    }
    public void saveAllAnchors()
    {
        foreach(var anchors in _spatialAnchors)
        {
            StartCoroutine(waitForAnchorToLocalize(anchors));
            anchors.Save((anchors, success) =>
            {
                if (success)
                {
                    Debug.Log("Anchor Saved:" + anchors.gameObject.name + "UUID is:" + anchors.Uuid);
                }
            });
        }
    }
    private IEnumerator waitForAnchorToLocalize(OVRSpatialAnchor anchor)
    {
        while (!anchor.Created && !anchor.Localized)
        {
            yield return new WaitForEndOfFrame();
        }

    }
}
