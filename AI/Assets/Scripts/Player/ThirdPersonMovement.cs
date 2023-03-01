using UnityEngine;

namespace Player
{
    public class ThirdPersonMovement : MonoBehaviour
    {
        public CharacterController controller;
        public Transform cam;

        public float speed = 6;
        private readonly float _gravity = -9.81f;
        public float jumpHeight = 3;
        private Vector3 _velocity;
        private bool _isGrounded;

        public Transform groundCheck;
        private const float GroundDistance = 0.4f;
        public LayerMask groundMask;

        private float _turnSmoothVelocity;
        private const float TurnSmoothTime = 0.1f;

        void Update()
        {
            //jump
            _isGrounded = Physics.CheckSphere(groundCheck.position, GroundDistance, groundMask);

            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -4f;
            }

            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2 * _gravity);
            }

            //gravity
            _velocity.y += _gravity * Time.deltaTime;
            controller.Move(_velocity * Time.deltaTime);
            //walk
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                    TurnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * (speed * Time.deltaTime));
            }

        }
    }
}
