using UnityEngine;
using Meta.XR.MRUtilityKit;
public class Toggle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject Spawner;
    private GameObject spawnedObject;
    private WallArtsPlacement wallArts;
    private GameObject spawningObject;
    private MRUKCustomManager _MRUKManager;
    private MRUKRoom room;

    [HideInInspector] public string uuidKey;
    void Start()
    {
        Spawner = FindFirstObjectByType<Spawner>().gameObject;
        _MRUKManager = FindFirstObjectByType<MRUKCustomManager>();
        wallArts=FindFirstObjectByType<WallArtsPlacement>();
        Debug.Log("Spawner is:" + Spawner.name);
        spawningObject = gameObject;
    }

    void Update()
    {
        if(room==null)
        {
            if(MRUK.Instance.IsInitialized)
            {
                room = MRUK.Instance.GetCurrentRoom();
            }
        }
    }

    public void toggle()
    {
        gameObject.SetActive(true);
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(gameObject, Spawner.transform.position,Quaternion.identity);
            spawnedObject.SetActive(true);
            //CreateSpatialAnchor();


            //Debug.Log("Spawn Loop Starts here:");
            //foreach(var obj in _MRUKManager._spatialAnchors)
            //{
            //    Debug.Log("inside loop:" + obj.name);
            //}
            //Debug.Log("Spawn Loop Ends here.");

        }

        else
        {
            Debug.LogError("Destroyed");
           // DestroySpatialAnchor();
            Destroy(spawnedObject);
            spawnedObject = null;



            //Debug.Log("Destroy Loop Starts here:");
            //foreach (var obj in _MRUKManager._spatialAnchors)
            //{
            //    if (_MRUKManager._spatialAnchors.Count == 0) Debug.Log("nulllllll");
            //    Debug.Log("inside loop:"+obj.name);
            //}
            //Debug.Log("Destroy Loop Ends here.");
        }
        gameObject.SetActive(false);

    }
    private void CreateSpatialAnchor()
    {
        spawnedObject.AddComponent<OVRSpatialAnchor>();
        _MRUKManager._spatialAnchors.Add(spawnedObject.GetComponent<OVRSpatialAnchor>());
    }
    private void DestroySpatialAnchor()
    {
        _MRUKManager._spatialAnchors.Remove(spawnedObject.GetComponent<OVRSpatialAnchor>());
        Destroy(spawnedObject.GetComponent<OVRSpatialAnchor>());
    }
    public void WallArtsToggle()
    {
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(gameObject, Spawner.transform.position, Quaternion.identity);
            spawnedObject.SetActive(true);
            wallArts.objectToPlace = spawnedObject;
        }
        else
        {
            Destroy(spawnedObject);
            wallArts.objectToPlace = null;
        }
        //if (room != null)
        //{
        //    MRUKAnchor closestKeyWall = room.GetKeyWall(out Vector2 surface,0.5f);
        //    gameObject.transform.position = new Vector3(surface.x, surface.y, gameObject.transform.position.z);
        //    gameObject.transform.rotation= Quaternion.LookRotation(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z));
        //}
    }
}
