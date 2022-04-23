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
    [SerializeField] AudioClip flyingSound;
    [SerializeField] AudioClip finishSound;
    [SerializeField] AudioClip deadSound;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem flyParticle;
    [SerializeField] ParticleSystem finishParticle;

    enum State {Playing, Dead, NextLevel};
    State state = State.Playing;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        state = State.Playing;
    }

    // Update is called once per frame
    void Update()
    {
        if(state != State.Dead)
        {
            RocketLaunch();
            RocketRotation();
        }        
    }
    private void RocketLaunch()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * flySpeed * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.clip = flyingSound;
                audioSource.Play();
                flyParticle.Play();
                //audioSource.PlayOneShot(flyingSound);
            }
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.Pause();
            flyParticle.Stop();
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
        if (state != State.Playing)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":                
                break;
            case "Finish":
                Finish();
                Invoke("LoadNextLevel", 2f);                
                break;
            case "Powerup":
                Debug.Log("Energy on");
                break;
            default:
                DestroyRocket();
                break;
        }
    }
    void DestroyRocket()
    {
        Debug.Log("Rocket BOOOM!");
        state = State.Dead;
        audioSource.Stop();
        audioSource.PlayOneShot(deadSound);
        deathParticle.Play();
        Invoke("LoadFirstLevel", 2f);
    }

    void Finish()
    {
        state = State.NextLevel;
        audioSource.Stop();
        audioSource.clip = finishSound;
        audioSource.Play();
        finishParticle.Play();
        Debug.Log(finishSound.name);
        Invoke("LoadNextLevel", 2f);
    }

    void LoadNextLevel() //Finish
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void LoadFirstLevel() //GameOver
    {
        SceneManager.LoadScene("LevelOne");
    }
}
