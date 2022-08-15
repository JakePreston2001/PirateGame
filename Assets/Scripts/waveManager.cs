using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveManager : MonoBehaviour
{

    public static waveManager instance;
    [SerializeField] Wave[] Waves;
    private float offset;
    [SerializeField] private float speed;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object");
            Destroy(this);
        }
        Waves = new Wave[4];
        for (int i = 0; i < Waves.Length; i++)
        {
            Waves[0] = new Wave(0.22f, 60, new Vector2(0.37f,0.21f));
            Waves[1] = new Wave(0.18f, 30, new Vector2(0.6f,0.01f));
            Waves[2] = new Wave(0.08f, 15, new Vector2(1f,0f));
        }
    }
    private void Update()
    {
        offset += Time.deltaTime * speed;
    }

    internal float GetWaveHeight(float x, float z)
    {
        Vector2 position = new Vector2(x, z);
        float height = 0;

        height += GerstnerWave(Waves[0], position);
        height += GerstnerWave(Waves[1], position);
        height += GerstnerWave(Waves[2], position);

        return height;
    }
    private float GerstnerWave(Wave _wave, Vector2 _p)
    {
        float _cOffset = _wave.c * offset;
        float _f = _wave.k * (Vector2.Dot(_wave.Direction, _p) - _cOffset);
        float _af = _wave.a * Mathf.Cos(_f);
        _p.x -= _wave.Direction.x * _af;
        _p.y -= _wave.Direction.y * _af;
        _f = _wave.k * (Vector2.Dot(_wave.Direction, _p) - _cOffset);
        return _wave.a * Mathf.Sin(_f);
    }
}

internal class Wave
{
    private float _steepness;
    internal float Steepness
    {
        get => _steepness;
        set
        {
            _steepness = value;
            a = value / k;
        }
    }
    private float _length;
    internal float Length
    {
        get => _length;
        set
        {
            _length = value;
            k = 2f * Mathf.PI / value;
            c = Mathf.Sqrt(9.8f / k);
            a = _steepness / k;
        }
    }
    private Vector2 _direction;
    internal Vector2 Direction
    {
        get => _direction;
        set => _direction = value.normalized;
    }

    internal float k = 1f;
    internal float c = 1f;
    internal float a = 1f;

    internal Wave(float _steepness, float _length, Vector2 _direction)
    {
        // Order is important
        Length = _length;
        Steepness = _steepness;
        Direction = _direction;
    }
}