using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using TMPro;
using System;

public class SpatialAnchorManager : MonoBehaviour
{
    public OVRSpatialAnchor anchorPrefab;
    public const string uuidPlayerPrefs="numUuids";

    private Canvas canvas;
    private TextMeshProUGUI uuidText;
    private TextMeshProUGUI savedStatusText;
    private List<OVRSpatialAnchor> anchorList=new List<OVRSpatialAnchor>();
    private OVRSpatialAnchor lastCreatedAnchor;
    private AnchorLoader anchorLoader;
    // Start is called before the first frame update
    private void Awake()=> anchorLoader=GetComponent<AnchorLoader>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger,OVRInput.Controller.RTouch))
        {
            createSpatialAnchor();
        }
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            saveLastSpatialAnchor();
        }
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            UnsaveLastSpatialAnchor();
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch))
        {
            loadAllAnchors();
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            unsaveAllAnchors();
        }
    }
   
    public void createSpatialAnchor()
    {
        OVRSpatialAnchor workingAnchor = Instantiate(anchorPrefab, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch));
        canvas=workingAnchor.gameObject.GetComponentInChildren<Canvas>();
        uuidText=canvas.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        savedStatusText= canvas.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        StartCoroutine(createSpatialAnchor(workingAnchor));
    }
    private void saveLastSpatialAnchor()
    {
        lastCreatedAnchor.Save((lastCreatedAnchor, success) =>
        {
            if(success)
            {
                savedStatusText.text = "Saved";
            }
        }
        );
        SaveUuidToPlayerPrefs(lastCreatedAnchor.Uuid);
    }
    void SaveUuidToPlayerPrefs(Guid uuid)
    {
        if(!PlayerPrefs.HasKey(uuidPlayerPrefs))
        {
            PlayerPrefs.SetInt(uuidPlayerPrefs, 0);
        }
        int playerprefuuid= PlayerPrefs.GetInt(uuidPlayerPrefs);
        PlayerPrefs.SetString("uuid" + playerprefuuid, uuid.ToString());
        PlayerPrefs.SetInt(uuidPlayerPrefs, ++playerprefuuid);
    }
    private void UnsaveLastSpatialAnchor()
    {
        lastCreatedAnchor.Erase((lastCreatedAnchor, success) =>
        {
            if (success)
            {
                savedStatusText.text = "Not Saved";
            }
        }
        );
    }
    public void loadAllAnchors()
    {
        anchorLoader.LoadAnchorsByUuid();
    }
    private void unsaveAllAnchors()
    {
        foreach (var anchor in anchorList)
        {
            UnsaveAnchor(anchor);
        }
        anchorList.Clear();
        ClearAllUuidsFromPlayerPrefs();
    }
    private void UnsaveAnchor(OVRSpatialAnchor anchor)
    {
        anchor.Erase((erasedAnchor, success) =>
        {
            if(success)
            {
                var textComponents = erasedAnchor.GetComponentsInChildren<TextMeshProUGUI>();
                if(textComponents.Length>1)
                {
                    var savedStatusText = textComponents[1];
                    savedStatusText.text = "Not Saved";
                }
            }
        });
    }
    private void ClearAllUuidsFromPlayerPrefs()
    {
        if(PlayerPrefs.HasKey(uuidPlayerPrefs))
        {
            int playerprefsNumUuids=PlayerPrefs.GetInt(uuidPlayerPrefs);
            for(int i=0; i<playerprefsNumUuids; i++)
            {
                PlayerPrefs.DeleteKey("uuid"+i);
            }
            PlayerPrefs.DeleteKey(uuidPlayerPrefs);
            PlayerPrefs.Save();
        }
    }
    private IEnumerator createSpatialAnchor(OVRSpatialAnchor anchor)
    {
        while(!anchor.Created && !anchor.Localized)
        {
            yield return null;
        }
        Guid anchorguid = anchor.Uuid;
        anchorList.Add(anchor);
        lastCreatedAnchor = anchor;
        uuidText.text = "UUid:"+anchorguid.ToString();
        savedStatusText.text ="Not Saved" ;
    }
}
