using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerOrbit : MonoBehaviour
{
    public Transform firstStar;     
    [SerializeField] float _speedIncreaseRate;
    [SerializeField] float _minOrbitSpeed;
    [SerializeField] float _maxOrbitSpeed;

    [SerializeField] float _radiusIncreaseRate;
    [SerializeField] float _minRadius;
    [SerializeField] float _maxRadius;

    [SerializeField] ParticleSystem _planetDestroyPFX;
    AudioSource _SFX;
    [SerializeField] private float directionalForceMultiplier = 5f;


    [HideInInspector] public Transform currentStar;
    [SerializeField] private float _starEnergyDrainRate;

    private float radius = 2f;
    private float _angularSpeed = 2f; 
    private float angle;

    private Star _currentStarBeingDrained;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private bool isLaunched = false;
    public bool IsPlayerOrbiting;
    public bool IsPlayerDraining;
    private bool isHoldingLeftClick = false;
    private CameraDeadzoneSwitcher cam;

    private Player _player;

    private AudioSource _drainAudio;
    private bool _isPlayingDrainSound;

    private AudioSource _launchAudio;

    private GameManager _gm;

    void Start()
    {
        _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _drainAudio = GameObject.FindGameObjectWithTag("PlayerDrain").GetComponent<AudioSource>();
        _launchAudio = GameObject.FindGameObjectWithTag("PlayerLaunch").GetComponent<AudioSource>();
        _drainAudio.loop = true;
        _player = GetComponent<Player>();
        _SFX = GetComponent<AudioSource>();
        mainCamera = Camera.main;
        cam = GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraDeadzoneSwitcher>();
        currentStar = firstStar;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        if (isLaunched)
        {
            isHoldingLeftClick = Input.GetMouseButton(0);
            return;
        }

        CalculateSpeed();

        if (Input.GetMouseButtonDown(0))
        {
            Launch();
        }
        else
        {
            CalculateOrbit();
        }
    }

    void FixedUpdate()
    {
        if (isLaunched && isHoldingLeftClick)
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mouseWorldPos - transform.position;
            direction.Normalize();

            rb.AddForce(direction * directionalForceMultiplier, ForceMode2D.Force);
        }
    }

    private void CalculateSpeed()
    {
        if (Input.GetMouseButton(1))
        {
            radius -= _radiusIncreaseRate * Time.deltaTime;
            IsPlayerDraining = true;

            if (!_isPlayingDrainSound)
            {
                _drainAudio.Play();
                _isPlayingDrainSound = true;
            }

            float orb = Mathf.Max(_currentStarBeingDrained.StarOrbitRadius, 0.01f);
            float baseMultiplier = 2.2f;
            float drainRate = (1f / orb) * baseMultiplier * Time.deltaTime;
            _currentStarBeingDrained.DrainStarEnergy(drainRate);

        }
        else
        {
            IsPlayerDraining = false;
            radius += _radiusIncreaseRate * Time.deltaTime;

            if (_isPlayingDrainSound)
            {
                _drainAudio.Stop();
                _isPlayingDrainSound = false;
            }
        }

        radius = Mathf.Clamp(radius, _minRadius, _maxRadius);

        float t = Mathf.InverseLerp(_maxRadius, _minRadius, radius);
        _angularSpeed = Mathf.Lerp(_minOrbitSpeed, _maxOrbitSpeed, t);
    }


    private void CalculateOrbit()
    {
        angle += _angularSpeed * Time.deltaTime;

        if (angle > Mathf.PI * 2) angle -= Mathf.PI * 2;

        float x = currentStar.position.x + Mathf.Cos(angle) * radius;
        float y = currentStar.position.y + Mathf.Sin(angle) * radius;

        transform.position = new Vector3(x, y, transform.position.z);
    }

    private void Launch()
    {
        _drainAudio.Stop();
        _isPlayingDrainSound = false;

        _launchAudio.Play();

        IsPlayerOrbiting = false;
        cam.NewFollowTarget(transform, 12);

        Vector2 center = currentStar.position;
        Vector2 offset = (Vector2)transform.position - center;

        Vector2 tangentDirection = new Vector2(-offset.y, offset.x).normalized;

        float launchSpeed = radius * _angularSpeed;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = tangentDirection * launchSpeed;

        isLaunched = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("StarExplosion"))
        {
            _player.Die(3);
        }
        if (other.CompareTag("Star") && other.transform != currentStar)
        {
            IsPlayerOrbiting = true;
            currentStar = other.transform;

            Vector2 offset = transform.position - other.transform.position;
            radius = offset.magnitude;
            angle = Mathf.Atan2(offset.y, offset.x);

            cam.NewFollowTarget(other.transform, radius);
            _maxRadius = radius;

            _currentStarBeingDrained = other.GetComponent<Star>(); //

            isLaunched = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
        }
        else if (other.CompareTag("Planet"))
        {
            _player.AddHp(0.1f);
                
            Instantiate(_planetDestroyPFX, other.transform.position, Quaternion.identity);
            _planetDestroyPFX.Play();

            _SFX.pitch = Random.Range(0.45f, 0.65f);
            _SFX.Play();
            //_SFX.pitch = 1;

            _gm.AddPlanetDestroyed(1);
            Destroy(other.gameObject);
        }
    }
}
