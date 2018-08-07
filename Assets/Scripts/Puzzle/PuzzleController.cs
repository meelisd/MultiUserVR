using UnityEngine;
using UnityEngine.Networking;

public class PuzzleController : NetworkBehaviour {
  public GameObject PuzzlePrefab;
  
  public override void OnStartServer() {
    var puzzleInstance = Instantiate(PuzzlePrefab, transform.position, transform.rotation);
    NetworkServer.Spawn(puzzleInstance);
  }
}