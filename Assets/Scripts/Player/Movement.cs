using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed; 
    void Start()
    {
        //Debug.Log("Left boundary: " + Constants.LEFT_BOUNDARY + "\n " + "Right boundary:" + Constants.RIGHT_BOUNDARY);
    }

    void Update()
    {
        movePlayer();
    }

    private void movePlayer()
    {
        // Fowards.
        transform.Translate(Speed * Time.deltaTime * Vector3.forward, Space.World);

        // Sideways.
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if(this.gameObject.transform.position.x > Constants.LEFT_BOUNDARY)
                transform.Translate(Speed * Time.deltaTime * Vector3.left);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (this.gameObject.transform.position.x < Constants.RIGHT_BOUNDARY)
                transform.Translate(Speed * Time.deltaTime * Vector3.right);
        }
    }
}
