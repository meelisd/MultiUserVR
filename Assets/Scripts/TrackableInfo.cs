using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TrackableInfo : MonoBehaviour
{
    public GameObject Root;
    public GameObject Head;
    public GameObject LeftHand;
    public GameObject RightHand;

    public Trackable[] GetTrackables()
    {
        if (!Head || !LeftHand || !RightHand) {
            return new Trackable[0];
        }
        var rootRot = Root.transform.rotation;
        var trackables = new Trackable[4] { 
            new Trackable(Root.transform.position, Quaternion.Euler(0, rootRot.y, 0)),
            new Trackable(Root.transform.InverseTransformPoint(Head.transform.position), Head.transform.rotation),
            new Trackable(Root.transform.InverseTransformPoint(LeftHand.transform.position), LeftHand.transform.rotation),
            new Trackable(Root.transform.InverseTransformPoint(RightHand.transform.position), RightHand.transform.rotation)
        };

        return trackables;
    }

    public void SetTrackables(Trackable[] trackables) {
        Root.transform.SetPositionAndRotation(trackables[0].Position, trackables[0].Rotation);
        Head.transform.SetPositionAndRotation(trackables[1].Position, trackables[1].Rotation);
        LeftHand.transform.SetPositionAndRotation(trackables[2].Position, trackables[2].Rotation);
        RightHand.transform.SetPositionAndRotation(trackables[3].Position, trackables[3].Rotation);
    }

    public bool IsConfigured {
        get {
            return Root && Head && LeftHand && RightHand;
        }
    }
}