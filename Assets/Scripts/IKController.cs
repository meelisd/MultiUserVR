using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKController : MonoBehaviour {
	private Animator _animator;

	public bool ikActive = false;

	public Transform LookAt;
	public Transform LeftHand;
	public Transform RightHand;

	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
	}
	
	void OnAnimatorIK() {
		if (_animator != null) {
			if (ikActive) {
				if (LookAt != null) {
					_animator.SetLookAtWeight(1);
					_animator.SetLookAtPosition(LookAt.position);
				}

				if (LeftHand != null) {
					_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
					_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
					_animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHand.position);
					_animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHand.rotation);
				}

				if (RightHand != null) {
					_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
					_animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
					_animator.SetIKPosition(AvatarIKGoal.RightHand, RightHand.position);
					_animator.SetIKRotation(AvatarIKGoal.RightHand, RightHand.rotation);
				}
			} else {          
                _animator.SetLookAtWeight(0);
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,0);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand,0); 
            }
		}
	}
}
