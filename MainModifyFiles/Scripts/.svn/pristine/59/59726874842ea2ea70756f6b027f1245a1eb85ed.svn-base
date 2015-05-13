// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  Range.cs
// Author   : willson
// Created  : 2013/2/22 
// Porpuse  : 
// **********************************************************************
using System;

public class Range
{
	private float m_Floor;
	private float m_Ceil;
	
	public Range (float floor,float ceil)
	{
		m_Floor = floor;
		m_Ceil = ceil;
	}
	
	public float getFloor()
	{
		return m_Floor;
	}
	
	public void setFloor(float f)
	{
		m_Floor = f;
	}
	
	public float getCeil()
	{
		return m_Ceil;
	}
	
	public void setCeil(float c)
	{
		m_Ceil = c;
	}
	
	public void move(float s)
	{
		m_Floor += s;
		m_Ceil += s;
	}
	
	public float start
	{
		get
		{
			return getFloor();
		}
		
		set
		{
			setFloor(value);
		}
	}
	
	public float end
	{
		get
		{
			return getCeil();
		}
		
		set
		{
			setCeil(value);
		}
	}
}


