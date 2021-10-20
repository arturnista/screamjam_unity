using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity
{

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(EntityData))]
    public class EntityMovement : MonoBehaviour
    {

        [SerializeField] protected float _acceleration = 50f;
        [SerializeField] protected float _moveSpeed = 10f;

        protected EntityData _entityData;
        protected Rigidbody2D _rigidbody;
        protected CircleCollider2D _collider;

        protected Vector2 _targetVelocity;
        protected Vector2 _velocity;

        protected Animator _animator;

        protected RaycastHit2D[] _collisionResults = new RaycastHit2D[5];
        protected ContactFilter2D _collisionFilter;
        
        protected bool _isMoving;

        protected bool _isResetting;
        protected Vector2 _resetPosition;

        protected virtual void Awake()
        {            
            _entityData = GetComponent<EntityData>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CircleCollider2D>();
            _animator = GetComponent<Animator>();

            _collisionFilter = new ContactFilter2D()
            {
                useTriggers = false
            };
            _isMoving = true;
        }

        public void StopMovement()
        {
            _isMoving = false;
        }

        public void StartMovement()
        {
            _isMoving = true;
        }

        private void LateUpdate()
        {

            if (_isMoving)
            {
                var speedDiff = Vector3.Dot(_entityData.LookDirection.normalized, _entityData.MoveDirection.normalized);
                speedDiff = speedDiff < 0f ? .7f : 1f;

                _targetVelocity = _entityData.MoveDirection.normalized * _moveSpeed * speedDiff;
            }
            else
            {
                _targetVelocity = Vector2.zero;
            }

            _velocity = Vector2.MoveTowards(_velocity, _targetVelocity, _acceleration * Time.deltaTime);
            _animator.SetFloat("MoveSpeed", _velocity.magnitude);
            Flip();
        }

        public void ResetState(Vector2 position)
        {
            _isResetting = true;
            _resetPosition = position;
        }

        private void FixedUpdate()
        {
            if (_isResetting)
            {
                _isResetting = false;
                _rigidbody.MovePosition(_resetPosition);
                return;
            }
            
            Vector2 motion = _velocity * Time.fixedDeltaTime;
            int collidingAmount = _collider.Cast(motion.normalized, _collisionFilter, _collisionResults, motion.magnitude);
            if (collidingAmount > 0)
            {
                for (int i = 0; i < collidingAmount; i++)
                {
                    var hit = _collisionResults[i];
                    float projection = Vector2.Dot (_velocity, hit.normal);
                    if (projection < 0) 
                    {
                        _velocity = _velocity - projection * hit.normal;
                    }
                }
            }

            if (_isMoving)
            {
                _animator.speed = _velocity.magnitude / 3f;
            }
            else
            {
                _animator.speed = 1f;
            }
            _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.fixedDeltaTime);
        }

        private void Flip()
        {
            float lookSign = Mathf.Sign(_entityData.LookDirection.x);
            if (_entityData.LookDirection.x != 0f && lookSign != Mathf.Sign(transform.localScale.x))
            {
                StartCoroutine(FlipCoroutine(lookSign));
                if (lookSign > 0) _entityData.RotationSum = Vector3.zero;
                else _entityData.RotationSum = new Vector3(0f, 0f, 180);
            }
        }

        private IEnumerator FlipCoroutine(float dir)
        {
            var localScale = transform.localScale;
            while (localScale.x != dir)
            {   
                localScale.x = Mathf.MoveTowards(localScale.x, dir, 25f * Time.deltaTime);
                transform.localScale = localScale;
                yield return null;
            }

            localScale = new Vector3(dir, 1f, 1f);
            transform.localScale = localScale;
        }
        
    }

}
