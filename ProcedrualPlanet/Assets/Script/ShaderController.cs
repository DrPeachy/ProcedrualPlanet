using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    public static ShaderController sc;
    public Material terrain;
    public Material ocean;
    public Material space;
    //=============terrain=============
    public float seed {set;get;}
    public float frequency {set;get;}
    public float amplitude {set;get;}
    public float lacunartiy {set;get;}
    public float persistence{set;get;}
    public float terrainHeight {set;get;}
    public float colorFactor {set;get;}

    //=============ocean================
    public float oceanHeight {set;get;}
    public float foamDensity {set;get;}
    public float foamStrength {set;get;}
    public float foamSpeed {set;get;}
    public float waveDensity {set;get;} 
    public float waveIntensity {set;get;}
    public float waveSpeed {set;get;}
    public float oceanColor {set;get;}
    public float foamColor {set;get;}


    //============space=================
    public float starDensity {set;get;}
    public float starSpeed {set;get;}
    public float darkColor {set;get;}
    public float lightColor {set;get;}
    //===========crater=================
    public bool craterOn {set;get;}
    public bool foamWhite {set;get;}
    public float craterHeight {set;get;}
    public float craterDepth {set;get;}
    public float craterDensity {set;get;}
    private void Awake() {
        sc = this;
    }

    private void Start() {
        Initialize();
    }
    public void Initialize(){

        seed = 3076f;
        frequency = 1f;
        amplitude = 1.15f;
        lacunartiy = 2.9f;
        persistence = 0.319f;
        terrainHeight = 2f;
        colorFactor = 0.65f;

        oceanHeight = 2f;
        foamDensity = 10f;
        foamStrength = 0.894f;
        foamSpeed = 0.86f;
        waveDensity = 2.43f;
        waveIntensity = 0.079f;
        waveSpeed = 1f;
        oceanColor = 0.488f;
        foamColor = 0f;

        starDensity = 0.03f;
        starSpeed = 2.4e-07f;
        darkColor = 0.4f;
        lightColor = 0.8f;

        craterOn = false;
        foamWhite = true;
        craterHeight = 1f;
        craterDepth = 2.56f;
        craterDensity = 5f;

        ocean.SetColor("_foamColor", Color.white);
        ocean.SetColor("_waterColor", Color.cyan);
    }
    public void UpdateTerrain(){
        terrain.SetFloat("_seed", seed);
        terrain.SetFloat("_frequency", frequency);
        terrain.SetFloat("_amplitude", amplitude);
        terrain.SetFloat("_lacunarity", lacunartiy);
        terrain.SetFloat("_persistence", persistence);
        terrain.SetFloat("_terrainHeight", terrainHeight);
        terrain.SetFloat("_ColorRangeFactor", colorFactor);
    }

    public void UpdateOcean(){
        ocean.SetColor("_waterColor", Color.HSVToRGB(oceanColor, 1, 1));
        ocean.SetFloat("_foamDensity", foamDensity);
        ocean.SetFloat("_foamStrength", foamStrength);
        ocean.SetColor("_foamColor", Color.HSVToRGB(foamColor, 1, 1));
        ocean.SetFloat("_foamSpeed", foamSpeed);
        ocean.SetFloat("_waveSpeed", waveSpeed);
        ocean.SetFloat("_waveDensity", waveDensity);
        ocean.SetFloat("_waveHeight", oceanHeight);
        ocean.SetFloat("_waveIntensity", waveIntensity);
    }

    public void UpdateSpace(){
        space.SetColor("_Dark", Color.HSVToRGB(darkColor, 1, 1));
        space.SetColor("_Light", Color.HSVToRGB(lightColor, 1, 1));
        space.SetFloat("_StarSize", starDensity);
        space.SetFloat("_RotateSpeed", starSpeed);
    }

    public void UpdateCrater(){
        terrain.SetFloat("_craterHeight", craterHeight);
        terrain.SetFloat("_craterDepth", craterDepth);
        terrain.SetFloat("_craterDensity", craterDensity);
    }

    public void UpdateToggle(){
        Debug.Log(foamWhite);
        terrain.SetInt("_craterOn", (craterOn)? 1 : 0);
        ocean.SetColor("_foamColor", (foamWhite)? Color.white : Color.HSVToRGB(foamColor, 1, 1));
        
    }
}