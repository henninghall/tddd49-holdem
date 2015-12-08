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
        public static NumberOfCardsList CardsBeforeRound = new NumberOfCardsList(new[] { 3, 1, 1 });
        public const int StartingChips = 100;
        public const int CardsOnHand = 2;
        
        public HashSet<Player> GetBestDrawPlayers(HashSet<Player> players)
        {
            HashSet<Player> currentBestDrawPlayers = new HashSet<Player>();
            Draw bestDrawSoFar = null;

            // Get best draw types by comparing all players draws.
            foreach (Player player in players)
            {
                Draw currentDraw = GetDraw(player.GetAllCards());

                try {
                    // continue with next player if a better draw than current draw has been found
                    if (GetBestDraw(currentDraw, bestDrawSoFar).Equals(bestDrawSoFar)) continue;
                    else {
                        // the current draw is the best found so far.
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

        public Draw GetDraw(Cards allCards) {
            int numberOfCards = allCards.Count;
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

        private void GetLongestStraight(CardCounter valueCounter, out int straightLenght, out int straightStartsAt)
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
