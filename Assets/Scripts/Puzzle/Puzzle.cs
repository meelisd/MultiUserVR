using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR.InteractionSystem;

public class Puzzle : NetworkBehaviour {
  PuzzlePart[] _parts;
  int[] _partClaims;
  TrackableSyncList _partTrackables = new TrackableSyncList();
  Dictionary<int, Hand> trackedParts = new Dictionary<int, Hand>();

  PlayerController _localPlayer;
  PlayerController LocalPlayer {
    get {
      if (_localPlayer == null) {
        _localPlayer = GameObject.FindGameObjectsWithTag("Player").Select(pc => pc.GetComponent<PlayerController>()).Where(pc => pc.isLocalPlayer).FirstOrDefault();
      }

      return _localPlayer;
    }
  }

  public float ShuffleRadius = 1;
  
  public override void OnStartServer() {
    if (_parts == null) {
      _parts = GetComponentsInChildren<PuzzlePart>();
      _partClaims = Enumerable.Range(0, _parts.Length).Select(n=>-1).ToArray();
      
      foreach (var part in _parts) {
        _partTrackables.Add(new Trackable(part.transform));
      }

      Shuffle();
    }
  }

  public override void OnStartClient() {
    _parts = GetComponentsInChildren<PuzzlePart>();
  }

  public void Start() {
  }

  public void Shuffle() {
    if (isServer) {
      var circle = Mathf.Deg2Rad * 360;
      var intervalRads = circle / _parts.Count();

      for(var i = 0; i < _parts.Count(); i++) {
        var shuffeledPosition = new Vector3(Mathf.Sin(i * intervalRads), transform.position.y, Mathf.Cos(i * intervalRads));
        _parts[i].transform.localPosition = shuffeledPosition;
      }
    }
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
      Debug.LogWarning("Trackables should be the same length as parts " + _partTrackables.Count + "/" + _parts.Length + ", skip sync");
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

          if (LocalPlayer != null && trackedParts.ContainsKey(i)) {
            var hand = trackedParts[i];
            UpdatePartPositionAndRotation(i, hand.transform.position, hand.transform.rotation);
          }
        }
			}
		}
	}

  public void UpdatePartPositionAndRotation(int partIndex, Vector3 position, Quaternion rotation) {
    if (isServer) {
      _parts[partIndex].transform.SetPositionAndRotation(position, rotation);
    } else {
      LocalPlayer.UpdatePartPositionAndRotation(partIndex, position, rotation);
    }
  }
  
  public void TrackPuzzlePart(PuzzlePart part, Hand hand) {
    var partIndex = Array.IndexOf(_parts, part);
    trackedParts[partIndex] = hand; 
  }

  public void ReleasePuzzlePart(PuzzlePart part, Hand hand) {
    var partIndex = Array.IndexOf(_parts, part);
    trackedParts.Remove(partIndex);
  }
}