using System.Collections;
using UnityEngine;

public class StarExplosion : MonoBehaviour
{
    public float duration;
    private CircleCollider2D _collider;
    public GameObject _starExplosion;
    public GameObject _blackHolePrefab;

    void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        _collider.isTrigger = true;
        _collider.radius = 0.1f;
    }

    public IEnumerator StartExplosion(float radius)
    {
        GetComponent<AudioSource>().Play(); // sound before supernova

        yield return new WaitForSecondsRealtime(0.7f);

        var explo = Instantiate(_starExplosion,transform.position,Quaternion.identity);
        explo.GetComponent<AudioSource>().Play(); //supernova sound
        var ps = explo.GetComponent<ParticleSystem>();
        var shape = ps.shape;
        shape.radius = radius;

        var main = ps.main;
        main.duration = duration;

        ps.Play();

        var blackHole = Instantiate(_blackHolePrefab, transform.position, Quaternion.identity);
        blackHole.transform.localScale = Vector3.zero;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            _collider.radius = Mathf.Lerp(0f, radius, t);

            float scale = Mathf.Lerp(0f, radius * 2f, t);
            blackHole.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
    }
}
