using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayBox : MonoBehaviour
{


    HitSlider hitSlider;
    float hitStrength = 0f;

    bool shouldReload;

    public Rigidbody boxRb;
    public Transform stopTransform;

    public float minSpeed = 0.3f;
    public float acceleration = 5f;
    public float deceleration = -5f;
    public float stoppingDistance = 0.3f;
    public float maxVelocity = 100f;

    public int scoreMultiplier = 1000;


    public Text currentScoreText;
    public Text totalScoreText;

    public GameObject hitText;
    public GameObject reloadSceneText;

    bool shouldAccelerate;
    bool shouldSlowdown;


    float xVel0;
    float xPos0;

    float currentAcceleration;

    float timeElapsed = 0f;

    int thisScore = 0;

    float totalDistance;


    // Start is called before the first frame update
    void Start()
    {
        hitSlider = GetComponent<HitSlider>();
        shouldReload = false;
        shouldAccelerate = false;
        shouldSlowdown = false;

        currentAcceleration = 0f;

        totalDistance = Vector3.Distance(boxRb.position, stopTransform.position);
        
        RoundStart();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            if (shouldReload)
            {
                AddToTotalScore(thisScore);
                shouldReload = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
                
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (!hitSlider.isHit)
            {
                hitSlider.Strike();

                hitStrength = hitSlider.GetHitValue();
                Debug.Log("Hit with : " + hitStrength);

                StartAcceleration();

            }

        }
        
    }

    void FixedUpdate()
    {

        if (Vector3.Distance(boxRb.position, stopTransform.position) <= 0.1f)
        {
            shouldAccelerate = false;
            shouldSlowdown = false;
        }

        // Debug.Log("Current Acceleration : " + currentAcceleration);

        // Should decelerate when the puck reaches certain maximum velocity (max velocity of the machine * hit strength)
        if (boxRb.velocity.magnitude >= (maxVelocity * hitStrength))
        {
            StopAcceleration();
            
        }


        // p = p0 + v0 * t + (0.5 * a * t ^ 2)
        // v0 = zVel0
        // p0 = zPos0
        // a = currentAcceleration
        // t = timeElapsed

        if (shouldAccelerate || shouldSlowdown)
        {
            if(shouldSlowdown && (boxRb.velocity.normalized == (-transform.right)))
            {
                shouldSlowdown = false;
            }
            else
            {
                timeElapsed += Time.fixedDeltaTime;
                Vector3 newPos = boxRb.position;
                newPos.x = (xPos0 + (xVel0 * timeElapsed) + (0.5f * currentAcceleration * (timeElapsed * timeElapsed)));
                boxRb.MovePosition(newPos);
            }
        }

        // Round Complete Show Scores
        if(hitSlider.isHit && !shouldAccelerate && !shouldSlowdown && !shouldReload)
        {
            shouldReload = true;
            RoundComplete();
        }

    }

    void RoundComplete()
    {
        float distToEnd = Vector3.Distance(boxRb.position, stopTransform.position);


        float tempScore = (1f - (distToEnd / totalDistance));
        tempScore += 0.1f;
        tempScore *= 10;
        thisScore = Mathf.RoundToInt(tempScore);
        // Debug.Log("Temp Score is : " + thisScore);

        thisScore *= scoreMultiplier;

        

        

        if(hitStrength == 1f)
        {
            thisScore = 2000;
        }
        

        currentScoreText.text = "Current Score: " + thisScore.ToString();
        hitText.SetActive(false);
        reloadSceneText.SetActive(true);
    }

    void RoundStart()
    {
        totalScoreText.text = "Total Score  " + ScoreKeeper.totalScore.ToString();
        currentScoreText.text = "";
        hitText.SetActive(true);
        reloadSceneText.SetActive(false);
    }

    void StartAcceleration()
    {
        shouldAccelerate = true;
        xVel0 = boxRb.velocity.x;
        xPos0 = boxRb.position.x;

        currentAcceleration = acceleration;

        Debug.Log("Max velocity is : " + maxVelocity * hitStrength);

        timeElapsed = 0f;
    }


    void StopAcceleration()
    {
        if(!shouldSlowdown && boxRb.velocity.magnitude > stoppingDistance)
        {
            shouldAccelerate = false;
            shouldSlowdown = true;

            xVel0 = boxRb.velocity.x;
            xPos0 = boxRb.position.x;

            timeElapsed = 0;

            currentAcceleration = deceleration;
        }
        

    }


    void AddToTotalScore(int value)
    {
        ScoreKeeper.totalScore += value;
    }

}
