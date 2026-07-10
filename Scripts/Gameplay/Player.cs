using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Image _hpImg;
    public float _decreaseEnergyRate;
    public float _increaseEnergyRate;
    public float _increaseEnergyRateWhileAbsorbing;
    public ParticleSystem _explosionPFX;

    private float _hp;
    private float _maxHp = 1.1f;
    private PlayerOrbit orbit;
    private Rigidbody2D _rb;
    private GameManager _gm;

    void Start()
    {
        _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _hp = 1;
        orbit = GetComponent<PlayerOrbit>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_hp <= 0)
        {
            Die(1);
        }

        float d_decreaseEnergy = _decreaseEnergyRate * Time.deltaTime;
        float d_increaseEnergyInOrbit = _increaseEnergyRate * Time.deltaTime;
        float d_increaseEnergyWhileAbsorbing = _increaseEnergyRateWhileAbsorbing * Time.deltaTime;

        if (orbit.IsPlayerDraining && orbit.IsPlayerOrbiting && _hp < _maxHp)
        {
            _hp += d_increaseEnergyWhileAbsorbing;
            _hp = Mathf.Min(_hp, _maxHp);
        }
        else if (orbit.IsPlayerOrbiting && _hp < _maxHp)
        {
            _hp += d_increaseEnergyInOrbit;
            _hp = Mathf.Min(_hp, _maxHp);
        }
        else if (_hp > 0)
        {
            _hp -= d_decreaseEnergy;
            _hp = Mathf.Max(_hp, 0f);
        }

        _hpImg.fillAmount = _hp / _maxHp;
    }

    public void AddHp(float amount)
    {
        _hp += amount;
    }

    public void Die(int reason)
    {
        _gm.DeathReason = reason;
        Instantiate(_explosionPFX, transform.position, Quaternion.identity);
        _explosionPFX.Play();
        _gm.GameOver();

        Destroy(gameObject);
    }
}
