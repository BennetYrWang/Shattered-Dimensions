using System;
using UnityEngine;

namespace BennetWang.MovementSystem
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator bodyAnimator, illusionAnimator;
        [SerializeField] private PlayerMovementController _movementController;
        private void Start()
        {
            _movementController.onMoveStart += OnPlayerMoveStart;
            _movementController.onMoveStop += OnPlayerMoveStop;
            _movementController.body.onFallingBegin += OnBodyStartFalling;
            _movementController.illusion.onFallingBegin += OnIllusionStartFalling;
            _movementController.body.onLanding += OnBodyLanding;
            _movementController.illusion.onLanding += OnIllusionLanding;
        }

        private void OnDestroy()
        {
            _movementController.onMoveStart -= OnPlayerMoveStart;
            _movementController.onMoveStop -= OnPlayerMoveStop;
            _movementController.body.onFallingBegin -= OnBodyStartFalling;
            _movementController.illusion.onFallingBegin -= OnIllusionStartFalling;
            _movementController.body.onLanding -= OnBodyLanding;
            _movementController.illusion.onLanding -= OnIllusionLanding;
        }

        void OnBodyStartFalling()
        {
            bodyAnimator.SetBool("inAir", true);
        }

        void OnBodyLanding()
        {
            bodyAnimator.SetBool("inAir", false);
            SoundManager.Instance.PlaySoundEffect(SoundManager.Clip.JumpLanding);
        }

        void OnIllusionStartFalling()
        {
            illusionAnimator.SetBool("inAir", true);
        }

        void OnIllusionLanding()
        {
            illusionAnimator.SetBool("inAir", false);
        }

        void OnPlayerMoveStart()
        {
            bodyAnimator.SetBool("isMoving", true);
            illusionAnimator.SetBool("isMoving", true);
        }

        void OnPlayerMoveStop()
        {
            bodyAnimator.SetBool("isMoving", false);
            illusionAnimator.SetBool("isMoving", false);
        }
    }
}