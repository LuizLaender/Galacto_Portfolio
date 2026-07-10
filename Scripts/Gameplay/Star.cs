using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{
    [SerializeField] private float _starHP;
    [SerializeField] private GameObject _explosion;
    private Image _hpImg;
    private bool isStarDestroyed;
    public float StarOrbitRadius;
    public bool IsBlackHole = false;
    private GameManager _gm;

    void Awake()
    {
        _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        Image[] images = GetComponentsInChildren<Image>();
        foreach (Image img in images)
        {
            if (img.name == "value")
            {
                _hpImg = img;
                break;
            }
        }
    }

    void Start()
    {
        StarOrbitRadius = GetComponent<CircleCollider2D>().radius;
    }

    public void DrainStarEnergy(float rate)
    {
        if (isStarDestroyed) return;

        if (_starHP <= 0)
        {
            isStarDestroyed = true;
            StartCoroutine(GenerateBlackHole());
        }
        else if (_starHP >= 0)
        {
            _starHP -= rate;
            _hpImg.fillAmount = _starHP;
        }
    }

    IEnumerator GenerateBlackHole()
    {
        // particle effect of explosion and
        // collider of explosion
        GameObject.FindGameObjectWithTag("ScreenFlash").GetComponent<ScreenFlash>().Flash();
        _gm.AddStarDestroyed(1);

        GameObject obj = Instantiate(_explosion, transform.position, Quaternion.identity);
        StarExplosion explosion = obj.GetComponent<StarExplosion>();
        StartCoroutine(explosion.StartExplosion(StarOrbitRadius));


        // white flash on screen
        // sound effect
        // new sprite gets rendered

        yield return new WaitForSeconds(0.5f);
        IsBlackHole = true;
    }
}
