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
                if(special == "Clockwise")
                {
                    for(int i = 0; i < game.)
                    Pile temp = game.PlayerHands[0];
                    game.PlayerHands[0] = game.PlayerHands[1];
                    game.PlayerHands[1] = game.PlayerHands[2];
                    game.PlayerHands[2] = game.PlayerHands[2];
                    game.PlayerHands[3] = game.PlayerHands[2];
                    game.PlayerHands[4] = game.PlayerHands[2];
                }
            }
            return false;
        }
    }
}
