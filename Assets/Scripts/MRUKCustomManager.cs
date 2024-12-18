using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections;

public class MRUKCustomManager : MonoBehaviour
{
    //[HideInInspector] public MRUK _MRUK;
    [HideInInspector] public MRUKRoom _room;
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
}
