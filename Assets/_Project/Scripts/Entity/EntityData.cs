using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity
{

    public class EntityData : MonoBehaviour
    {

        public Vector3 MoveDirection { get; set; }
        public Vector3 LookDirection { get; set; }

        public EntityMovement Movement { get; protected set; }
        public EntityHealth Health { get; protected set; }

        private void Awake()
        {
            Movement = GetComponent<EntityMovement>();
            Health = GetComponent<EntityHealth>();
        }

    }

}
