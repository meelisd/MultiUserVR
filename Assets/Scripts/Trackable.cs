using UnityEngine;
using UnityEngine.Networking;

public struct Trackable {
    public Vector3 Position;
    public Quaternion Rotation;

    public void LoadFromTransform(Transform transform) {
        Position = transform.position;
        Rotation = transform.rotation;
    }

    public Trackable(Transform transform) {
        Position = transform.position;
        Rotation = transform.rotation;
    }

    public Trackable(Vector3 position, Quaternion rotation) {
        Position = position;
        Rotation = rotation;
    }
}

public class TrackableSyncList : SyncListStruct<Trackable> {
}