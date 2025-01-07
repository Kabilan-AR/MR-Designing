using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class MRUKCustomManager : MonoBehaviour
{
    //[HideInInspector] public MRUK _MRUK;
    [HideInInspector] public MRUKRoom _room;
   public List<GameObject> _spatialAnchors=new List<GameObject>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!MRUK.Instance.IsInitialized)
        {
            StartCoroutine(WaitForMRUKInitialization());
        }
    }
    private IEnumerator WaitForMRUKInitialization()
    {
        while(!MRUK.Instance.IsInitialized)
        {
            yield return null;
        }
        _room=MRUK.Instance.GetCurrentRoom();
    }
    public void saveAllAnchors()
    {
        foreach (GameObject obj in _spatialAnchors)
        {
            Debug.Log(obj);
            var anchor = obj.GetComponent<OVRSpatialAnchor>();
            if (anchor == null)
            {
                Debug.Log("Anchor added to:" + obj);
                anchor = obj.AddComponent<OVRSpatialAnchor>();
            }

            // Ensure the anchor is localized before saving
            StartCoroutine(waitForAnchorToLocalize(anchor));
        }
    }

    private IEnumerator waitForAnchorToLocalize(OVRSpatialAnchor anchor)
    {
        // Wait until the anchor is localized
        while (!anchor.Created && !anchor.Localized)
        {
            yield return null; 
        }

        // Save the anchor once localized
        anchor.Save((anch, success) =>
        {
            if (success)
            {
                Debug.Log("Anchor Saved: " + anch.gameObject.name + " UUID is: " + anch.Uuid);
            }
            else
            {
                Debug.LogError("Failed to save anchor: " + anch.gameObject.name);
            }
        });
    }

    public void discordAllAnchors()
    {
        foreach (GameObject obj in _spatialAnchors)
        {
            Destroy(obj.GetComponent<OVRSpatialAnchor>());
        }
       
    }
    
}
