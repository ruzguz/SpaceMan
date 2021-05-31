using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    // Camera variables 
    public static CameraFollow sharedInstance;
    float dampingTime = 0.3f;
    public Vector3 velocity = Vector3.zero;
    public float cameraSpeed = 0.5f;
    Vector3 cameraStartPosition;
    

    // KillZone varaible 
    public GameObject killZone; 
    public float killZoneOffset = 10f;
    public Vector3 killZoneVelocity = Vector3.zero;

    // Awake is called before the frame 0
    void Awake() 
    {
        Application.targetFrameRate = 60;

        if (sharedInstance == null) {
            sharedInstance = this;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        // Set camrea start position
        cameraStartPosition = this.transform.position;
        // Set killzone start position
        killZone.transform.position = new Vector3(this.transform.position.x, 
                                                  this.transform.position.y - killZoneOffset, 
                                                   0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // FixedUpdate is called in a fixed frame rate
    void FixedUpdate()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame) {
            // Moving camera 
            MoveCamera(true);
        }
    }

    public void ResetCameraPosition() 
    {
        MoveCamera(false);
    }

    public void MoveCamera(bool smooth)
    {
        // Create new position
        Vector3 destination = new Vector3(
                                            this.transform.position.x,
                                            this.transform.position.y + cameraSpeed,
                                            this.transform.position.z
                                        );
        // Automatic camera movement upward
        if(smooth){
            this.transform.position = Vector3.SmoothDamp(
                                                    this.transform.position,
                                                    destination,
                                                    ref velocity,
                                                    dampingTime
                                                        );
            MoveKillZone();
        // Reset camera position
        } else {
            this.transform.position = cameraStartPosition;
            ResetKillZonePosition();
        }
    }


    // Kill zone methods
    void MoveKillZone()
    {
        Vector3 destination = new Vector3(
                                            killZone.transform.position.x,
                                            killZone.transform.position.y + cameraSpeed,
                                            0
                                        );
        
        killZone.transform.position = Vector3.SmoothDamp(
                                                    killZone.transform.position,
                                                    destination,
                                                    ref killZoneVelocity,
                                                    dampingTime
                                                );
    }

    void ResetKillZonePosition()
    {
        killZone.transform.position = new Vector3(
            cameraStartPosition.x,
            cameraStartPosition.y - killZoneOffset,
            0
        );
    }
}
