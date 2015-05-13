using UnityEngine;
using System.Collections;

public class SceneSkyboxData
{
	public double x;
	
	public double y;
	
	public double z;
	
	public SkyboxItem far = null;
	
	public SkyboxItem near = null;
	
	
	public SceneSkyboxData()
	{}
	
	public SceneSkyboxData( Transform skyboxTransform )
	{
		if( skyboxTransform != null )
		{
			this.x = skyboxTransform.position.x;
			
			this.y = skyboxTransform.position.y;
			
			this.z = skyboxTransform.position.z;
			
			Transform skybox_far = skyboxTransform.FindChild("skybox_far");
			Transform skybox_near = skyboxTransform.FindChild("skybox_near");
			
			if( skybox_far != null )
			{
				far = new SkyboxItem( skybox_far );
			}
			
			if( skybox_near != null )
			{
				near = new SkyboxItem( skybox_near );
			}
		}
	}
}

public class SkyboxItem
{
	public double local_p_x;
	
	public double local_p_y;
	
	public double local_p_z;	
	
	public double local_r_x;

	public double local_r_y;
	
	public double local_r_z;

	public double local_s_x;

	public double local_s_y;
	
	public double local_s_z;
	
	
	public SkyboxItem()
	{}
	
	public SkyboxItem( Transform transform )
	{
		this.local_p_x  = transform.localPosition.x;
		
		this.local_p_y  = transform.localPosition.y;
		
		this.local_p_z  = transform.localPosition.z;

		this.local_r_x  = transform.localEulerAngles.x;
		
		this.local_r_y  = transform.localEulerAngles.y;
		
		this.local_r_z  = transform.localEulerAngles.z;
		
		this.local_s_x  = transform.localScale.x;
		
		this.local_s_y  = transform.localScale.y;
		
		this.local_s_z  = transform.localScale.z;
	}
}