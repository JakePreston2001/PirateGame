using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WaterController : MonoBehaviour
{
    public static WaterController instance;

    [SerializeField] private GameObject water;
    [SerializeField] private bool ExampleWaterOne;
    [SerializeField] private bool ExampleWaterTwo;
    private Renderer waterShader;

    #region Waves in editor
    [Header("Wave A")]
    [SerializeField] private float WaveLengthA;
    [Range(0.01f, 0.99f)] [SerializeField] private float SteepnessA;
    [Range(0.01f, 1)][SerializeField] private float DirAX;
    [Range(0.01f, 1)][SerializeField] private float DirAY;

    [Space]
    [Header("Wave B")]
    [SerializeField] private float WaveLengthB;
    [Range(0.01f, 0.99f)][SerializeField] private float SteepnessB;
    [Range(0.01f, 0.99f)][SerializeField] private float DirBX;
    [Range(0.01f, 0.99f)][SerializeField] private float DirBY;

    [Space]
    [Header("Wave C")]
    [SerializeField] private float WaveLengthC;
    [Range(0.01f, 0.99f)][SerializeField] private float SteepnessC;
    [Range(0.01f, 0.99f)][SerializeField] private float DirCX;
    [Range(0.01f, 0.99f)][SerializeField] private float DirCY;
    #endregion

    [HideInInspector]
    public Vector4 WaveA, WaveB, WaveC;

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

        #region set values
        WaveA.x = DirAX;
        WaveA.y = DirAY;
        WaveA.z = SteepnessA;
        WaveA.w = WaveLengthA;

        WaveB.x = DirBX;
        WaveB.y = DirBY;
        WaveB.z = SteepnessB;
        WaveB.w = WaveLengthB;

        WaveC.x = DirCX;
        WaveC.y = DirCY;
        WaveC.z = SteepnessC;
        WaveC.w = WaveLengthC;
        #endregion

    }

    private void Start()
    {

        #region set values at start
        WaveA.x = DirAX;
        WaveA.y = DirAY;
        WaveA.z = SteepnessA;
        WaveA.w = WaveLengthA;

        WaveB.x = DirBX;
        WaveB.y = DirBY;
        WaveB.z = SteepnessB;
        WaveB.w = WaveLengthB;

        WaveC.x = DirCX;
        WaveC.y = DirCY;
        WaveC.z = SteepnessC;
        WaveC.w = WaveLengthC;
        #endregion

        waterShader = water.gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ExampleWaterOne)
        {
            ExampleWaterOne = false;
            DirAX = 0.37f;
            DirAY = 0.21f;
            SteepnessA = 0.22f;
            WaveLengthA = 60;

            DirBX = 0.6f;
            DirBY = 0.01f;
            SteepnessB = 0.18f;
            WaveLengthB = 30;

            DirCX = 0.99f;
            DirCY = 0.01f;
            SteepnessC = 0.08f;
            WaveLengthC = 15;
        }

        if (ExampleWaterTwo)
        {
            ExampleWaterTwo = false;
            DirAX = 0.17f;
            DirAY = 0.17f;
            SteepnessA = 0.22f;
            WaveLengthA = 300;

            DirBX = 0.095f;
            DirBY = 0.01f;
            SteepnessB = 0.28f;
            WaveLengthB = 150;

            DirCX = 0.6f;
            DirCY = 0.46f;
            SteepnessC = 0.31f;
            WaveLengthC = 75;
        }
        
        #region Set Wave Values
        WaveA.x = DirAX;
        WaveA.y = DirAY;
        WaveA.z = SteepnessA;
        WaveA.w = WaveLengthA;

        WaveB.x = DirBX;
        WaveB.y = DirBY;
        WaveB.z = SteepnessB;
        WaveB.w = WaveLengthB;

        WaveC.x = DirCX;
        WaveC.y = DirCY;
        WaveC.z = SteepnessC;
        WaveC.w = WaveLengthC;
        #endregion

        #region set waves in shader
        waterShader.sharedMaterial.SetVector("_WaveA", WaveA);
        waterShader.sharedMaterial.SetVector("_WaveB", WaveB);
        waterShader.sharedMaterial.SetVector("_WaveC", WaveC);
        #endregion
    }
}
