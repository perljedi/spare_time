using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Poker
{
	public enum Action
	{
		Fold,
		Check,
		Call,
		Bet,
		Raise
	}
	public class Deck
	{
		private List<Card> cards;
		private List<Card> stack;

		public Deck ()
		{
			cards = new List<Card>();
			stack = new List<Card>();
			foreach (int sval in Enum.GetValues(typeof(Card.Suit))) {
				for (int i=2; i<15; i++) {
					Card newCard = new Card ((Card.Suit)sval, i);
					cards.Add(newCard);
					stack.Add(newCard);
				}
			}
		}

		public void shuffle ()
		{
 			Random rnd = new Random();
    		stack = stack.OrderBy<Card, int>((item) => rnd.Next()).ToList();
		}	

		public Card getCard ()
		{
			if (stack.Count < 1) {
				throw new OutOfCardsException ();
			}
			Card dealt = stack[0];
			stack.Remove(dealt);
			return dealt;
		}

		public List<Card> getCards (int number)
		{
			List<Card> myCards = new List<Card> ();
			int i = 0;
			while (i++ < number && stack.Count > 0) {
				myCards.Add (this.getCard());
			}
			return myCards;
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

	public class OutOfCardsException : Exception
	{

	}
	public class Card
	{
		public enum Suit
		{
			Club,
			Diamond,
			Heart,
			Spade
		}

		public enum Value {
			Two=2,
			Three=3,
			Four=4,
			Five=5,
			Six=6,
			Seven=7,
			Eight=8,
			Nine=9,
			Ten=10,
			Jack=11,
			Queen=12,
			King=13,
			Ace=14
		}

		private Suit m_suit;
		private int m_value;

		public Card (Suit suit, int val)
		{
			m_suit = suit;
			m_value = val;
		}
		public Card (Suit suit, Value val)
		{
			m_suit = suit;
			m_value = (int)val;
		}
		public Suit suit {
			get {
				return m_suit;
			}
		}
		public Value value {
			get {
				return (Value)m_value;
			}
		}
		public int IntValue {
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

	public class Move
	{
		public long bet;
		public Action action;
		public Card myCard;
	}

	public struct Opponent
	{
		String name;
		int position;
		int chips;
		Action lastAction;
	}

	public struct Hand
	{
		public List<Card> cards;
	}

	public interface Table
	{
		List<Opponent> getPlayers();
		List<Card> getCommunityCards();
		long getPot();
	}

	public interface Player
	{
		Opponent getPublicState();
		List<Move> placeCards(List<Card> cards, Table gameState);
		void setPosition(int position);
	}

	public abstract class Dealer
	{
		protected Deck myDeck;
		protected List<Player> players;
		protected Boolean gameOver;


		public Dealer (List<Player> player_list)
		{
			myDeck = new Deck();
			myDeck.shuffle();
			gameOver=false;
			players = player_list;
		}

		public Dealer ()
		{
			myDeck = new Deck();
			myDeck.shuffle();
			gameOver=false;
		}

		public List<Player> Players {
			get {
				return players;
			}
		}

		public Boolean GameOver {
			get {
				return gameOver;
			}
		}

		public abstract void Deal();
	}
}