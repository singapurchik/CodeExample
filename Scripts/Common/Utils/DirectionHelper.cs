using UnityEngine;

namespace FAS
{
    public enum HitDirection
    {
        Front,
        FrontRight,
        Right,
        BackRight,
        Back,
        BackLeft,
        Left,
        FrontLeft
    }

    public static class DirectionHelper
    {
        private const float FRONT_MIN = -22.5f;
        private const float FRONT_MAX = 22.5f;

        private const float FRONT_RIGHT_MIN = 22.5f;
        private const float FRONT_RIGHT_MAX = 67.5f;

        private const float RIGHT_MIN = 67.5f;
        private const float RIGHT_MAX = 112.5f;

        private const float BACK_RIGHT_MIN = 112.5f;
        private const float BACK_RIGHT_MAX = 157.5f;

        private const float BACK_MIN_1 = 157.5f;
        private const float BACK_MAX_1 = 180f;
        private const float BACK_MIN_2 = -180f;
        private const float BACK_MAX_2 = -157.5f;

        private const float BACK_LEFT_MIN = -157.5f;
        private const float BACK_LEFT_MAX = -112.5f;

        private const float LEFT_MIN = -112.5f;
        private const float LEFT_MAX = -67.5f;

        private const float FRONT_LEFT_MIN = -67.5f;
        private const float FRONT_LEFT_MAX = -22.5f;

        public static HitDirection GetHitDirection(Vector3 forward, Vector3 hitDirection)
        {
            var angle = Vector3.SignedAngle(forward, hitDirection, Vector3.up);

            if (angle >= FRONT_MIN && angle < FRONT_MAX)
                return HitDirection.Front;
            if (angle >= FRONT_RIGHT_MIN && angle < FRONT_RIGHT_MAX)
                return HitDirection.FrontRight;
            if (angle >= RIGHT_MIN && angle < RIGHT_MAX)
                return HitDirection.Right;
            if (angle >= BACK_RIGHT_MIN && angle < BACK_RIGHT_MAX)
                return HitDirection.BackRight;
            if (angle >= BACK_MIN_1 || angle < BACK_MAX_2)
                return HitDirection.Back;
            if (angle >= BACK_LEFT_MIN && angle < BACK_LEFT_MAX)
                return HitDirection.BackLeft;
            if (angle >= LEFT_MIN && angle < LEFT_MAX)
                return HitDirection.Left;
            if (angle >= FRONT_LEFT_MIN && angle < FRONT_LEFT_MAX)
                return HitDirection.FrontLeft;

            return HitDirection.Front;
        }
    }
}