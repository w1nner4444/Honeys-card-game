using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get8Backbone
{
    class GameState
    {
        Pile Supply;
        Pile Discard;
        Pile Star;
        List<Pile> PlayerHands;

        public GameState(int numPlayers)
        {
            List<Card> cards = GenerateDeck();
            Supply = new Pile(cards);
            Supply.Shuffle();
            for (int i = 0; i < numPlayers; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    PlayerHands[i].Insert(Supply.Draw());
                }
            }
            Discard = new Pile(new List<Card>());
            Star = new Pile(new List<Card>());
        }

        private List<Card> GenerateDeck()
        {
            List<Card> cards = new List<Card>();
            for (int i = 0; i < 10; i++)
            {
                cards.Add(new Card(cardType.black1));
                cards.Add(new Card(cardType.purple3));
                cards.Add(new Card(cardType.green2));
                cards.Add(new Card(cardType.yellow2));
            }
            for (int i = 0; i < 5; i++)
            {
                cards.Add(new Card(cardType.red4));
                cards.Add(new Card(cardType.pink4));
                cards.Add(new Card(cardType.brown4));
                cards.Add(new Card(cardType.orange4));
            }
            for (int i = 0; i < 4; i++)
            {
                cards.Add(new Card(cardType.steal));
            }
            for (int i = 0; i < 3; i++)
            {
                cards.Add(new Card(cardType.blue5));
                cards.Add(new Card(cardType.teal5));
                cards.Add(new Card(cardType.nope));
                cards.Add(new Card(cardType.swap));
                cards.Add(new Card(cardType.amplify));
            }
            for (int i = 0; i < 2; i++)
            {
                cards.Add(new Card(cardType.search));
                cards.Add(new Card(cardType.cycle));
                cards.Add(new Card(cardType.credit));
            }
            return cards;
        }
    }

    public class Pile
    {
        private List<Card> pile;
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
            pile.RemoveAt(pile.Count - 1);
            return card;
        }

        public void Insert(Card card)
        {
            pile.Add(card);
        }
    }

    public class Card
    {
        private cardType type;
        private int value;
        public Card(cardType type)
        {
            this.type = type;
            switch (type)
            {
                case cardType.black1:
                    value = 1;
                    break;
                case cardType.green2:
                case cardType.yellow2:
                    value = 2;
                    break;
                case cardType.purple3:
                    value = 3;
                    break;
                case cardType.orange4:
                case cardType.pink4:
                case cardType.red4:
                case cardType.brown4:
                    value = 4;
                    break;
            }

        }

        public int getCost()
        {
            return value;
        }
    }

    public enum cardType
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
