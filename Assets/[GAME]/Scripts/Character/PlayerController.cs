using UnityEngine;


//In this section, you have to edit OnPointerDown and OnPointerUp sections to make the game behave in a proper way using hJoint
//Hint: You may want to Destroy and recreate the hinge Joint on the object. For a beautiful gameplay experience, joint would created after a little while (0.2 seconds f.e.) to create mechanical challege for the player
//And also create fixed update to make score calculated real time properly.
//Update FindRelativePosForHingeJoint to calculate the position for you rope to connect dynamically
//You may add up new functions into this class to make it look more understandable and cosmetically great.

public class PlayerController : MonoBehaviour {

    [SerializeField] private bool _isFlying;
    public bool IsFlying { get { return _isFlying; } set { _isFlying = value; } }

    private HingeJoint hJoint;
    private LineRenderer lRenderer;
    private Rigidbody playerRigidbody;

    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;
        EventManager.OnGameStart.AddListener(() => IsFlying = true);
        EventManager.OnLevelFinish.AddListener(() => IsFlying = false);
        EventManager.OnMouseDown.AddListener(PointerDown);
        EventManager.OnMouseUp.AddListener(PointerUp);
        playerRigidbody = GetComponent<Rigidbody>();
        hJoint = GetComponent<HingeJoint>();
        lRenderer = GetComponent<LineRenderer>();
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;
        EventManager.OnGameStart.RemoveListener(() => IsFlying = true);
        EventManager.OnLevelFinish.RemoveListener(() => IsFlying = false);
        EventManager.OnMouseDown.RemoveListener(PointerDown);
        EventManager.OnMouseUp.RemoveListener(PointerUp);
    }


    void Start ()
    {
        FindRelativePosForHingeJoint(new Vector3(0,10,0));
	}
	
    public void FindRelativePosForHingeJoint(Vector3 blockPosition = default(Vector3))
    {
        //Update the block position on this line in a proper way to Find Relative position for our blockPosition
        if(IsFlying)
            blockPosition = BlockCreator.Instance.GetRelativeBlock(transform.position.z).position;
        hJoint.anchor = new Vector3(blockPosition.x, (blockPosition.y / 2) - transform.position.y, blockPosition.z - transform.position.z);
        lRenderer.SetPosition(1, hJoint.anchor);
        lRenderer.enabled = true;
        Debug.Log("Anchor: " + hJoint.anchor);
    }

    public void BreakHingeJoint()
    {
        Destroy(hJoint);
        lRenderer.enabled = false;
    }

    public void PointerDown()
    {
        //Debug.Log("Pointer Down");
        //This function works once when player holds on the screen
        //FILL the behaviour here when player holds on the screen. You may or not call other functions you create here or just fill it here
        if (hJoint == null)
            hJoint = gameObject.AddComponent<HingeJoint>();
        FindRelativePosForHingeJoint();
    }

    public void PointerUp()
    {
        //Debug.Log("Pointer Up");
        //This function works once when player takes his/her finger off the screen
        //Fill the behaviour when player stops holding the finger on the screen.
        BreakHingeJoint();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Block") && IsFlying)
        {
            PointerUp(); //Finishes the game here to stoping holding behaviour
            GameManager.Instance.GameEnd();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Point"))
        {
            if(Vector3.Distance(transform.position, other.gameObject.transform.position) < .5f)
            {
                ScoreManager.Instance.score += 10f;
            }
            else
            {
                ScoreManager.Instance.score += 5f;
            }
            other.gameObject.SetActive(false);
        }
    }
}
