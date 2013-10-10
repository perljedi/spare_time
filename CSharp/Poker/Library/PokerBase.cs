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
					Card newCard = new Card (sval, i);
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
			Club=1,
			Diamond=2,
			Heart=3,
			Spade=4
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


		public Card (Suit mSuit, int val)
		{
			suit = mSuit;
			value = (Value)val;
		}
		public Card (Suit mSuit, Value val)
		{
			suit = mSuit;
			value = val;
		}
		public Card (int intSuit, Value val)
		{
			suit = (Suit) intSuit;
			value = val;
		}
		public Card (int intSuit, int val)
		{
			suit = (Suit) intSuit;
			value = (Value)val;
		}
		public Suit suit {
			get; private set;
		}
		public Value value {
			get; protected set;
		}
		public int intValue {
			get {
				return (int)value;
			}
		}	
		public bool isFaceCard ()
		{
			if (intValue > 10 && intValue < 14) 
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
		public String name;
		public int position;
		public int chips;
		public Action lastAction;
		public List<Hand> hands;
	}

	public class Hand
	{
		public List<Card> cards{ 
			get; set; 
		}
		public virtual Boolean isFlush ()
		{
			return cards.Count == 5 && cards.Select(x=>x.suit).Distinct().ToList().Count == 1;
		}
		public virtual bool isStraight ()
		{
			List<Card.Value> values = cards.Select (x => x.value).Distinct ().OrderBy(x=>(int)x).ToList();
			if (values.Count < 5) {
				return false;
			} else {
				if(values[4].Equals(Card.Value.Ace) && values[3].Equals(Card.Value.Five)){
					return true;
				}
				return values[4]-values[0] == 4;
			}
		}
		public Boolean isFourOfAKind ()
		{
			var setsBySize = this.groupByValueAndSortByCount(cards);
			return setsBySize[0].Value == 4;
		}

		public bool isFullHouse ()
		{
			var setsBySize = this.groupByValueAndSortByCount(cards);
			return setsBySize.Count () == 2 && setsBySize[0].Value == 3 && setsBySize[1].Value == 2;
		}

		public bool isThreeOfAKind ()
		{
			if (cards.Count () < 3) {
				return false;
			} else {
				var setsBySize = this.groupByValueAndSortByCount (cards);
				if (setsBySize [0].Value != 3) {
					return false;
				}
				return setsBySize.Count() == 1 || setsBySize [1].Value == 1;
			}
		}

		public bool isTwoPair ()
		{
			if (cards.Count () < 4) {
				return false;
			} else {
				var setsBySize = this.groupByValueAndSortByCount (cards);
				return setsBySize[0].Value == 2 && setsBySize[1].Value == 2;
			}
		}

		public bool isOnePair ()
		{
			if (cards.Count () < 2) {
				return false;
			} else {
				var setsBySize = this.groupByValueAndSortByCount (cards);
				return setsBySize[0].Value == 2 && (setsBySize.Count() == 1 || setsBySize[1].Value == 1);
			}
		}

		protected KeyValuePair<Card.Value,int>[] groupByValueAndSortByCount (List<Card> someCards)
		{
			return someCards.GroupBy(x => x.value).Select(g=> new { g.Key, Count=g.Count () }).OrderByDescending(ob=>ob.Count).ToDictionary(x=>x.Key, x=>x.Count).ToArray ();
		}
	}

	public interface Table
	{
		List<Opponent> getPlayers();
		List<Card> getCommunityCards();
		long getPot();
	}

	public interface IPlayer
	{
		Move takeAction(List<Card> cards, Table gameState);
		void setPosition(int position);
		int getPosition();
	}

	public abstract class Dealer
	{
		protected Deck myDeck;


		public Dealer (List<IPlayer> player_list)
		{
			myDeck = new Deck();
			myDeck.shuffle();
			GameOver=false;
			players = player_list;
		}

		public Dealer ()
		{
			myDeck = new Deck();
			myDeck.shuffle();
			GameOver=false;
		}

		public List<IPlayer> players {
			get; protected set;
		}

		public Boolean GameOver {
			get; protected set;
		}

		public abstract void Deal();
	}
}