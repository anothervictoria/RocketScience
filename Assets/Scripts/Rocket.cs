using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    SceneChanger sceneChanger;
    
    //private Camera mainCamera;

    enum State {Playing, Dead, NextLevel};
    State state = State.Playing;

    // Start is called before the first frame update
    void Start()
    {
        sceneChanger = FindObjectOfType<SceneChanger>();
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        state = State.Playing;        
    }

    

    // Update is called once per frame
    void Update()
    {
        if (state != State.Dead)
        {
            RocketLaunch();
            RocketRotation();
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            StartCoroutine(NextScene());
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
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.Pause();
            flyParticle.Stop();
        }
    }

    private void RocketRotation()
    {      

        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(-rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(rotationSpeed);
        }
        rigidBody.freezeRotation = false;
    }

    void ApplyRotation(float rotationValue)
    {
        rigidBody.freezeRotation = true;
        transform.Rotate(-Vector3.forward * rotationValue * Time.deltaTime);
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
        StartCoroutine(FirstScene());
    }

    void Finish()
    {
        state = State.NextLevel;
        audioSource.Stop();
        audioSource.clip = finishSound;
        audioSource.Play();
        finishParticle.Play();
        Debug.Log(finishSound.name);
        StartCoroutine(NextScene());
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(2);
        sceneChanger.LoadNextLevel();
    }

    IEnumerator FirstScene()
    {
        yield return new WaitForSeconds(2);
        sceneChanger.LoadFirstLevel();
    }
}
