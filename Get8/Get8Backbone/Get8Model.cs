using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get8Backbone
{
    class Get8Model
    {
        Pile Draw;
        Pile Discard;
        Pile Star;
    }

    class Pile
    {
        List<Card> pile;
        public Pile(List<Card> cards)
        {
            pile = cards;
        }

        public void Shuffle()
        {
            int n = pile.Count;
            Random rand = new Random();
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                Card card = pile[k];
                pile[k] = pile[n];
                pile[n] = card;
            }
        }

        public Card Draw()
        {
            Card card = pile[pile.Count - 1];
            pile.RemoveAt(pile.Count-1);
            return card;
        }

        public void Insert(Card card)
        {
            pile.Add(card);
        }
    }

    class Card
    {
        enum cardType
        {
            black1,
            yellow2,
            green2,
            purple3,
            red4,
            orange4,
            brown4,
            pink4,
            teal5,
            blue5,
            steal,
            search,
            amplify,
            swap,
            cycle,
            credit,
            nope
        }
    }
}
