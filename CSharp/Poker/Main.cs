using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker
{
	public class PokerGame
	{
		public static void Main ()
		{

			List<Card> cards = new List<Card>{
				new Card(Poker.Card.Suit.Club, Card.Value.Ace),
				new Card(Poker.Card.Suit.Heart, Card.Value.Ace),
				new Card(Poker.Card.Suit.Spade, Card.Value.King),
				new Card(Poker.Card.Suit.Diamond, Card.Value.King),
				new Card(Poker.Card.Suit.Club, Card.Value.Five)
			};
			var thing = cards.GroupBy(cv=>cv.value).Select(g => new { g.Key, Count=g.Count() });

			Console.WriteLine(thing);

			List<IChinesePokerPlayer> players = new List<IChinesePokerPlayer> ();
			players.Add (new DumbBot ("Dave"));
			players.Add (new DumbBot ("Eric"));
			players.Add (new DumbBot ("Stephen"));
			players.Add (new DumbBot ("Dan"));
			ChineseOpenFaceDealer dealer = new ChineseOpenFaceDealer (players);
			while (! dealer.GameOver) {
				dealer.Deal ();
			}
			ChinesePokerTable table = dealer.getState ();
			foreach (Opponent player in table.getPlayers()) {
				Console.WriteLine("Player Hands: "+player.name);
				foreach(Hand hand in player.hands){
					foreach(Card cd in hand.cards){
						Console.Write(cd.suit+" "+cd.value+", ");
					}
					Console.WriteLine();
				}
			}


		}
	}
	public class DumbBot : IChinesePokerPlayer
	{
		int m_position;
		int nextHand;
		List<Hand> hands;
		public String myName;

		public DumbBot(){
			hands = new List<Hand>();
			Hand me = new Hand();
			me.cards=new List<Card>();
			hands.Add(me);
			Hand me1 = new Hand();
			me1.cards=new List<Card>();
			hands.Add(me1);
			Hand me2 = new Hand();
			me2.cards=new List<Card>();
			hands.Add(me2);
			nextHand=0;
		}

		public DumbBot(String name){
			myName = name;
			hands = new List<Hand>();
			Hand me = new Hand();
			me.cards=new List<Card>();
			hands.Add(me);
			Hand me1 = new Hand();
			me1.cards=new List<Card>();
			hands.Add(me1);
			Hand me2 = new Hand();
			me2.cards=new List<Card>();
			hands.Add(me2);
			nextHand=0;
		}

		public DumbBot (int position)
		{
			m_position = position;
		}
		public List<Hand> layoutFirstHand (List<Card> cards, ChinesePokerTable currentState)
		{
			foreach (Card next in cards) {
				this.takeAction (new List<Card> (){next}, currentState);
			}
			return hands;
		}

		public String getName ()
		{
			return myName;
		}

		public Move takeAction (List<Card> cards, Table currentState)
		{
			Console.WriteLine(myName + " is adding card to "+nextHand);
			var nextMove = new ChineseMove();
			nextMove.myCard = cards[0];
			nextMove.rowNumber = nextHand;
			hands[nextHand].cards.Add (cards[0]);
			if(hands[nextHand].cards.Count == 5){
				Console.WriteLine("Hand has 5 cards");
				foreach(Card cd in hands[nextHand].cards){
					Console.Write(cd.suit+" "+cd.value+", ");
				}
				Console.WriteLine(" and is full");
				nextHand++;
			}

			return nextMove;
		}

		public void setPosition(int position)
		{
			m_position = position;
		}
		public int getPosition ()
		{
			return m_position;
		}
	}
}
