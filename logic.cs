﻿
using System;

namespace Snake
{

	//polymorphism 
    
	public class Node: IDisposable
	{
		public Node(int number)
		{
			value = number;
			previous = null;
			next = null;
		}
		
		public void Dispose() {
			
			GC.SuppressFinalize(this);
			
		}
		
		
		public int value;
		public Node previous;
		public Node next;
		
		
	}
	
	public class Logic
	{
		public Logic()
		{
			length = 0;
		}
		
		public Node last;
		public Node first;
		public int length;
		
		
		
		public void Add_Node(int number)
		{
			if(last == null)
			{
				Node nd = new Node(number);
				last = nd;
				last.next = null;
				last.previous = null;
				first = last;
			}
			else
			{
				Node nd = new Node(number);
				last.next = nd;
				nd.previous = last;
				nd.next = null;
				last = nd;
			}
			length++;
		}
		
		public int Get_The_Last_Node_Value()
		{
			return last.value;
		}
		
		public int Get_The_First_Node_Value()
		{
			return first.value;
		}
		
		public void Delete_First_Node()
		{
			if(first!=last) {
				Node temp = first;
				first = first.next;
				first.previous = null;
				temp.Dispose();
				length--;
			}
		}
	}
}
