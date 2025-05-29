using UnityEngine;
using Photon.Pun;

public class MultiplayerMovementController : MonoBehaviourPun
{

    public float speed = 5f;

    private void Start()
    {
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(h, 0, v) * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
