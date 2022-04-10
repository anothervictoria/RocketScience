using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float flySpeed = 100f;
    enum State {Playing, Dead, NextLevel};

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        RocketLaunch();
        RocketRotation();
    }
    private void RocketLaunch()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * flySpeed);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Pause();
        }
    }

    private void RocketRotation()
    {
        rigidBody.freezeRotation = true;
        float speed = rotationSpeed * Time.deltaTime;
        
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * speed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * speed);
        }
        rigidBody.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("ok");
                break;
            case "Finish":
                SceneManager.LoadScene("SceneTwo");
                break;
            case "Powerup":
                Debug.Log("Energy on");
                break;
            default:
                Debug.Log("Rocket BOOOM!");
                SceneManager.LoadScene("SceneOne");
                break;
        }
    }
}
