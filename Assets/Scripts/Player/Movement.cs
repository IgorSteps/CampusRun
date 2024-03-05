using UnityEngine;

public class Movement : MonoBehaviour
{
    public float FrontSpeed = Constants.DEFAULT_FORWARD_SPEED;
    public float SideSpeed = Constants.DEFAULT_SIDE_SPEED;

    void Update()
    {
        movePlayer();
    }

    private void movePlayer()
    {
        // Fowards.
        transform.Translate(FrontSpeed * Time.deltaTime * Vector3.forward, Space.World);

        // Sideways.
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if(this.gameObject.transform.position.x > Constants.LEFT_BOUNDARY)
                transform.Translate(SideSpeed * Time.deltaTime * Vector3.left);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (this.gameObject.transform.position.x < Constants.RIGHT_BOUNDARY)
                transform.Translate(SideSpeed * Time.deltaTime * Vector3.right);
        }
    }
}
