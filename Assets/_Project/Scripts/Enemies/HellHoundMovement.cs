using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Game.Entity
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class HellHoundMovement : EntityMovement
    {

        [SerializeField] private float _wanderMoveSpeed = 10f;
        [SerializeField] private float _runMoveSpeed = 10f;
        [SerializeField] private float _dodgeMoveSpeed = 10f;
        [Header("SFX")]
        [SerializeField] private GameObject _runningSound;
        [SerializeField] private List<AudioClip> _idleSounds;
        [SerializeField] private List<AudioClip> _attackSounds;

        private EntityData _target;
        private DetectPlayer _detectPlayer;
        private Vector3 _targetPosition;

        private Seeker _aiSeeker;
        private int _waypointIndex;
        private Path _path;

        private AudioSource _audioSource;

        private enum EnemyState
        {
            Wander,
            Attack,
            Scared
        }

        [SerializeField] private EnemyState _currentState;

        private float _zigZagTime;
        private bool _zigZagRightSide;

        private float _attackTime;
        private float _runningTime;

        protected override void Awake()
        {
            base.Awake();
            var target = GameObject.FindGameObjectWithTag("Player");
            _target = target.GetComponent<EntityData>();
            _aiSeeker = GetComponent<Seeker>();
            _detectPlayer = GetComponentInChildren<DetectPlayer>();
            _audioSource = GetComponent<AudioSource>();

            _currentState = EnemyState.Wander;
            _entityData.LookDirection = Vector3.right;
            _targetPosition = transform.position;
            StartCoroutine(PlayIdleSoundCoroutine());
        }

        private IEnumerator PlayIdleSoundCoroutine()
        {
            while (true)
            {
                yield return new WaitUntil(() => _currentState == EnemyState.Wander);

                if (_idleSounds.Count > 0)
                {
                    var sound = _idleSounds[Random.Range(0, _idleSounds.Count)];
                    _audioSource.PlayOneShot(sound);
                    yield return new WaitForSeconds(sound.length);
                }
                yield return new WaitForSeconds(Random.Range(2f, 10f));
            }
        }

        private void OnEnable()
        {
            _entityData.Health.OnDamage += HandleDamage;
            _detectPlayer.OnDetectPlayer += HandleDetectPlayer;
            _detectPlayer.OnLosePlayer += HandleLosePlayer;
        }

        private void OnDisable()
        {
            _entityData.Health.OnDamage -= HandleDamage;
            _detectPlayer.OnDetectPlayer += HandleDetectPlayer;
            _detectPlayer.OnLosePlayer += HandleLosePlayer;
        }

        private void HandleDamage(int dmg)
        {
            _runningTime = 2f;
            _moveSpeed = _dodgeMoveSpeed;
            _currentState = EnemyState.Scared;

            _targetPosition = transform.position + ((transform.position - _target.transform.position).normalized * 10f);
            CreateNewPath(_targetPosition);            
        }

        private void HandleDetectPlayer(Transform player)
        {
            StartAttacking();
        }

        private void HandleLosePlayer(Transform player)
        {
            if (_attackTime < 0f) _currentState = EnemyState.Wander;
        }

        private void StartAttacking()
        {
            _currentState = EnemyState.Attack;
            _attackTime = 4f;
            _audioSource.PlayOneShot(_attackSounds[Random.Range(0, _attackSounds.Count)]);
        }

        private void Update()
        {
            switch (_currentState)
            {
                case EnemyState.Wander:
                    _runningSound.SetActive(false);
                    WanderUpdate();
                    break;
                case EnemyState.Attack:
                    _runningSound.SetActive(true);
                    AttackUpdate();
                    break;
                case EnemyState.Scared:
                    _runningSound.SetActive(true);
                    ScaredUpdate();
                    break;
            }
            
            if (_entityData.MoveDirection.sqrMagnitude > 0.1f)
            {
                _entityData.LookDirection = _entityData.MoveDirection;
            }

            if (_path != null)
            {
                var direction = _path.vectorPath[_waypointIndex] - transform.position;
                if (direction.magnitude < 1f)
                {
                    _waypointIndex += 1;
                }
                else
                {
                    _entityData.MoveDirection = direction.normalized;
                }
            }
            Debug.DrawRay(transform.position, _entityData.LookDirection * 2f, Color.yellow);
        }

        private void WanderUpdate()
        {

            if (_detectPlayer.Detected)
            {
                StartAttacking();
                return;
            }

            void FindNewPoint()
            {
                float distance = Vector3.Distance(_target.transform.position, transform.position);
                float inverseDistance = (1f / distance) * 50f;

                Debug.Log($"Looking for player at {distance} => {inverseDistance} distance");

                _targetPosition = (Vector2)_target.transform.position + (Random.insideUnitCircle * inverseDistance);
                CreateNewPath(_targetPosition);
            }

            if (Vector3.Distance(_targetPosition, transform.position) < 1f)
            {
                FindNewPoint();   
            }

            _moveSpeed = _wanderMoveSpeed;
            if (_path == null || _waypointIndex >= _path.vectorPath.Count)
            {
                FindNewPoint();
            }
        }

        private void AttackUpdate()
        {            
            var directDirection = _target.transform.position - transform.position;
            if (directDirection.magnitude > 10f)
            {
                DirectMovement();
            }
            else
            {
                float dir = Vector3.Dot(directDirection.normalized, _target.LookDirection);

                if (dir < -.7f && _detectPlayer.Detected)
                {
                    ZigZagMovement();
                }
                else
                {
                    DirectMovement();
                }
            }

            _attackTime -= Time.deltaTime;
            if (_attackTime < 0f && _detectPlayer.Detected)
            {
                _currentState = EnemyState.Wander;
            }
        }

        private void DirectMovement()
        {
            _moveSpeed = _runMoveSpeed;
            CreateNewPath(_target.transform.position);
        }

        private void ZigZagMovement()
        {
            _moveSpeed = _dodgeMoveSpeed;
            void UpdateMovement()
            {
                _zigZagTime -= Time.deltaTime;
                if (_zigZagTime > 0) return;
                _zigZagTime = 1f;

                _zigZagRightSide = !_zigZagRightSide;
            }
        
            // Zig zag movement
            Vector3 targetPosition = _target.transform.position;
            Vector3 position = transform.position;

            var directDirection = (targetPosition - position).normalized;
            Vector3 rightOfTarget = Vector3.Cross(directDirection, Vector3.forward);

            Vector3 fPoint = targetPosition + (rightOfTarget * 3f);
            Vector3 sPoint = targetPosition - (rightOfTarget * 3f);
            Vector3 movePosition;

            if (_zigZagRightSide)
            {
                movePosition = fPoint;
            }
            else
            {
                movePosition = sPoint;
            }

            UpdateMovement();

            // Debug.DrawLine(transform.position, movePosition);
            
            _entityData.MoveDirection = (movePosition - position).normalized;
        }

        private void ScaredUpdate()
        {
            _moveSpeed = _dodgeMoveSpeed;
            if (_waypointIndex >= _path.vectorPath.Count)
            {
                _currentState = EnemyState.Wander;
            }

            _runningTime -= Time.deltaTime;
            if (_runningTime < 0f)
            {
                _currentState = EnemyState.Wander;
            }
        }

        private void CreateNewPath(Vector3 target)
        {
            if (!_aiSeeker.IsDone()) return;

            // _path = null;
            _aiSeeker.StartPath(transform.position, target, (path) => {
                _waypointIndex = 0;
                if (!path.error)
                {
                    _path = path;
                }
            });
        }

    }

}
