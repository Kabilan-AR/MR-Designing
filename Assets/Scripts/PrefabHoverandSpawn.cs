using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.HandGrab;
using Meta.XR.MRUtilityKit;

public class PrefabHoverAndSpawn : MonoBehaviour
{

    private void Start()
    {
        Rigidbody objectRigidbody = GetComponent<Rigidbody>();

        if (objectRigidbody != null)
        {
            objectRigidbody.interpolation = RigidbodyInterpolation.Interpolate; 
            objectRigidbody.linearDamping = 5f; 
            objectRigidbody.angularDamping = 5f;
            objectRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous; 

            objectRigidbody.solverIterations = 10;
            objectRigidbody.solverVelocityIterations = 10;
        }

    }
}
