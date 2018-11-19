using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_keyboard : MonoBehaviour {
    GameObject currentBlock;
    GameObject previewBlock;
    private float period = 1f;
    private float nextDropTime = 0f;

	// Use this for initialization
	void Start () {
        this.currentBlock = gameObject;
        this.currentBlock.GetComponent<Rigidbody>().useGravity = false;
        this.previewBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
        this.previewBlock.AddComponent<Rigidbody>();
        this.previewBlock.GetComponent<Rigidbody>().useGravity = false;
        this.previewBlock.transform.position = new Vector3(10, 10, 0);
	}

	// Update is called once per frame
    void Update () {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float z_axis = Input.GetAxisRaw("Z");
        Vector3 blockPos = this.currentBlock.transform.position;

        // Handle keyboard input for movement along x/z axes
        if (horizontal < 0) blockPos = handleMovement(blockPos, Vector3.left);
        if (horizontal > 0) blockPos = handleMovement(blockPos, Vector3.right);
        if (z_axis < 0) blockPos = handleMovement(blockPos, Vector3.back);
        if (z_axis > 0) blockPos = handleMovement(blockPos, Vector3.forward);

        // Drop the currentBlock to the floor; 
        if (Time.time > this.nextDropTime)
        {
            blockPos = handleMovement(blockPos, Vector3.down);
            this.nextDropTime += this.period;
        }

        this.currentBlock.transform.position = blockPos;
	}

    // Keeps blocks from going through the walls and the floor
    private Vector3 handleMovement(Vector3 currentPos, Vector3 proposedMovement) { 
        if ((currentPos + proposedMovement).x < -5f) {
            Vector3 tempPos = currentPos;
            tempPos.x = -4.5f;
            return tempPos;
        }

        if ((currentPos + proposedMovement).x > 5f)
        {
            Vector3 tempPos = currentPos;
            tempPos.x = 4.5f;
            return currentPos;
        }

        if ((currentPos + proposedMovement).z < -5f)
        {
            Vector3 tempPos = currentPos;
            tempPos.z = -4.5f;
            return currentPos;
        }

        if ((currentPos + proposedMovement).z > 5f)
        {
            Vector3 tempPos = currentPos;
            tempPos.z = 4.5f;
            return currentPos;
        }

        if ((currentPos + proposedMovement).y < 0f)
        {
            Vector3 tempPos = currentPos;
            tempPos.y = .5f;
            this.currentBlock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            this.previewBlock.transform.position = new Vector3(0, 10, 0);
            return currentPos;
        }

        return currentPos + proposedMovement; 
    }

    private void lockTransition() {
        currentBlock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        currentBlock = previewBlock;
        currentBlock.transform.position = new Vector3(0, 10, 0); 
        previewBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
        previewBlock.name = "Preview";
        previewBlock.transform.position = new Vector3(10, 100, 0);
        previewBlock.AddComponent<Rigidbody>();
        previewBlock.GetComponent<Rigidbody>().useGravity = false;
    }
}