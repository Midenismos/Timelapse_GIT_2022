using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]

public class LightSource : MonoBehaviour

{
 
    private GameObject Celestial;
	[SerializeField] private GameObject Planet;
	[SerializeField] private GameObject Atmosphere;
	[SerializeField] private GameObject Rings;
    private Transform IlluminationSource;
	private MaterialPropertyBlock _propBlockPlanet;
	private MaterialPropertyBlock _propBlockAtmosphere;
	private MaterialPropertyBlock _propBlockRings;
    private Renderer PlanetM;
	private Renderer RingsM;
    private Renderer AtmosphereM;
	
	public GameObject Sun;
		
public void Start ()

{
        Celestial = this.gameObject;
		
       
		
		
        //Planet = transform.Find("Planet").gameObject;
		if(Planet != null)
			PlanetM = Planet.GetComponent<Renderer>();
		
	
	
		//Atmosphere = transform.Find("Atmosphere").gameObject;
		if(Atmosphere != null)
			AtmosphereM = Atmosphere.GetComponent<Renderer>();
		
			
		//Rings = transform.Find("Rings").gameObject;
		if(Rings != null)
			RingsM = Rings.GetComponent<Renderer>();
		

}

    public void Update()

    {
    	if (Sun != null)
		{
			
	   IlluminationSource = Sun.transform;
	   Vector3 targetDir = IlluminationSource.position - Celestial.transform.position;
			if(PlanetM != null)
            {
				_propBlockPlanet = new MaterialPropertyBlock();
				PlanetM.GetPropertyBlock(_propBlockPlanet);
				_propBlockPlanet.SetVector("_LightSource", targetDir);
				PlanetM.SetPropertyBlock(_propBlockPlanet);
			}

			if (AtmosphereM != null)
			{
				_propBlockAtmosphere = new MaterialPropertyBlock();
				AtmosphereM.GetPropertyBlock(_propBlockAtmosphere);
				_propBlockAtmosphere.SetVector("_LightSourceAtmo", targetDir);
				AtmosphereM.SetPropertyBlock(_propBlockAtmosphere);
			}

			if (Rings != null)
            {
				_propBlockRings = new MaterialPropertyBlock();	   
	   RingsM.GetPropertyBlock(_propBlockRings);
	   _propBlockRings.SetVector("_LightSourceRings", targetDir);
       RingsM.SetPropertyBlock(_propBlockRings);
			}
		}
	}
}

