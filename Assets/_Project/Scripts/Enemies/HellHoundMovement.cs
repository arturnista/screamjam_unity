using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class HellHoundMovement : EntityMovement
    {

        [SerializeField] private float _runMoveSpeed = 10f;
        [SerializeField] private float _dodgeMoveSpeed = 10f;

        private EntityData _target;

        private float _zigZagTime;
        private bool _zigZagRightSide;

        private float _runningTime;
        private bool _isRunningFromTarget;

        protected override void Awake()
        {
            base.Awake();
            var target = GameObject.FindGameObjectWithTag("Player");
            _target = target.GetComponent<EntityData>();
        }

        private void OnEnable()
        {
            _entityData.Health.OnDamage += HandleDamage;
        }

        private void OnDisable()
        {
            _entityData.Health.OnDamage -= HandleDamage;
        }

        private void HandleDamage(float dmg)
        {
            _runningTime = 2f;
            _moveSpeed = _dodgeMoveSpeed;
            _isRunningFromTarget = true;
        }

        private void Update()
        {
            var directDirection = (_target.transform.position - transform.position).normalized;
            if (_isRunningFromTarget)
            {
                _entityData.MoveDirection = -directDirection;
                _runningTime -= Time.deltaTime;
                if (_runningTime < 0f)
                {
                    _isRunningFromTarget = false;
                }
            }
            else if (Vector3.Distance(_target.transform.position, transform.position) > 10f)
            {
                DirectMovement();
            }
            else
            {
                float dir = Vector3.Dot(directDirection, _target.LookDirection);

                if (dir > -.7f)
                {
                    DirectMovement();
                }
                else
                {
                    ZigZagMovement();
                }
            }
            
            _entityData.LookDirection = _entityData.MoveDirection;
        }

        private void DirectMovement()
        {
            _moveSpeed = _runMoveSpeed;
            var directDirection = (_target.transform.position - transform.position).normalized;
            _entityData.MoveDirection = directDirection;
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

            Debug.DrawLine(transform.position, fPoint, Color.red);
            Debug.DrawLine(transform.position, sPoint, Color.yellow);
            // Debug.DrawLine(transform.position, movePosition);
            
            _entityData.MoveDirection = (movePosition - position).normalized;
        }

    }

}
