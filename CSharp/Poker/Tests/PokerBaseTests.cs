using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Poker
{
	[TestFixture()]
	public class DeckTests
	{
		Deck testDeck;
		[SetUp]
		public void Init ()
		{
			testDeck = new Deck();
		}

		[Test()]
		public void aDeckStartsWith52Cards ()
		{
			Assert.AreEqual(testDeck.getStackSize(), 52);
		}
		[Test()]
		public void getCardReturnsACard ()
		{
			Assert.IsInstanceOf<Card>(testDeck.getCard());
		}

		[Test()]
		public void stackSizeDecreasesWhenACardIsDealt ()
		{
			Assert.AreEqual(testDeck.getStackSize(), 52);
			testDeck.getCard();
			Assert.AreEqual(testDeck.getStackSize(), 51);
		}

		[Test()]
		public void getCardsReturnsANumberOfCardsRequested()
		{
			List<Card> cards = testDeck.getCards(5);
			Assert.AreEqual(cards.Count, 5);
		}
		[Test()]
		public void deckHasOnefEachCard ()
		{
			Dictionary<Card.Suit,List<Card>> cardsBySuit = new Dictionary<Card.Suit, List<Card>>();
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
			Card looseCard = new Card(Card.Suit.Club, 2);
			Assert.IsFalse(testDeck.isValidCard(looseCard));
			Card fromDeck = testDeck.getCard();
			Assert.IsTrue(testDeck.isValidCard(fromDeck));
		}

		[Test]
		[ExpectedException(typeof(OutOfCardsException))]
		public void testGetCardThrowsExceptionIfStackIsEmpty ()
		{
			while (testDeck.getStackSize() > 0) {
				testDeck.getCard ();
			}
			testDeck.getCard();
		}
	}

	public class TestDealer : Dealer
	{
		public override void Deal(){
			//no-op
		}
	}
			    
	[TestFixture()]
	public class DealerTests
	{
		Dealer testDealer;
		Hand testHand;

		[TestFixtureSetUp]
		public void Init ()
		{
			testDealer = new TestDealer();
		}

		[Test]
		public void test_isFlush_returnsFalseForNonFlush ()
		{
			testHand = new Hand();
			testHand.cards = new List<Card>();
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 3));
			testHand.cards.Add(new Card(Poker.Card.Suit.Spade, 5));
			testHand.cards.Add(new Card(Poker.Card.Suit.Heart, 6));
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 8));
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 9));

			Assert.IsFalse(testDealer.isFlush(testHand));
		}
		[Test]
		public void test_isFlush_returnsTrueForFlush ()
		{
			testHand = new Hand();
			testHand.cards = new List<Card>();
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 3));
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 5));
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 6));
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 8));
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 9));

			Assert.IsTrue(testDealer.isFlush(testHand));
		}
		[Test]
		public void test_isStraight_returnsFalseIfAnyPairsExist ()
		{
			testHand = new Hand();
			testHand.cards = new List<Card>();
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 3));
			testHand.cards.Add(new Card(Poker.Card.Suit.Heart, 3));
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 6));
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 8));
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 9));

			Assert.IsFalse(testDealer.isStraight(testHand));
		}

		[Test]
		public void test_isStraight_returnsFalseIfNoPairsButLowCardIsMoreThanFourLessThanHigh ()
		{
			testHand = new Hand();
			testHand.cards = new List<Card>();
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 2));
			testHand.cards.Add(new Card(Poker.Card.Suit.Heart, 3));
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 6));
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 8));
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 9));

			Assert.IsFalse(testDealer.isStraight(testHand));
		}

		[Test]
		public void test_isStraight_returnsTrueIfNoPairsAndHighIsFourMoreThanLow ()
		{
			testHand = new Hand();
			testHand.cards = new List<Card>();
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 2));
			testHand.cards.Add(new Card(Poker.Card.Suit.Heart, 3));
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 4));
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 5));
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, 6));

			Assert.IsTrue(testDealer.isStraight(testHand));
		}

		[Test]
		public void test_isStraight_returnsTrueForAceThroughFive ()
		{
			testHand = new Hand();
			testHand.cards = new List<Card>();
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, Card.Value.Ace));
			testHand.cards.Add(new Card(Poker.Card.Suit.Heart, Card.Value.Two));
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, Card.Value.Three));
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, Card.Value.Four));
			testHand.cards.Add(new Card(Poker.Card.Suit.Club, Card.Value.Five));

			Assert.IsTrue(testDealer.isStraight(testHand));
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

