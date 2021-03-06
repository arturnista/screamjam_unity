using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Game.Entity
{

    public class EnemyMovement : EntityMovement
    {

        private enum EnemyState
        {
            Wander,
            Attack,
            PerformingAttack,
            Scared,
            Death
        }

        [Header("Wander")]
        [SerializeField] private float _wanderSpeed;
        [Header("Attack")]
        [SerializeField] private float _attackSpeed;
        [SerializeField] private LayerMask _detectMask;
        [SerializeField] private float _attackDistance = 2f;
        [Header("SFX")]
        [SerializeField] private GameObject _runningSound;
        [SerializeField] private List<AudioClip> _idleSounds;
        [SerializeField] private List<AudioClip> _attackSounds;
        [SerializeField] private List<AudioClip> _deathSounds;

        private EnemyState _currentState;

        private AudioSource _audioSource;
        private AnimationEffects _animationEffects;

        private float _tensionIncrease;
        private float _tension;
        public float Tension => _tension;

        private Seeker _aiSeeker;
        private EntityData _target;

        private Vector3 _targetPosition;
        private Vector3 _lastTargetPosition;

        private int _waypointIndex;
        private Path _path;

        private bool _reachPosition;
        private float _attackTime;
        private float _wanderTime;
        private float _scaredTime;

        private Coroutine _attackCoroutine;

        protected override void Awake()
        {
            base.Awake();

            _tension = 1f;

            var target = GameObject.FindGameObjectWithTag("Player");
            _target = target.GetComponent<EntityData>();
            _aiSeeker = GetComponent<Seeker>();
            _audioSource = GetComponent<AudioSource>();
            _animationEffects = GetComponentInChildren<AnimationEffects>();

            _entityData.LookDirection = Vector3.right;
            _reachPosition = true;

            StartWander();
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
            GetComponent<EnemyHealth>().OnDeath += HandleDeath;
            _animationEffects.OnAttackDetect += HandleDealDamage;
        }

        private void OnDisable()
        {
            _entityData.Health.OnDamage -= HandleDamage;
            GetComponent<EnemyHealth>().OnDeath -= HandleDeath;
            _animationEffects.OnAttackDetect -= HandleDealDamage;
        }

        private void HandleDamage(int dmg)
        {
            StartScared();
        }

        private void HandleDeath()
        {
            StartDeath();
        }

        private void HandleDealDamage()
        {
            if (Vector3.Distance(_target.transform.position, transform.position) < _attackDistance)
            {
                _target.Health.DealDamage(1);
            }
        }

        private void Update()
        {
            _tension = Mathf.Clamp(_tension + _tensionIncrease * Time.deltaTime, 1f, 3f);
            switch (_currentState)
            {
                case EnemyState.Wander:
                    TargetAlarm();
                    WanderUpdate();
                    break;
                case EnemyState.Attack:
                    TargetAlarm();
                    AttackUpdate();
                    break;
                case EnemyState.PerformingAttack:
                    PerformingAttackUpdate();
                    break;
                case EnemyState.Scared:
                    ScaredUpdate();
                    break;
            }

            if (_entityData.MoveDirection.sqrMagnitude > 0.1f)
            {
                _entityData.LookDirection = _entityData.MoveDirection;
            }

            CreateNewPath();
            PathMovement();
        }

        // Detect if the target can be attacked
        private void TargetAlarm()
        {
            bool CheckForLineOfSight()
            {
                RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, _target.transform.position, _detectMask);
                if (hits.Length == 0) Debug.DrawLine(transform.position, _target.transform.position, Color.green);
                else Debug.DrawLine(transform.position, _target.transform.position, Color.red);
                return hits.Length == 0;
            }

            void Attack()
            {
                if (_currentState != EnemyState.Attack) StartAttack();
                else KeepAttack();
            }

            // Decrease the max distance if the lamp is not active
            float maxDistance = ItemSwitch.IsLampActive ? 15f : 8f;

            var direction = _target.transform.position - transform.position;
            // If player is far away from the enemy
            if (direction.magnitude > maxDistance) return;
            // If enemy doest not have line of sight with the player
            if (!CheckForLineOfSight()) return;

            // If player is very close to enemy
            if (direction.magnitude < 2f)
            {
                Attack();
                return;
            }
            float angle = Vector3.Dot(_entityData.LookDirection.normalized, direction.normalized);
            // If player is in front of the enemy
            if (angle > .3f)
            {
                Attack();
                return;
            }
        }

#region Wander Behaviour
        private void FindNewWanderPoint()
        {
            float distance = Vector3.Distance(_target.transform.position, transform.position);
            float inverseDistance = (1f / distance) * 50f;

            _targetPosition = (Vector2)_target.transform.position + (Random.insideUnitCircle * inverseDistance);
            _wanderTime = 5f;
        }

        private void StartWander()
        {
            _tensionIncrease = -0.2f;
            _moveSpeed = _wanderSpeed;

            _currentState = EnemyState.Wander;
            FindNewWanderPoint();
            _runningSound.SetActive(false);
        }

        private void WanderUpdate()
        {
            if (_reachPosition) FindNewWanderPoint();
            _wanderTime -= Time.deltaTime;
            if (_wanderTime <= 0f)
            {
                FindNewWanderPoint();
            }
        }
#endregion

#region Enemy Attack Behaviour
        public void StartAttack()
        {
            _tensionIncrease = 0.7f;
            _moveSpeed = _attackSpeed;

            _runningSound.SetActive(true);
            _audioSource.PlayOneShot(_attackSounds[Random.Range(0, _attackSounds.Count)]);
            
            KeepAttack();
            _currentState = EnemyState.Attack;
        }

        private void KeepAttack()
        {
            _attackTime = 5f;
        }

        private void AttackUpdate()
        {
            if (Vector3.Distance(_target.transform.position, transform.position) < _attackDistance)
            {
                StartPerformingAttack();
                return;
            }

            _targetPosition = _target.transform.position;
            _attackTime -= Time.deltaTime;
            if (_attackTime < 0f)
            {
                StartWander();
            }
        }
#endregion

#region Enemy Performing Attack Behaviour
        private void StartPerformingAttack()
        {
            _tensionIncrease = 0.7f;
            _attackTime = 0f;
            _currentState = EnemyState.PerformingAttack;
            _targetPosition = transform.position;
            _attackCoroutine = StartCoroutine(AttackCoroutine());
        }

        private void PerformingAttackUpdate()
        {

        }

        private IEnumerator AttackCoroutine()
        {
            _animator.SetTrigger("Attack");
            StopMovement();
            yield return new WaitForSeconds(1f);
            StartMovement();

            StartAttack();
            _attackCoroutine = null;
        }
#endregion

#region Enemy Scared Behaviour
        private void StartScared()
        {
            _tensionIncrease = -0.2f;
            _moveSpeed = _attackSpeed;
            _scaredTime = 8f;
            
            if (_attackCoroutine != null)
            {
                _animator.SetTrigger("Scared");
                StartMovement();
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }

            _currentState = EnemyState.Scared;
            _runningSound.SetActive(true);

            var direction = (transform.position - _target.transform.position).normalized;
            _targetPosition = transform.position + (direction * 10f);
            _reachPosition = false;
        }
        
        private void ScaredUpdate()
        {
            if (_reachPosition) StartWander();
            _scaredTime -= Time.deltaTime;
            if (_scaredTime < 0f)
            {
                StartWander();
            }
        }
#endregion
        private void StartDeath()
        {
            _tension = 1f;
            _tensionIncrease = 0f;

            _currentState = EnemyState.Death;
            _runningSound.SetActive(false);
            _audioSource.PlayOneShot(_deathSounds[Random.Range(0, _deathSounds.Count)]);
        }

        private void CreateNewPath()
        {
            if (!_aiSeeker.IsDone()) return;
            if (Vector3.Distance(_targetPosition, _lastTargetPosition) < 1f) return;

            if (Vector3.Distance(_targetPosition, transform.position) < 1f)
            {
                _path = null;
                _reachPosition = true;
                return;
            }

            _lastTargetPosition = _targetPosition;

            _aiSeeker.StartPath(transform.position, _targetPosition, (path) => {
                _waypointIndex = 0;
                if (!path.error)
                {
                    _reachPosition = false;
                    _path = path;
                }
            });
        }

        private void PathMovement()
        {
            if (_path == null) return;
            if (_reachPosition) return;
            
            if (_waypointIndex >= _path.vectorPath.Count)
            {
                _reachPosition = true;
                return;
            }

            _reachPosition = false;
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

    }

}
