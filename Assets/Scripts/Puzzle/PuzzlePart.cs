using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(MeshCollider))]
public class PuzzlePart : MonoBehaviour {

  public void SetTrackable(Trackable trackable) {
    this.transform.SetPositionAndRotation(trackable.Position, trackable.Rotation);
  }
  public Trackable GetTrackable() {
    return new Trackable(transform);
  }
}