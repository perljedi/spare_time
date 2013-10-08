using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker
{
	public class ChineseOpenFaceDealer : Dealer
	{
		private List<Opponent> m_opponents;
		int round;

		public ChineseOpenFaceDealer (List<IChinesePokerPlayer> players) : base(players.ConvertAll(x => (IPlayer)x))
		{
			m_opponents = new List<Opponent>();
			int pos=1;
			round = 0;
			foreach (IPlayer play in players) {
				play.setPosition(pos);
				Opponent next = new Opponent();
				next.position = pos++;
				next.hands = new List<Hand>();
				m_opponents.Add (next);
			}
		}

		public override void Deal ()
		{
			if (round == 0) {
				foreach (IChinesePokerPlayer player in players) {
					int pos = player.getPosition ();
					List<Hand> hands = player.layoutFirstHand (myDeck.getCards (5), new ChinesePokerTable (m_opponents));
					Opponent next = new Opponent ();
					next.position = pos;
					int ind = pos - 1;
					next.hands = hands;
					next.name = player.getName();
					Console.WriteLine ("Player opponent at " + pos);
					Console.WriteLine ("Player opponent at " + m_opponents [ind]);
					m_opponents [ind] = next;
				}
			} else {
				foreach (IPlayer player in players) {
					ChinesePokerTable tab = new ChinesePokerTable (m_opponents);
//					ChineseMove player_move = (ChineseMove)
				    player.takeAction (new List<Card> (){ myDeck.getCard() }, (Table)tab);
//					int pos = player.getPosition ();
//					int ind = pos-1;
//					m_opponents [ind].hands [player_move.rowNumber].cards.Add (player_move.myCard);
				}
			}
			if (myDeck.getStackSize() < players.Count) {
				GameOver = true;
			}
			round++;
		}

		public ChinesePokerTable getState ()
		{
			return new ChinesePokerTable (m_opponents);
		}
	}

	public class ChinesePokerTable : Table
	{
		private List<Opponent> m_players;
		public ChinesePokerTable (List<Opponent> players)
		{
			m_players = players;
		}
		public List<Opponent> getPlayers(){
			return m_players;
		}
		public List<Card> getCommunityCards ()
		{
			return new List<Card>();
		}
		public long getPot ()
		{
			return 0;
		}
	}

	public interface IChinesePokerPlayer : IPlayer 
	{
		List<Hand> layoutFirstHand(List<Card> cards, ChinesePokerTable currentState);
		String getName();
	}

	public class ChineseMove : Move
	{
		public int rowNumber;
	}
}

