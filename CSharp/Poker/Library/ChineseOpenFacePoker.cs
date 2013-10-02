using System;
using System.Collections.Generic;

namespace Poker
{
	public class ChineseOpenFaceDealer : Dealer
	{
		public ChineseOpenFaceDealer (List<Player> players) : base(players)
		{

		}

		public override void Deal(){
			foreach(Player player in players){
				player.placeCards(myDeck.getCards(5), new Table());
			}
		}
	}

	public class ChineseMove : Move
	{
		public int rowNumber;
	}
}

