using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entity;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : EntityMovement
{

    private void Update()
    {

        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        _entityData.MoveDirection = new Vector3(horizontal, vertical);

    }

}
