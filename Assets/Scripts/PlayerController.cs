using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	public GameObject ServerPrefab;
	public GameObject LocalPrefab;
	public GameObject RemotePrefab;

	protected GameObject Instance;
	private TrackableInfo MyTrackables;

	public TrackableSyncList SyncedTrackables = new TrackableSyncList();

	// Use this for initialization
	void Start () {
		if (isLocalPlayer && isServer) {
			Instance = Instantiate(ServerPrefab, transform.position, transform.rotation, transform);
			MyTrackables = Instance.GetComponent<TrackableInfo>();
			Camera.main.gameObject.SetActive(false);
		} else if (isLocalPlayer) {
			Instance = Instantiate(LocalPrefab, transform.position, transform.rotation, transform);
			MyTrackables = Instance.GetComponent<TrackableInfo>();

			if (Camera.main != null) {
				Camera.main.transform.SetParent(MyTrackables.Head.transform, false);
				Camera.main.transform.localPosition = Vector3.zero;
				Camera.main.transform.localRotation = Quaternion.identity;
			}

		} else {
			Instance = Instantiate(RemotePrefab, transform.position, transform.rotation, transform);
			MyTrackables = Instance.GetComponent<TrackableInfo>();
			SyncedTrackables.Callback += OnChangeTrackables;
		}

		if (isServer) {
			SyncedTrackables.Add(new Trackable());
			SyncedTrackables.Add(new Trackable());
			SyncedTrackables.Add(new Trackable());
			SyncedTrackables.Add(new Trackable());
		}
	}

    public void SyncMyTrackables() {
		if (MyTrackables) {
			SyncTrackables(MyTrackables.GetTrackables());
		}
	}

	protected void SyncTrackables(Trackable[] trackables) {
		if (isServer) {
			SyncedTrackables[0] = trackables[0];
			SyncedTrackables[1] = trackables[1];
			SyncedTrackables[2] = trackables[2];
			SyncedTrackables[3] = trackables[3];
		} else {
			Debug.Log("send: " + trackables[0].Position + " - " + trackables[0].Rotation);
			CmdSyncTrackables(trackables);
		}
	}

	[Command]
	protected void CmdSyncTrackables(Trackable[] trackables) {
		SyncTrackables(trackables);	
	}

	protected void OnChangeTrackables(SyncList<Trackable>.Operation op, int itemIndex) {
		if (isLocalPlayer) {
			return;
		}
		
		
	}
	
	void Update () {
		if (isLocalPlayer) {
			SyncMyTrackables();
		} else {
			if (MyTrackables && MyTrackables.IsConfigured) {
				var trackables = SyncedTrackables;
				MyTrackables.Root.transform.localPosition = trackables[0].Position;
				MyTrackables.Root.transform.localRotation = trackables[0].Rotation;

				MyTrackables.Head.transform.localPosition = trackables[1].Position;
				MyTrackables.Head.transform.localRotation = trackables[1].Rotation;
				
				MyTrackables.LeftHand.transform.localPosition = trackables[2].Position;
				MyTrackables.LeftHand.transform.localRotation = trackables[2].Rotation;

				MyTrackables.RightHand.transform.localPosition = trackables[3].Position;
				MyTrackables.RightHand.transform.localRotation = trackables[3].Rotation;
			}
		}
	}

	public override void OnStartLocalPlayer() {
	}
}
