using UnityEngine;

namespace CG
{
    public class Player : MonoBehaviour
    {
        public bool CanMove { get; private set; } = true;
        private bool IsSprinting => canSprint && Input.GetKey(sprintKey);

        [SerializeField] private bool canSprint = true;
        [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
        [SerializeField] private float _walkSpeed = 3.0f;
        [SerializeField] private float _sprintSpeed = 6.0f;

        private CharacterController _characterController;

        private Vector3 _moveDirection;
        private Vector2 _currentInput;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (CanMove)
            {
                HandleMovement();
            }
        }

        private void HandleMovement()
        {
            _currentInput = new Vector2(
                (IsSprinting ? _sprintSpeed : _walkSpeed) * Input.GetAxis("Vertical"),
                (IsSprinting ? _sprintSpeed : _walkSpeed) * Input.GetAxis("Horizontal"));

            float moveDirectionY = _moveDirection.y;

            _moveDirection =
                (transform.TransformDirection(Vector3.forward) * _currentInput.x) +
                (transform.TransformDirection(Vector3.right) * _currentInput.y);

            _moveDirection.y = moveDirectionY;

            _characterController.Move(_moveDirection * Time.deltaTime);
        }
    }
}
