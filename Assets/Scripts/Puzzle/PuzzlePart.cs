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
  
		//-------------------------------------------------
		// Called when this GameObject becomes attached to the hand
		//-------------------------------------------------
		private void OnAttachedToHand( Hand hand )
		{
			Debug.Log("Attached to hand " + hand.name);
		}


		//-------------------------------------------------
		// Called when this GameObject is detached from the hand
		//-------------------------------------------------
		private void OnDetachedFromHand( Hand hand )
		{
			Debug.Log("Detached from hand " + hand.name);
		}


    
		//-------------------------------------------------
		// Called when a Hand starts hovering over this object
		//-------------------------------------------------
		private void OnHandHoverBegin( Hand hand )
		{
			Debug.Log("Hovering hand: " + hand.name);
		}


		//-------------------------------------------------
		// Called when a Hand stops hovering over this object
		//-------------------------------------------------
		private void OnHandHoverEnd( Hand hand )
		{
			Debug.Log("No Hand Hovering");
		}

    private void HandHoverUpdate( Hand hand )
		{
      if ( hand.GetStandardInteractionButtonDown() || ( ( hand.controller != null ) && hand.controller.GetPressDown( Valve.VR.EVRButtonId.k_EButton_Grip ) ) ) {
        if ( _hand == null )
        {
          _hand = hand;
          Debug.Log("Start tracking puzzle part");
          _puzzle.TrackPuzzlePart(this, hand);

          // // Call this to continue receiving HandHoverUpdate messages,
          // // and prevent the hand from hovering over anything else
          // hand.HoverLock( GetComponent<Interactable>() );

          // // Attach this object to the hand
          // hand.AttachObject( gameObject, attachmentFlags );
        }
        else
        {
          _hand = null;
          Debug.Log("Release puzzle part");
          _puzzle.ReleasePuzzlePart(this, hand);
          // // Detach this object from the hand
          // hand.DetachObject( gameObject );

          // // Call this to undo HoverLock
          // hand.HoverUnlock( GetComponent<Interactable>() );

        }
      }
    }

}