using System;
using System.Collections.Generic;

namespace Poker
{
	public class PokerGame
	{
		public static void Main() 
		{
			List<Player> players = new List<Player>();
			ChineseOpenFaceDealer dealer = new ChineseOpenFaceDealer(players);
			dealer.Deal();
		}
	}

	public class DumbBot : Player
	{
		int m_position;
		int nextHand;
		List<Hand> hands;

		public DumbBot(){
			hands = new List<Hand>();
			hands.Add(new Hand());
			hands.Add(new Hand());
			hands.Add(new Hand());
			nextHand=0;
		}

		public DumbBot (int position)
		{
			m_position = position;
		}

		public List<Move> placeCards (List<Card> cards, Table currentState)
		{
			var moves = new List<Move> ();
			foreach (Card next in cards) {
				hands[nextHand].Add(next);
				var nextMove = new ChineseMove();
				moves.Add(nextMove);

				if(hands[nextHand].count == 5){
					nextHand++;
				}
			}
			return moves;
		}
		public Opponent getPublicState()
		{
			return new Opponent();
		}
		public void setPosition(int position)
		{
			m_position = position;
		}
	}
}
