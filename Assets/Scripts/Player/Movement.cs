using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed; 
    void Start()
    {
        Debug.Log("Left boundary: " + Constants.LEFT_BOUNDARY + "\n " + "Right boundary:" + Constants.RIGHT_BOUNDARY);
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
        if (Input.GetKey(KeyCode.A))
        {
            if(this.gameObject.transform.position.x > Constants.LEFT_BOUNDARY)
                transform.Translate(3.0f * Time.deltaTime * Vector3.left);
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (this.gameObject.transform.position.x < Constants.RIGHT_BOUNDARY)
                transform.Translate(3.0f * Time.deltaTime * Vector3.right);
        }
    }
}
