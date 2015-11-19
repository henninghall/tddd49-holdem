using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tddd49_holdem
{
	public enum DrawType { HighCards, OnePair, TwoPairs, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraigtFlush};

    public class RulesEngine
    {
        private readonly Table _table;

        public static readonly List<int> CardsBeforeRound = new List<int>{0, 3, 1, 1};
        public const int StartingChips = 1000;
        public const int CardsOnHand = 2;


        public RulesEngine(Table table)
        {
            _table = table;
        }

        public bool IsFoldValid(Player player)
        {
            // fold always ok
            return true;
        }

        public bool IsCheckValid(Player player)
        {
            return (_table.GetHighestBetPlayers().Contains(player));
        }

        public bool IsCallValid(Player player)
        {
             // not valid if player has highest bet already
            if (_table.GetHighestBetPlayers().Contains(player)) return false;
            // must have enough chips
            return _table.GetHighestBet() <= (player.CurrentBet + player.ChipsOnHand);
        }

        public bool IsRaiseValid(Player player) {
            // must have enough chips
            return (player.CurrentBet + player.ChipsOnHand) > _table.GetHighestBet();
        }

        public HashSet<Player> GetBestDrawPlayers(HashSet<Player> players){

			HashSet<Player> currentBestDrawPlayers = new HashSet<Player>();
			DrawType currentBestDrawType = DrawType.HighCards;

		    // Get best draw types by comparing all players draws.
			foreach (Player player in players) {
			    Draw currentDraw = new Draw (player.GetAllCards(), this);

			    // if same draw type
				if (currentDraw.Type == currentBestDrawType) {
					currentBestDrawPlayers.Add(player);
				} 
				// if current draw type is better than so far found
				else if (currentDraw.Type > currentBestDrawType) {
					currentBestDrawPlayers.Clear();
					currentBestDrawPlayers.Add(player);
					currentBestDrawType = currentDraw.Type;
				}
			}
		    // TODO: Compare draws with same draw type

			return currentBestDrawPlayers;
		}


		// TODO: Not implemented correctly
		public void GetDraw(Cards allCards, out DrawType drawtype, out Cards cardsInDraw){
			CardCounter valueCounter = new CardCounter(15);
			CardCounter colorCounter = new CardCounter(4);
			int straightLenght;
			int straightStartsAt;

			foreach (Card card in allCards) {
				valueCounter[card.Value].Add(card);
				colorCounter[card.Color].Add(card);
			}

			GetLongestStraight(valueCounter, out straightLenght, out straightStartsAt);

			// remove
			cardsInDraw = new Cards();

			// Flush or Straight Flush
			if (colorCounter.Max () >= 5) {

				// Straight Flush
				if (straightLenght >= 5) {
					drawtype = DrawType.StraigtFlush;
					cardsInDraw = valueCounter.GetStraight (5, straightStartsAt); 
				}
				// Flush
				else {
					drawtype = DrawType.Flush;
                    Console.WriteLine("colorMax: " + colorCounter.Max());
				    cardsInDraw = colorCounter[colorCounter.HighestMaxIndex()];
				}
			}

			// Straight 
			else if (straightLenght >= 5) {
				cardsInDraw.AddRange(valueCounter.GetStraight (5, straightStartsAt)); 
				drawtype = DrawType.Straight;
			}
			// FourOfAKind
			else switch (valueCounter.Max ()) {
			    case 4:
			        drawtype = DrawType.FourOfAKind;
			        cardsInDraw.AddRange(valueCounter.GetHighestFourOfAKind());
			        cardsInDraw.AddRange (valueCounter.GetHighestSingelCards (1));
			        break;

            // TreeOfAKind / FullHouse 
			    case 3:
			        cardsInDraw.AddRange(valueCounter.GetHighestThreeOfAKind ());

			        if (valueCounter.Contains (2)) {
			            drawtype = DrawType.FullHouse;
			            cardsInDraw.AddRange (valueCounter.GetHighestPair ());
			        } else {
			            drawtype = DrawType.ThreeOfAKind;
			            cardsInDraw.AddRange (valueCounter.GetHighestSingelCards (2));
			        }
			        break;
            // TwoPair / Pair
			    case 2:
			        cardsInDraw.AddRange(valueCounter.GetHighestPair ());

			        if (valueCounter.NumberOfCardsOfSize (2) == 2) {
			            drawtype = DrawType.TwoPairs;
			            cardsInDraw.AddRange (valueCounter.GetSecondHighestPair ());
			            cardsInDraw.AddRange (valueCounter.GetHighestSingelCards (1));
			        } else {
			            drawtype = DrawType.OnePair;
			            cardsInDraw.AddRange (valueCounter.GetHighestSingelCards (3));
			        }
			        break;
            // HighCards
			    default:
			        drawtype = DrawType.HighCards;
			        cardsInDraw.AddRange(valueCounter.GetHighestSingelCards(5));
			        break;
			}

			Console.WriteLine(valueCounter);
			Console.WriteLine(colorCounter);
		}

		private void GetLongestStraight(CardCounter valueCounter, out int straightLenght, out int straightStartsAt){
			int longestStraight = 0;
			int longestStraightStartsAt = 0;
			int currentStraigt = 0;
			int index = 0;
			foreach (Cards cards in valueCounter) {
				if (cards.Any()) {
					currentStraigt++;
					if (currentStraigt > longestStraight) {
						longestStraight = currentStraigt; 
						longestStraightStartsAt = index - currentStraigt + 1;
					}
				}
				else currentStraigt = 0; 
				index++;
			}
			straightLenght = longestStraight;
			straightStartsAt = longestStraightStartsAt;
		}


    }
}
