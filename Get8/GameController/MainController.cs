using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Get8Backbone;

namespace GameController
{
    /// </summary>
    public class MainController
    {
        public GameState game;


        public bool PlayerAction(int playerTurn, string action, Pile pile, string special)
        {
            if (action == "Buy")
            {
                foreach (Card card in pile.GetCards())
                {
                    for (int i = 0; i < card.GetCost(); i++)
                    {
                        game.PlayerHands[playerTurn].Insert(game.Supply.Draw());
                    }
                }
            }
            else if (action == "Trade")
            {
                int totalValue = 0;
                foreach (Card card in pile.GetCards())
                {
                    totalValue += card.GetCost();
                }
                int totalDraws = totalValue / 4;
                for (int i = 0; i < totalDraws; i++)
                {
                    try
                    {
                        game.PlayerHands[playerTurn].Insert(game.Star.Draw());
                    }
                    catch { throw new ArgumentException("Not enough stars"); }
                }
            }
            else if (action == "Cycle")
            {
                if (special == "Clockwise")
                {
                    List<Card> starsToRemove;
                    List<Card> starsToAdd = new List<Card>();
                    for (int i = 0; i < game.NumPlayers - 1; i++)
                    {
                        starsToRemove = new List<Card>(); // get the current stars in this player's hand

                        foreach (Card card in game.PlayerHands[i].GetCards())
                        {
                            if (card.GetCost() > 3)
                            {
                                starsToRemove.Add(card);
                            }
                        } // we have all their stars
                        game.PlayerHands[i].RemoveAll(starsToRemove);
                        game.PlayerHands[i].InsertAll(starsToAdd);
                        starsToAdd = starsToRemove ;
                    }
                }
            }
            return false;
        }
    }
}
