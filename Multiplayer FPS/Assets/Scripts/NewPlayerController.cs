using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimeCharacter.Manager;

namespace AnimeCharacter.PlayerControl
{
    public class NewPlayerController : MonoBehaviour
    {
        [SerializeField] private float AnimeBlendSpeed = 8.9f;
        private Rigidbody _PlayerRigidbody;
        private InputManager _inputManager;
        private Animator _animator;
        private bool _hasAnimator;
        private int _xVelHash;
        private int _yVelHash;

        private const float _walkSpeed = 3f;
        private const float _runSpeed = 6f;
        private Vector2 _currentVelocity;

        private void Start()
        {
            _hasAnimator = TryGetComponent<Animator>(out _animator);
            _PlayerRigidbody = GetComponent<Rigidbody>();
            _inputManager = GetComponent<InputManager>();

            _xVelHash = Animator.StringToHash("X_velocity");
            _yVelHash = Animator.StringToHash("Y_velocity");
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (!_hasAnimator) return;

            float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
            if (_inputManager.Move == Vector2.zero) targetSpeed = 0.1f;

            _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, targetSpeed * _inputManager.Move.x, AnimeBlendSpeed * Time.fixedDeltaTime);
            _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, targetSpeed * _inputManager.Move.y, AnimeBlendSpeed * Time.fixedDeltaTime);

            var xVelDifference = _currentVelocity.x - _PlayerRigidbody.velocity.x;
            var zVelDifference = _currentVelocity.y - _PlayerRigidbody.velocity.z;

            _PlayerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);

            _animator.SetFloat(_xVelHash, _currentVelocity.x);
            _animator.SetFloat(_yVelHash, _currentVelocity.y);

        }
    }
}

