// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  Summary.cs
// Author   : willson
// Created  : 2013/2/22 
// Porpuse  : 
// **********************************************************************
using System;

public class Summary
{
	private int m_iTotal = 0;
	private int m_iTotalPage = 0;
	private int m_iCurrentPage = 0;
	private int m_iPageSize = 0;
	
	public Summary ()
	{
	}
	
	public static Summary create(int iTotal,int iTotalPage,int iCurrentPage,int iPageSize) {
		Summary r = new Summary();
		
		r.m_iTotal = iTotal;
		r.m_iTotalPage = iTotalPage;
		r.m_iCurrentPage = iCurrentPage;
		r.m_iPageSize = iPageSize;
		
		return r;
	}
	
	public int getTotal(){
		return m_iTotal;
	}
	
	public void setTotal(int total){
		m_iTotal = total;
		float iTotal = m_iTotal;
		float iPageSize = m_iPageSize;
		m_iTotalPage = (int)Math.Ceiling(iTotal/iPageSize);
	}
	
	public int getTotalPage(){
		return m_iTotalPage;
	}
	
	public void setTotalPage(int t) {
		m_iTotalPage = t;
	}
	
	public bool isFirstPage() {
		return m_iCurrentPage <= 1;
	}
	
	public bool isLastPage() {
		return m_iCurrentPage >= m_iTotalPage;
	}
	
	public int getCurrentPage(){
		return m_iCurrentPage;
	}
	
	public int getPrevPage(){
		return m_iCurrentPage - 1;
	}
	
	public int getNextPage(){
		return m_iCurrentPage + 1;
	}
	
	public void setCurrentPage(int page){
		m_iCurrentPage = page;
	}
	
	public int getPageSize() {
		return m_iPageSize;
	}
	
	public void setPageSize(int ps) {
		m_iPageSize = ps;
	}
	
	public bool equal(Summary t) {
		return m_iCurrentPage == t.m_iCurrentPage && m_iPageSize == t.m_iPageSize && m_iTotal == t.m_iTotal && m_iTotalPage == t.m_iTotalPage;
	}
	
	public Range getRange() {
		int f = (m_iCurrentPage - 1) * m_iPageSize;
		int c = m_iCurrentPage * m_iPageSize;
		c = c < m_iTotal ? c : m_iTotal;
		--c;
		return new Range(f, c);
	}
	
	public void destroy() {
		
	}
}

