using UnityEngine;


//In this section, you have to edit OnPointerDown and OnPointerUp sections to make the game behave in a proper way using hJoint
//Hint: You may want to Destroy and recreate the hinge Joint on the object. For a beautiful gameplay experience, joint would created after a little while (0.2 seconds f.e.) to create mechanical challege for the player
//And also create fixed update to make score calculated real time properly.
//Update FindRelativePosForHingeJoint to calculate the position for you rope to connect dynamically
//You may add up new functions into this class to make it look more understandable and cosmetically great.

public class PlayerController : MonoBehaviour {

    [SerializeField] private bool _isFlying;
    public bool IsFlying { get { return _isFlying; } set { _isFlying = value; } }
    [SerializeField]
    private float _forceAmount;
    private bool _isConnected;
    private float _timer;
    private HingeJoint _hJoint;
    private LineRenderer _lRenderer;
    private Rigidbody _playerRigidbody;

    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;
        _isConnected = false;
        _timer = 0.3f;
        EventManager.OnGameStart.AddListener(() => IsFlying = true);
        EventManager.OnLevelFinish.AddListener(() => IsFlying = false);
        EventManager.OnMouseDown.AddListener(PointerDown);
        EventManager.OnMouseUp.AddListener(PointerUp);
        _playerRigidbody = GetComponent<Rigidbody>();
        _hJoint = GetComponent<HingeJoint>();
        _lRenderer = GetComponent<LineRenderer>();
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

    private void FixedUpdate()
    {
        if (!_isConnected)
            return;
        //_playerRigidbody.velocity = Vector3.forward * _forceAmount * Time.deltaTime;

        //_playerRigidbody.AddRelativeForce(Vector3.forward * _forceAmount * Time.deltaTime, ForceMode.Force);

        if (_timer < 0.01f)
            _timer += Time.deltaTime;
        else
        {
            _playerRigidbody.AddRelativeForce(Vector3.forward * _forceAmount * Time.deltaTime,ForceMode.Force);
            _timer = 0;
        }
        
    }

    public void FindRelativePosForHingeJoint(Vector3 blockPosition = default(Vector3))
    {
        //Update the block position on this line in a proper way to Find Relative position for our blockPosition
        if(IsFlying)
            blockPosition = BlockCreator.Instance.GetRelativeBlock(transform.position.z).position;
        transform.eulerAngles = Vector3.zero;
        _hJoint.anchor = new Vector3(blockPosition.x, (blockPosition.y / 2) - transform.position.y, blockPosition.z - transform.position.z);
        _lRenderer.SetPosition(1, _hJoint.anchor);
        _lRenderer.enabled = true;
        Debug.Log("Anchor: " + _hJoint.anchor);
    }

    public void BreakHingeJoint()
    {
        Destroy(_hJoint);
        _lRenderer.enabled = false;
        _isConnected = false;
        _timer = 0.01f;
    }

    public void PointerDown()
    {
        //Debug.Log("Pointer Down");
        //This function works once when player holds on the screen
        //FILL the behaviour here when player holds on the screen. You may or not call other functions you create here or just fill it here
        if (_hJoint == null)
            _hJoint = gameObject.AddComponent<HingeJoint>();
        FindRelativePosForHingeJoint();
        //_playerRigidbody.velocity = Vector3.forward * _forceAmount * Time.deltaTime;
        _isConnected = true;
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
