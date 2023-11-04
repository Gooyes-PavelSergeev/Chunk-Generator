using GooyesPlugin;
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

        [SerializeField] private Animator _animator;

        private CharacterController _characterController;

        private Vector3 _moveDirection;
        private Vector2 _currentInput;

        private float _currentSpeed;

        private void Start()
        {
            _currentSpeed = 0f;
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (CanMove)
            {
                HandleMovement();
            }
            _animator.SetFloat("Speed", _currentSpeed);
        }

        private void HandleMovement()
        {
            _currentInput = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
            if (_currentInput.magnitude < 0.01f) return;

            if (_currentInput.magnitude > 1) _currentInput.Normalize();
            Vector2 speedVector = _currentInput * (IsSprinting ? _sprintSpeed : _walkSpeed);

            _moveDirection = new Vector3(speedVector.y, 0, speedVector.x);
            _characterController.Move(_moveDirection * Time.deltaTime);
            _currentSpeed = _moveDirection.magnitude / _sprintSpeed;
            transform.eulerAngles = new Vector3(0, Mathf.Atan2(_currentInput.y, _currentInput.x) * Mathf.Rad2Deg, 0);
        }
    }
}
