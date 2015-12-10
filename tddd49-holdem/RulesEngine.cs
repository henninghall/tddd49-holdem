using System.Collections.Generic;
using System.Linq;
using tddd49_holdem.counters;
using tddd49_holdem.Exceptions;
using tddd49_holdem.Players;

namespace tddd49_holdem
{
    public enum DrawType { HighCards, OnePair, TwoPairs, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraigtFlush };

    public class RulesEngine
    {
        public static List<int> CardsBeforeRound = new List<int>(new[] { 3, 1, 1 });
        public const int StartingChips = 100;
        public const int CardsOnHand = 2;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="players"></param>
        /// <returns>A set of players with best draw</returns>
        public PlayerList GetBestDrawPlayers(PlayerList players)
        {
            PlayerList currentBestDrawPlayers = new PlayerList();
            Draw bestDrawSoFar = null;

            // Get best draw types by comparing all players draws.
            foreach (Player player in players)
            {
                if (!player.HasCards()) continue;
                Draw currentDraw = GetDraw(player.GetAllCards());
                try {
                    // save the current draw if it is better than the previuosly best found draw
                    if (!GetBestDraw(currentDraw, bestDrawSoFar).Equals(bestDrawSoFar)){
                        currentBestDrawPlayers.Clear();
                        currentBestDrawPlayers.Add(player);
                        bestDrawSoFar = currentDraw;
                    }
                }
                catch (TieDrawException) {
                    currentBestDrawPlayers.Add(player);
                }
            }
            return currentBestDrawPlayers;
        }

        public Draw GetBestDraw(Draw currentDraw, Draw bestDrawSoFar)
        {
            // if no bestDraw has been found
            if (bestDrawSoFar == null) return currentDraw;

            // if current draw type is better than so far found
            if (currentDraw.Type > bestDrawSoFar.Type) return currentDraw;
            if (currentDraw.Type < bestDrawSoFar.Type) return bestDrawSoFar;

            // if same draw type
            if (currentDraw.Type == bestDrawSoFar.Type)
            {
                // Important! Assuming the lists are sorted in a way that the most "valued" cards is first. 
                // Example: In TwoPair the highest pair first, then second highest pair, and last the highest remaining card.
                for (int i = 0; i < currentDraw.Cards.Count; i++)
                {
                    if (currentDraw.Cards[i].Value > bestDrawSoFar.Cards[i].Value) return currentDraw;
                    if (currentDraw.Cards[i].Value < bestDrawSoFar.Cards[i].Value) return bestDrawSoFar;
                }
                throw new TieDrawException();
            }
            return null;
        }

        public DrawType GetDrawType(Cards allCards)
        {
            return GetDraw(allCards).Type;
        }

        /// <summary>
        /// Returns a set with the winner at the current table state. 
        /// Will return all players in case of a Tie. 
        /// </summary>
        public PlayerList GetWinners(Table table)
        {
            // if all players except one folded
            if (table.Players.NumberOfPlayersWithCards() == 1) return table.Players.GetPlayerWithCards();

            // else winner(s) is determined due to rules
            return GetBestDrawPlayers(table.Players);
        }

        /// <summary>
        /// Calculates and returns the draw (Pair, Straight etc) from the submitted cards. 
        /// The number of sumbitted cards are arbitrary .
        /// </summary>
        public Draw GetDraw(Cards allCards) {
            ValueCounter valueCounter = new ValueCounter(allCards);
            ColorCounter colorCounter = new ColorCounter(allCards);
            int straightLenght;
            int straightStartsAt;
            GetLongestStraight(valueCounter, out straightLenght, out straightStartsAt);
            DrawType drawtype;
            Cards cardsInDraw = new Cards();

            // Flush or Straight Flush
            if (colorCounter.Max() >= 5)
            {

                // Straight Flush
                if (straightLenght >= 5)
                {
                    drawtype = DrawType.StraigtFlush;
                    cardsInDraw = valueCounter.GetStraight(5, straightStartsAt);
                }
                // Flush
                else
                {
                    drawtype = DrawType.Flush;
                    cardsInDraw = colorCounter[colorCounter.HighestMaxIndex()];
                }
            }

            // Straight 
            else if (straightLenght >= 5)
            {
                cardsInDraw.AddRange(valueCounter.GetStraight(5, straightStartsAt));
                drawtype = DrawType.Straight;
            }

            else
                switch (valueCounter.Max())
                {
                    // FourOfAKind
                    case 4:
                        drawtype = DrawType.FourOfAKind;
                        cardsInDraw.AddRange(valueCounter.GetHighestFourOfAKind());
                        cardsInDraw.AddRange(valueCounter.GetHighestSingleCards(1));
                        break;

                    // TreeOfAKind / FullHouse 
                    case 3:
                        cardsInDraw.AddRange(valueCounter.GetHighestThreeOfAKind());

                        if (valueCounter.Contains(2))
                        {
                            drawtype = DrawType.FullHouse;
                            cardsInDraw.AddRange(valueCounter.GetHighestPair());
                        }
                        else
                        {
                            drawtype = DrawType.ThreeOfAKind;
                            cardsInDraw.AddRange(valueCounter.GetHighestSingleCards(2));
                        }
                        break;
                    // TwoPair / Pair
                    case 2:
                        cardsInDraw.AddRange(valueCounter.GetHighestPair());

                        // TwoPair
                        if (valueCounter.NumberOfCardsOfSize(2) == 2)
                        {
                            drawtype = DrawType.TwoPairs;
                            cardsInDraw.AddRange(valueCounter.GetSecondHighestPair());
                            cardsInDraw.AddRange(valueCounter.GetHighestSingleCards(1));
                        }
                        // Three pair
                        else if (valueCounter.NumberOfCardsOfSize(2) == 3)
                        {
                            drawtype = DrawType.TwoPairs;
                            cardsInDraw.AddRange(valueCounter.GetSecondHighestPair());
                            
                            // comparing third pair to highest single card
                            Card c1 = valueCounter.GetThirdHighestPair().First();
                            Card c2 = valueCounter.GetHighestSingleCards(1).First();
                            cardsInDraw.Add(c1.Value > c2.Value ? c1 : c2);
                        }
                        // One pair
                        else
                        {
                            drawtype = DrawType.OnePair;
                            cardsInDraw.AddRange(valueCounter.GetHighestSingleCards(3));
                        }
                        break;
                    // HighCards
                    default:
                        drawtype = DrawType.HighCards;
                        cardsInDraw.AddRange(valueCounter.GetHighestSingleCards(5));
                        break;
                }
            return new Draw(cardsInDraw, drawtype);
        }

        /// <summary>
        /// Using a Cardcounter to find the longest straight.
        /// Both the longest straight and straight starting point will be returned in form of out variabels. 
        /// </summary>
        private static void GetLongestStraight(CardCounter valueCounter, out int straightLenght, out int straightStartsAt)
        {
            int longestStraight = 0;
            int longestStraightStartsAt = 0;
            int currentStraigt = 0;
            int index = 0;
            foreach (Cards cards in valueCounter)
            {
                if (cards.Any())
                {
                    currentStraigt++;
                    if (currentStraigt > longestStraight)
                    {
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
