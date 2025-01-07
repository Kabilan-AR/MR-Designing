using UnityEngine;
using Meta.XR.MRUtilityKit;
public class Toggle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject Spawner;
    [HideInInspector] public GameObject spawnedObject;
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
            _MRUKManager._spatialAnchors.Add(spawnedObject);
        }

        else
        {
            Debug.LogError("Destroyed");
            _MRUKManager._spatialAnchors.Remove(spawnedObject);
            Destroy(spawnedObject);
            spawnedObject = null;
        }
        gameObject.SetActive(false);

    }
   
    public void WallArtsToggle()
    {
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(gameObject, Spawner.transform.position, Quaternion.identity);
            spawnedObject.SetActive(true);
            _MRUKManager._spatialAnchors.Add( spawnedObject);
            wallArts.objectToPlace = spawnedObject;
        }
        else
        {
            _MRUKManager._spatialAnchors.Remove(spawnedObject);
            Destroy(spawnedObject);
            wallArts.objectToPlace = null;
        }
       
    }
}
