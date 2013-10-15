using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Poker;

namespace PokerTests
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

	[TestFixture()]
	public class HandTests
	{
		Hand testHand;
		[SetUp]
		public void InitHand (){
			testHand = new Hand();
		}

		[Test]
		public void test_isFlush_returnsFalseForNonFlush ()
		{
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, 3),
				new Card(Poker.Card.Suit.Spade, 5),
				new Card(Poker.Card.Suit.Heart, 6),
				new Card(Poker.Card.Suit.Club, 8),
				new Card(Poker.Card.Suit.Club, 9)
			};

			Assert.IsFalse(testHand.isFlush());
		}
		[Test]
		public void test_isFlush_returnsTrueForFlush ()
		{
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, 3),
				new Card(Poker.Card.Suit.Club, 5),
				new Card(Poker.Card.Suit.Club, 6),
				new Card(Poker.Card.Suit.Club, 8),
				new Card(Poker.Card.Suit.Club, 9)
			};

			Assert.IsTrue(testHand.isFlush());
		}
		[Test]
		public void test_isStraight_returnsFalseIfAnyPairsExist ()
		{
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, 3),
				new Card(Poker.Card.Suit.Heart, 3),
				new Card(Poker.Card.Suit.Club, 6),
				new Card(Poker.Card.Suit.Club, 8),
				new Card(Poker.Card.Suit.Club, 9)
			};

			Assert.IsFalse(testHand.isStraight());
		}

		[Test]
		public void test_isStraight_returnsFalseIfNoPairsButLowCardIsMoreThanFourLessThanHigh ()
		{
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, 2),
				new Card(Poker.Card.Suit.Heart, 3),
				new Card(Poker.Card.Suit.Club, 6),
				new Card(Poker.Card.Suit.Club, 8),
				new Card(Poker.Card.Suit.Club, 9)
			};

			Assert.IsFalse(testHand.isStraight());
		}

		[Test]
		public void test_isStraight_returnsTrueIfNoPairsAndHighIsFourMoreThanLow ()
		{
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, 2),
				new Card(Poker.Card.Suit.Heart, 3),
				new Card(Poker.Card.Suit.Club, 4),
				new Card(Poker.Card.Suit.Club, 5),
				new Card(Poker.Card.Suit.Club, 6)
			};

			Assert.IsTrue(testHand.isStraight());
		}

		[Test]
		public void test_isStraight_returnsTrueForAceThroughFive ()
		{
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Two),
				new Card(Poker.Card.Suit.Club, Card.Value.Three),
				new Card(Poker.Card.Suit.Club, Card.Value.Four),
				new Card(Poker.Card.Suit.Club, Card.Value.Five)
			};

			Assert.IsTrue(testHand.isStraight());
		}

		[Test]
		public void test_isFourOfAKind_returnsFalseForLessThanFour(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Club, Card.Value.Three),
				new Card(Poker.Card.Suit.Club, Card.Value.Four),
				new Card(Poker.Card.Suit.Club, Card.Value.Five)
			};

			Assert.IsFalse(testHand.isFourOfAKind());
		}

		[Test]
		public void test_isFourOfAKind_returnsTrueForIWin(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Ace),
				new Card(Poker.Card.Suit.Diamond, Card.Value.Ace),
				new Card(Poker.Card.Suit.Club, Card.Value.Five)
			};

			Assert.IsTrue(testHand.isFourOfAKind());
		}

		[Test]
		public void test_isFourOfAKind_returnsTrueForIWin_lessThenFiveCards(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Ace),
				new Card(Poker.Card.Suit.Diamond, Card.Value.Ace),
			};

			Assert.IsTrue(testHand.isFourOfAKind());
		}

		[Test]
		public void test_isFourOfAKind_returnsTrueWhenCardsAreNotInOrder(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Club, Card.Value.Five),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Ace),
				new Card(Poker.Card.Suit.Diamond, Card.Value.Ace)
			};

			Assert.IsTrue(testHand.isFourOfAKind());
		}

		[Test]
		public void test_isFullHouse_returnsFalseForFoutOfAKind(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Ace),
				new Card(Poker.Card.Suit.Diamond, Card.Value.Ace),
				new Card(Poker.Card.Suit.Club, Card.Value.Five)
			};

			Assert.IsFalse(testHand.isFullHouse());
		}

		[Test]
		public void test_isFullHouse_returnsFalseForTwoPair(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.King),
				new Card(Poker.Card.Suit.Diamond, Card.Value.King),
				new Card(Poker.Card.Suit.Club, Card.Value.Five)
			};

			Assert.IsFalse(testHand.isFullHouse());
		}

		[Test]
		public void test_isFullHouse_returnsTrueForIWin(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.King),
				new Card(Poker.Card.Suit.Diamond, Card.Value.King),
				new Card(Poker.Card.Suit.Club, Card.Value.King)
			};

			Assert.IsTrue(testHand.isFullHouse());
		}

		[Test]
		public void test_isThreeOfAKind_returnsFalseForTwoPair(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.King),
				new Card(Poker.Card.Suit.Diamond, Card.Value.King),
				new Card(Poker.Card.Suit.Club, Card.Value.Three)
			};
			Assert.IsFalse(testHand.isThreeOfAKind());
		}

		[Test]
		public void test_isThreeOfAKind_returnsFalseOnePair(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.King),
				new Card(Poker.Card.Suit.Diamond, Card.Value.Four),
				new Card(Poker.Card.Suit.Club, Card.Value.Three)
			};
			Assert.IsFalse(testHand.isThreeOfAKind());
		}
		[Test]
		public void test_isThreeOfAKind_returnsTrueForThree(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Ace),
				new Card(Poker.Card.Suit.Diamond, Card.Value.Four),
				new Card(Poker.Card.Suit.Club, Card.Value.Three)
			};
			Assert.IsTrue(testHand.isThreeOfAKind());
		}

		[Test]
		public void test_isThreeOfAKind_returnsFalseForFourOfAKind(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Ace),
				new Card(Poker.Card.Suit.Diamond, Card.Value.Ace),
				new Card(Poker.Card.Suit.Club, Card.Value.Three)
			};
			Assert.IsFalse(testHand.isThreeOfAKind());
		}

		[Test]
		public void test_isThreeOfAKind_returnsFalseForFullHouse(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.King),
				new Card(Poker.Card.Suit.Diamond, Card.Value.King),
				new Card(Poker.Card.Suit.Club, Card.Value.King)
			};

			Assert.IsFalse(testHand.isThreeOfAKind());
		}

		[Test]
		public void test_isThreeOfAKind_returnsTrueForThreeCards(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Ace),
			};
			Assert.IsTrue(testHand.isThreeOfAKind());
		}

		[Test]
		public void test_isTwoPair_returnsFalseForThreeCards(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Ace),
			};
			Assert.IsFalse(testHand.isTwoPair());
		}

		[Test]
		public void test_isTwoPair_returnsFalseFourOfAKind(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Ace)
			};

			Assert.IsFalse(testHand.isTwoPair());
		}
		[Test]
		public void test_isTwoPair_returnsFalseForFullHouse(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.King),
				new Card(Poker.Card.Suit.Club, Card.Value.King)
			};

			Assert.IsFalse(testHand.isTwoPair());
		}
		[Test]
		public void test_isTwoPair_returnsFalseForOnePair(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Three),
				new Card(Poker.Card.Suit.Spade, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Two),
				new Card(Poker.Card.Suit.Club, Card.Value.Ten)
			};

			Assert.IsFalse(testHand.isTwoPair());
		}
		[Test]
		public void test_isTwoPair_returnsFalseForNoPairs(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Three),
				new Card(Poker.Card.Suit.Spade, Card.Value.Five),
				new Card(Poker.Card.Suit.Spade, Card.Value.Two),
				new Card(Poker.Card.Suit.Club, Card.Value.Ten)
			};

			Assert.IsFalse(testHand.isTwoPair());
		}

		[Test]
		public void test_isTwoPair_returnsTrueForTwoPair(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Five),
				new Card(Poker.Card.Suit.Spade, Card.Value.Five),
				new Card(Poker.Card.Suit.Club, Card.Value.Ten)
			};

			Assert.IsTrue(testHand.isTwoPair());
		}

		[Test]
		public void test_isOnePair_returnsFalseForTwoPair(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Five),
				new Card(Poker.Card.Suit.Spade, Card.Value.Five),
				new Card(Poker.Card.Suit.Club, Card.Value.Ten)
			};

			Assert.IsFalse(testHand.isOnePair());
		}

		[Test]
		public void test_isOnePair_returnsFalseForThreeOfAKind(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Five),
				new Card(Poker.Card.Suit.Club, Card.Value.Ten)
			};

			Assert.IsFalse(testHand.isOnePair());
		}
		[Test]
		public void test_isOnePair_returnsFalseForFourOfAKind(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Ace),
				new Card(Poker.Card.Suit.Diamond, Card.Value.Ace),
				new Card(Poker.Card.Suit.Club, Card.Value.Ten)
			};

			Assert.IsFalse(testHand.isOnePair());
		}
		[Test]
		public void test_isOnePair_returnsTrueForOnePair(){
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Nine),
				new Card(Poker.Card.Suit.Diamond, Card.Value.Six),
				new Card(Poker.Card.Suit.Club, Card.Value.Ten)
			};

			Assert.IsTrue(testHand.isOnePair());
		}

		[Test]
		public void test_getHandScore_returns8Point875_forRoyalFlush()
		{
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Club, Card.Value.King),
				new Card(Poker.Card.Suit.Club, Card.Value.Queen),
				new Card(Poker.Card.Suit.Club, Card.Value.Jack),
				new Card(Poker.Card.Suit.Club, Card.Value.Ten)
			};
			Assert.AreEqual(testHand.getHandScore(), 8.875);
		}

		[Test]
		public void test_getHandScore_returns5Point5_forEightHighFlush()
		{
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Eight),
				new Card(Poker.Card.Suit.Club, Card.Value.Five),
				new Card(Poker.Card.Suit.Club, Card.Value.Three),
				new Card(Poker.Card.Suit.Club, Card.Value.Two),
				new Card(Poker.Card.Suit.Club, Card.Value.Six)
			};
			Assert.AreEqual(testHand.getHandScore(), 5.5);
		}

		[Test]
		public void test_getHandScore_returns7Point875_forFourAces()
		{
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Diamond, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.Ace),
				new Card(Poker.Card.Suit.Club, Card.Value.Ten)
			};
			Assert.AreEqual(testHand.getHandScore(), 7.8750625);
		}

		[Test]
		public void test_getHandScore_returns6Point8125_forKingsFullOfAnything()
		{
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.King),
				new Card(Poker.Card.Suit.Heart, Card.Value.King),
				new Card(Poker.Card.Suit.Diamond, Card.Value.King),
				new Card(Poker.Card.Suit.Spade, Card.Value.Two),
				new Card(Poker.Card.Suit.Club, Card.Value.Two)
			};
			Assert.AreEqual(testHand.getHandScore(), 6.8125125);
		}

		[Test]
		public void test_getHandScore_returns4Point75_forQueenHighStraight()
		{
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Queen),
				new Card(Poker.Card.Suit.Club, Card.Value.Jack),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ten),
				new Card(Poker.Card.Suit.Diamond, Card.Value.Nine),
				new Card(Poker.Card.Suit.Spade, Card.Value.Eight)
			};
			Assert.AreEqual(testHand.getHandScore(), 4.75);
		}

		[Test]
		public void test_getHandScore_returns3PointSomething_forAnyThreeOfAKind()
		{
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Seven),
				new Card(Poker.Card.Suit.Spade, Card.Value.Seven),
				new Card(Poker.Card.Suit.Heart, Card.Value.Seven),
				new Card(Poker.Card.Suit.Diamond, Card.Value.Nine),
				new Card(Poker.Card.Suit.Spade, Card.Value.Eight)
			};
			Assert.AreEqual(testHand.getHandScore(), 3.43755625);
		}
		[Test]
		public void test_getHandScore_returns2PointSomething_forTwoPair()
		{
			testHand.cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Eight),
				new Card(Poker.Card.Suit.Spade, Card.Value.Eight),
				new Card(Poker.Card.Suit.Heart, Card.Value.Six),
				new Card(Poker.Card.Suit.Diamond, Card.Value.Six),
				new Card(Poker.Card.Suit.Spade, Card.Value.Five)
			};
			Assert.Greater(testHand.getHandScore(), 2.50003750312);
			Assert.Less(testHand.getHandScore(), 2.500037503129);
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

