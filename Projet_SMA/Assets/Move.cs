using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
    // Déclaration de la variable de vitesse
    public float m_speed = 0.1f;
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // Création d'un nouveau vecteur de déplacement
        Vector3 move = new Vector3();

        // Récupération des touches haut et bas
        if (Input.GetKey(KeyCode.UpArrow))
            move.z += m_speed;
        if (Input.GetKey(KeyCode.DownArrow))
            move.z -= m_speed;

        // Récupération des touches gauche et droite
        if (Input.GetKey(KeyCode.LeftArrow))
            move.x -= m_speed;
        if (Input.GetKey(KeyCode.RightArrow))
            move.x += m_speed;

        // On applique le mouvement à l'objet
        transform.position += move;

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
	}
}
