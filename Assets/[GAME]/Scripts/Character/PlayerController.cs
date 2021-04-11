using UnityEngine;
//In this section, you have to edit OnPointerDown and OnPointerUp sections to make the game behave in a proper way using hJoint
//Hint: You may want to Destroy and recreate the hinge Joint on the object. For a beautiful gameplay experience, joint would created after a little while (0.2 seconds f.e.) to create mechanical challege for the player
//And also create fixed update to make score calculated real time properly.
//Update FindRelativePosForHingeJoint to calculate the position for you rope to connect dynamically
//You may add up new functions into this class to make it look more understandable and cosmetically great.

public class PlayerController : MonoBehaviour {

    [SerializeField] private bool _isFlying;
    public bool IsFlying { get { return _isFlying; } set { _isFlying = value; } }
    [SerializeField] private float _forceAmount;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _hookTime;
    private RigidbodyConstraints _xRotationFrozen;
    private RigidbodyConstraints _defaultConstraints;
    private Vector3 _hookPos;
    private Vector3 _blockPos;
    private float t;
    private bool _isHooking;
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
        _timer = 0.01f;
        t = 0;
        EventManager.OnGameStart.AddListener(() => IsFlying = true);
        EventManager.OnLevelFinish.AddListener(() => IsFlying = false);
        EventManager.OnMouseDown.AddListener(PointerDown);
        EventManager.OnMouseUp.AddListener(PointerUp);
        _playerRigidbody = GetComponent<Rigidbody>();
        _hJoint = GetComponent<HingeJoint>();
        _lRenderer = GetComponent<LineRenderer>();
        _defaultConstraints = _playerRigidbody.constraints;
        _xRotationFrozen = _defaultConstraints | RigidbodyConstraints.FreezeRotationX;
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
        _blockPos = new Vector3(0, 10, 0);
        FindRelativePosForLineRenderer();
	}

    private void FixedUpdate()
    {
        if(_isHooking)
        {
            HookLineRenderer();
        }
        if (_isConnected && IsFlying)
        {
            if (_playerRigidbody.velocity.z > _maxSpeed)
                _playerRigidbody.velocity = new Vector3(_playerRigidbody.velocity.x, _playerRigidbody.velocity.y, _maxSpeed);
            if (_timer < 0.01f)
                _timer += Time.deltaTime;
            else
            {
                _playerRigidbody.AddRelativeForce(Vector3.forward * _forceAmount * Time.deltaTime, ForceMode.Force);
                _timer = 0;
            }
        }
    }

    public void SetRelativePosForHingeJoint()
    {
        //Update the block position on this line in a proper way to Find Relative position for our blockPosition
        if (_hJoint == null)
            _hJoint = gameObject.AddComponent<HingeJoint>();
        _playerRigidbody.constraints = _defaultConstraints;
        _hJoint.anchor = _lRenderer.GetPosition(1);
        _isConnected = true;
    }
    private void FindRelativePosForLineRenderer()
    {
        if (IsFlying)
            _blockPos = BlockCreator.Instance.GetRelativeBlock(transform.position.z).position;
        else
        {
            CalculateHookPos();
            SetRelativePosForHingeJoint();
            return;
        }
        _lRenderer.SetPosition(1, Vector3.zero);
        _isHooking = true;
        _lRenderer.enabled = true;
    }

    private void HookLineRenderer()
    {
        t += Time.deltaTime / _hookTime;
        CalculateHookPos();
        _lRenderer.SetPosition(1, Vector3.Lerp(Vector3.zero, _hookPos, t));
        if (t >= 1)
        {
            _isHooking = false;
            SetRelativePosForHingeJoint();
        }
    }

    private void CalculateHookPos()
    {
        _hookPos = new Vector3(_blockPos.x, _blockPos.y - BlockCreator.Instance._blockLength - transform.position.y, _blockPos.z - transform.position.z);
    }

    public void BreakHingeJoint()
    {
        Destroy(_hJoint);
        _lRenderer.enabled = false;
        _isConnected = false;
        _isHooking = false;
        _timer = 0.01f;
        t = 0;
        _playerRigidbody.constraints = _xRotationFrozen;
        transform.eulerAngles = Vector3.zero;
    }

    public void PointerDown()
    {
        //Debug.Log("Pointer Down");
        //This function works once when player holds on the screen
        //FILL the behaviour here when player holds on the screen. You may or not call other functions you create here or just fill it here
        FindRelativePosForLineRenderer();
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
        ICollectable collectable = other.GetComponent<ICollectable>();
        if(collectable != null)
        {
            collectable.Collect();
        }
    }
}