using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity
{

    public class EntityData : MonoBehaviour
    {

        public Vector3 MoveDirection { get; set; }
        public Vector3 LookDirection { get; set; }

        private EntityMovement _movement;
        public EntityMovement Movement
        {
            get
            {
                if (_movement == null) SetupDeps();
                return _movement;
            }
        }
        private EntityHealth _health;
        public EntityHealth Health
        {
            get
            {
                if (_health == null) SetupDeps();
                return _health;
            }
        }

        private void SetupDeps()
        {
            _movement = GetComponent<EntityMovement>();
            _health = GetComponent<EntityHealth>();
        }

    }

}
