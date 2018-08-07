using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Puzzle : NetworkBehaviour {
  PuzzlePart[] _parts;
  int[] _partClaims;
  TrackableSyncList _partTrackables = new TrackableSyncList();

  public override void OnStartServer() {
    if (_parts == null) {
      _parts = GetComponentsInChildren<PuzzlePart>();
      _partClaims = Enumerable.Range(0, _parts.Length).Select(n=>-1).ToArray();
      
      foreach (var part in _parts) {
        _partTrackables.Add(new Trackable(part.transform));
      }
    }
  }

  public override void OnStartClient() {
    _parts = GetComponentsInChildren<PuzzlePart>();
  }

  public void Start() {
  }

  public void Shuffle() {

  }
  public void SyncMyTrackables() {
    var trackables = _parts.Select(p => p.GetTrackable());
    SyncTrackables(trackables.ToArray());
  }

  public void SyncTrackables(Trackable[] trackables) {
    if (!isServer) {
      return;
    }

    if (trackables.Length != _parts.Length) {
      Debug.LogWarning("Trackables should be the same length as parts, skip sync");
      return;
    }

    for(var i = 0; i < _parts.Length; i++) {
      _partTrackables[i] = trackables[i];
    }
  }

  void Update () {
    if (isServer) {
			SyncMyTrackables();
		}
    else {
			if (_parts.Length > 0) {
				if (_partTrackables.Count != _parts.Length) {
          Debug.LogWarning("Trackables should be the same length as parts " + _partTrackables.Count + "/" + _parts.Length + ", skip sync");
          return;
        }

        for(var i = 0; i < _parts.Length; i++) {
          _parts[i].SetTrackable(_partTrackables[i]);
        }
			}
		}
	}

  public void ClaimPart(int playerId, int partIndex) {
    if (!isServer) {
      return;
    }

    if (_partClaims[partIndex] != -1) {
      return;
    }

    _partClaims[partIndex] = playerId;
  }

  public void ReleasePart(int playerId, int partIndex) {
    if (!isServer) {
      return;
    }

    if (_partClaims[partIndex] != playerId) {
      return;
    }

    _partClaims[partIndex] = -1;
  }

}