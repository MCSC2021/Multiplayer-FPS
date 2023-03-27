using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimeCharacter.Manager;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

namespace AnimeCharacter.PlayerControl
{
    public class NewPlayerController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private float AnimeBlendSpeed = 8.9f;
        [SerializeField] GameObject cameraHolder;
        private Rigidbody _PlayerRigidbody;
        private InputManager _inputManager;
        private Animator _animator;
        private bool _hasAnimator;
        private int _xVelHash;
        private int _yVelHash;

        private const float _walkSpeed = 3f;
        private const float _runSpeed = 6f;
        private Vector2 _currentVelocity;

        PhotonView PV;
        PlayerManager playerManager;

        void Awake()
        {
            _PlayerRigidbody = GetComponent<Rigidbody>();
            PV = GetComponent<PhotonView>();
            //Referance to Playermanager
            playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        }

        private void Start()
        {
            if (!PV.IsMine)
            {
                Destroy(GetComponentInChildren<Camera>().gameObject);
                Destroy(_PlayerRigidbody);
            }
            else
            {
            
                _inputManager = GetComponent<InputManager>();

                _xVelHash = Animator.StringToHash("X_velocity");
                _yVelHash = Animator.StringToHash("Y_velocity");
            }
            _hasAnimator = TryGetComponent<Animator>(out _animator);

        }

        private void FixedUpdate()
        {
            if (!PV.IsMine)
                return;
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

