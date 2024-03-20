using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject[] ObjectToPlace;
    public GameObject placementIndicator;
    private ARSessionOrigin arOrgin;
    private Pose placementPose;
    bool placementPoseIsValid = false;
    int objectIndex = 0;
    bool click = false;
    public void onPointerDown()
    {
        if(!click && placementPoseIsValid)
        {
            click = true;
        }

    }
    public void onPointerUp()
    {
        if(click)
        {
            click = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        arOrgin = FindObjectOfType<ARSessionOrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        if (placementPoseIsValid && click)
        {
            PlaceObjects();
            click = false;
        }
    }

    private void PlaceObjects()
    {
        GameObject g = Instantiate(ObjectToPlace[objectIndex],placementPose.position,placementPose.rotation);
        Destroy(g, 10f);
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCentre = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hit = new List<ARRaycastHit>();
        arOrgin.GetComponent<ARRaycastManager>().Raycast(screenCentre, hit, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
        placementPoseIsValid = hit.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hit[0].pose;
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
    public void SelectObej(int i)
    {
        objectIndex = i;
    }
}
