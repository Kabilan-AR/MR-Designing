using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Meta.XR.MRUtilityKit;
public class AnchorLoader : MonoBehaviour
{
    private OVRSpatialAnchor anchorPrefab;
    private SpatialAnchorManager spatialAnchorManager;
    // Start is called before the first frame update
    Action<OVRSpatialAnchor.UnboundAnchor, bool> onLoadAnchor;

    private void Awake()
    {
        spatialAnchorManager = GetComponent<SpatialAnchorManager>();
        anchorPrefab = spatialAnchorManager.anchorPrefab;
        onLoadAnchor = OnLocalized;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadAnchorsByUuid()
    {
        if(!PlayerPrefs.HasKey(SpatialAnchorManager.uuidPlayerPrefs))
        {
            PlayerPrefs.SetInt(SpatialAnchorManager.uuidPlayerPrefs, 0);
        }
        int playerUUidCount = PlayerPrefs.GetInt(SpatialAnchorManager.uuidPlayerPrefs);
        if (playerUUidCount == 0)
        {
            return;
        }
            var uuids = new Guid[playerUUidCount];
            for(int i=0; i<playerUUidCount; i++)
            {
                var uuidKey = "uuid" + i;
                var currentUuid=PlayerPrefs.GetString(uuidKey);
                uuids[i] = new Guid(currentUuid);
            }
            Load(new OVRSpatialAnchor.LoadOptions
            {
                Timeout = 0,
                StorageLocation=OVRSpace.StorageLocation.Local,
                Uuids = uuids
            }) ;

        
    }
    private void Load(OVRSpatialAnchor.LoadOptions options)
    {
        OVRSpatialAnchor.LoadUnboundAnchors(options, anchors =>
        {
            if (anchors == null)
            {
                return;
            }
            foreach (var anc in anchors)
            {
                if (anc.Localized)
                {
                    onLoadAnchor(anc, true);
                }
                else if (!anc.Localizing)
                {
                    anc.Localize(onLoadAnchor);
                }
            }
        });
    }
    private void OnLocalized(OVRSpatialAnchor.UnboundAnchor unboundAnchor, bool success)
    {
        if(!success)
        {
            return;
        }
        var Pose = unboundAnchor.Pose;
        var spatialAnchor=Instantiate(anchorPrefab,Pose.position,Pose.rotation);
        unboundAnchor.BindTo(spatialAnchor);
        if(spatialAnchor.TryGetComponent<OVRSpatialAnchor>(out var anchor))
        {
            var uuidText = spatialAnchor.GetComponentInChildren<TextMeshProUGUI>();
            var savedStatusText = spatialAnchor.GetComponentsInChildren<TextMeshProUGUI>()[1];
            uuidText.text = "UUID:" + spatialAnchor.Uuid.ToString();
            savedStatusText.text = "Loaded from device";
        }
        
    }
}
