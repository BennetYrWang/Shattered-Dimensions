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
            _movementController.illusion.onFallingBegin += OnIllusionStatFalling;
        }

        private void OnDestroy()
        {
            _movementController.onMoveStart -= OnPlayerMoveStart;
            _movementController.onMoveStop -= OnPlayerMoveStop;
            _movementController.body.onFallingBegin -= OnBodyStartFalling;
            _movementController.illusion.onFallingBegin -= OnIllusionStatFalling;
        }

        void OnBodyStartFalling()
        {
            
        }

        void OnIllusionStatFalling()
        {
            
        }

        void OnPlayerMoveStart()
        {
            
        }

        void OnPlayerMoveStop()
        {
            
        }
    }
}