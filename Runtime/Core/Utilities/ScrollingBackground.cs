using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    private Renderer _renderer;
    [SerializeField] private float _speed = 0.05f;
    private Vector2 _direction;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (_renderer == null) return;

        _direction.Set(0f, _speed * Time.deltaTime);
        _renderer.material.mainTextureOffset += _direction; 
    }
}
