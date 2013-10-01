using System;

namespace Poker
{
	using System.Collections.Generic;
	using System.Linq;
	public enum Suit
	{
		Club,
		Diamond,
		Heart,
		Spade
	}

	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");
		}
	}

	class Deck
	{
		private List<Card> cards;
		private List<Card> stack;

		public Deck ()
		{
			cards = new List<Card>();
			stack = new List<Card>();
			foreach (int sval in Enum.GetValues(typeof(Suit))) {
				for (int i=2; i<15; i++) {
					Card newCard = new Card ((Suit)sval, i);
					cards.Add(newCard);
					stack.Add(newCard);
				}
			}
		}

		public void shuffle ()
		{

		}

		public Card getCard ()
		{
			Card dealt = stack[0];
			stack.Remove(dealt);
			return dealt;
		}
		public int getStackSize ()
		{
			return stack.Count;
		}

		public bool isValidCard (Card card)
		{
			return cards.Contains(card);
		}

	}
	class Card
	{

		private Suit m_suit;
		private int m_value;

		public Card (Suit suit, int val)
		{
			m_suit = suit;
			m_value = val;
		}
		public Suit suit {
			get {
				return m_suit;
			}
		}
		public int value {
			get {
				return m_value;
			}
		}
		public bool isFaceCard ()
		{
			if (m_value > 10 && m_value < 14) 
			{
				return true;
			}
			return false;
		}

	}

}