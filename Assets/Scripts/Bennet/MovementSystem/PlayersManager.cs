using System;
using UnityEngine;

namespace Bennet.MovementSystem
{
    public class PlayersManager : MonoBehaviour
    {
        public PlayerMovementController p1, p2;

        private void FixedUpdate()
        {
            float horizontalP1ToP2 = Vector3.Dot(p1.body.transform.position - p2.body.transform.position,
                p1.body.GetForwardDirection());
            p1.body.FacingRight = horizontalP1ToP2 >= 0;
            p2.body.FacingRight = horizontalP1ToP2 < 0;
        }
    }
}