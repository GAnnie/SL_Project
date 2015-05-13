using UnityEngine;
using System.Collections;

public class uvAnimation : MonoBehaviour
{
	public int column;
	public int line;
	public float framePerSecond = 0.5f;
	public float delayTime = 0.0f;
	
	public int loopTime = 1;
	
	private int currentColumn = 0;
	private int currentLine = 0;
	
	
	void Start()
	{
		StartCoroutine( StartUVAnimation() );
	}
	
	IEnumerator StartUVAnimation()
	{
		yield return new WaitForSeconds( delayTime );
		
		for ( int i = 0; i < loopTime; ++i )
		{
			while ( true )
			{
				float offsetX = currentColumn / (float)column;
				float offsetY = currentLine / (float)line;
	        	renderer.material.mainTextureOffset = new Vector2(offsetX, offsetY);
				
				yield return new WaitForSeconds( framePerSecond );
				
				++currentColumn;
				
				if ( currentColumn >= column )
				{
					currentColumn = 0;
					++currentLine;
					if ( currentLine >= line )
						break;
				}
			}
		}
		
		yield return null;
	}
}
