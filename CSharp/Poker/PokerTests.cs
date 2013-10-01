using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace Poker
{
	[TestFixture()]
	public class DeckTests
	{
		[Test()]
		public void aDeckStartsWith52Cards ()
		{
			Deck testDeck = new Deck();
			Assert.AreEqual(testDeck.getStackSize(), 52);
		}
		[Test()]
		public void getCardReturnsACard ()
		{
			Deck testDeck = new Deck();
			Assert.IsInstanceOf<Card>(testDeck.getCard());
		}
		[Test()]
		public void stackSizeDecreasesWhenACardIsDealt ()
		{
			Deck testDeck = new Deck();
			Assert.AreEqual(testDeck.getStackSize(), 52);
			testDeck.getCard();
			Assert.AreEqual(testDeck.getStackSize(), 51);
		}
		[Test()]
		public void deckHasOnefEachCard ()
		{
			Dictionary<Card.Suit,List<Card>> cardsBySuit = new Dictionary<Card.Suit, List<Card>>();
			Deck testDeck = new Deck ();
			while (testDeck.getStackSize() > 0) {
				Card next = testDeck.getCard();
				if(! cardsBySuit.ContainsKey(next.suit)){
					cardsBySuit[next.suit] = new List<Card>();
				}
				cardsBySuit[next.suit].Add(next);
			}
			Assert.AreEqual(cardsBySuit.Keys.Count, 4);
			Assert.AreEqual (cardsBySuit[Card.Suit.Club].Count, 13);
			Assert.AreEqual (cardsBySuit[Card.Suit.Diamond].Count, 13);
			Assert.AreEqual (cardsBySuit[Card.Suit.Heart].Count, 13);
			Assert.AreEqual (cardsBySuit[Card.Suit.Spade].Count, 13);
		}

		[Test()]
		public void testDeckKnowsItsOwnCards ()
		{
			Deck testDeck = new Deck();

			Card looseCard = new Card(Card.Suit.Club, 2);
			Assert.IsFalse(testDeck.isValidCard(looseCard));
			Card fromDeck = testDeck.getCard();
			Assert.IsTrue(testDeck.isValidCard(fromDeck));
		}
	}
			               
	[TestFixture()]
	public class CardTests
	{
		[Test()]
		public void aCardHasASuite ()
		{
			Card testCard = new Card(Card.Suit.Club, 2);
			Assert.AreEqual(Card.Suit.Club, testCard.suit);
		}

		[Test()]
		public void aCardHasAValue ()
		{
			Card testCard = new Card(Card.Suit.Club, 2);
			Assert.AreEqual((Card.Value)2, testCard.value);
		}

		[Test()]
		public void isFaceCardReturnsTrueForJackQueenAndKing ()
		{
			Card jack = new Card(Card.Suit.Club, 11);
			Assert.True(jack.isFaceCard());
			Card queen = new Card(Card.Suit.Club, 12);
			Assert.True(queen.isFaceCard());
			Card king = new Card(Card.Suit.Club, 13);
			Assert.True(king.isFaceCard());
		}

		[Test()]
		public void isFaceCardReturnsFalseForOthers ()
		{
			Card ten = new Card(Card.Suit.Club, 10);
			Assert.False(ten.isFaceCard());
			Card two = new Card(Card.Suit.Club, 2);
			Assert.False(two.isFaceCard());
			Card ace = new Card(Card.Suit.Club, 14);
			Assert.False(ace.isFaceCard());
		}
	}
}

