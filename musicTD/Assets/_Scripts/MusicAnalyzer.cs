using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicAnalyzer : MonoBehaviour {
    [System.Serializable]
    public struct Ranges
    {
        public string Name;
        public float lowVal;
        public float highVal;
    }
    [Range(1f,3f)] public float sensitivity;
    public int nSamples = 1024;
    [SerializeField]
    public Ranges[] freqRanges;

    private AudioSource AS;
    private Music M;
    private float[] freqData;
    private float[] unitData;
    private float fMax;
    private float SongLength;

    private void Awake()
    {
        fMax = Mathf.FloorToInt(AudioSettings.outputSampleRate / 2);
    }

    void Start ()
    {
        AS = GetComponent<AudioSource>();
        M = GetComponent<Music>();

        freqData = new float[nSamples];
        unitData = new float[freqRanges.Length];
        SongLength = 0;
    }
	
	void Update ()
    {
        if (AS.isPlaying)
        {
            // get spectrum
            AS.GetSpectrumData(freqData, 0, FFTWindow.BlackmanHarris);
            SongLength += Time.deltaTime / 2000;

            for (int i = 0; i < unitData.Length; i++)
            {
                if (i == 1)
                {
                    // Unit health will increase with song duration
                    unitData[i] += freqData[i] + SongLength;
                }
                else
                {
                    unitData[i] += freqData[i];
                }
            }

            if (unitData[0] > sensitivity)
            {
                float buff = 3f / sensitivity * .625f;

                for (int j = 0; j < unitData.Length; j++)
                    unitData[j] *= buff;

                M.SpawnEnemyUnit(unitData);

                for (int j = 0; j < unitData.Length; j++)
                    unitData[j] = 0;
            }
        }
    }

    private float BandVol(float fLow, float fHigh)
    { 
        fLow = Mathf.Clamp(fLow, 20, fMax); // limit low...
        fHigh = Mathf.Clamp(fHigh, fLow, fMax); // and high frequencies
         
        int n1 = Mathf.FloorToInt(fLow * nSamples / fMax);
        int n2 = Mathf.FloorToInt(fHigh * nSamples / fMax);
        float sum = 0;

        // average the volumes of frequencies fLow to fHigh
        for (int i = n1; i < n2; i++)
        {
            sum += freqData[i] * (i+1);
        }
        return sum / (n2 - n1 + 1);
     }
}
