using UnityEngine;
using UnityEngine.Networking;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class PuzzlePart : MonoBehaviour {
  private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & ( ~Hand.AttachmentFlags.SnapOnAttach ) & ( ~Hand.AttachmentFlags.DetachOthers );
  private Puzzle _puzzle;

  public void Start() {
    _puzzle = transform.parent.GetComponent<Puzzle>();
  }

  public void SetTrackable(Trackable trackable) {
    this.transform.SetPositionAndRotation(trackable.Position, trackable.Rotation);
  }
  public Trackable GetTrackable() {
    return new Trackable(transform);
  }

  Hand _hand;

    private void HandHoverUpdate( Hand hand )
		{
      if ( hand.GetStandardInteractionButtonDown() || ( ( hand.controller != null ) && hand.controller.GetPressDown( Valve.VR.EVRButtonId.k_EButton_Grip ) ) ) {
        if ( _hand == null )
        {
          _hand = hand;
          _puzzle.TrackPuzzlePart(this, hand);
        }
        else
        {
          _hand = null;
          _puzzle.ReleasePuzzlePart(this, hand);
        }
      }
    }
}