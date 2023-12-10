var lines = File.ReadAllLines("input.txt");
var hands = new List<Hand>();
foreach (var line in lines)
{
    var parts = line.Split(' ');
    var hand = new Hand(parts[0], int.Parse(parts[1]));
    hands.Add(hand);
}

hands.Sort();

var totalWinings = 0;
for (int i = 0; i < hands.Count; i++)
{
    totalWinings += hands[i].Bid * (i + 1);
}

Console.WriteLine(totalWinings);

class Hand : IComparable<Hand>
{
    char[] CardRanks = ['A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2'];

    private readonly string _cards;
    private readonly int _bid;

    private Dictionary<char, int> _cardCounts = new();

    public int Bid => _bid;

    public Hand(string cards, int bid)
    {
        _cards = cards;
        _bid = bid;

        foreach (var cardRank in CardRanks)
        {
            _cardCounts.Add(cardRank, 0);
        }

        for (int i = 0; i < _cards.Length; i++)
        {
            _cardCounts[_cards[i]]++;
        }
    }

    public int GetStrength()
    {
        // logic to calculate strength of the hand
        if (_cardCounts.Any(c => c.Value == 5))
        {
            // five of a kind
            return 7;
        }

        if (_cardCounts.Any(c => c.Value == 4))
        {
            // four of a kind
            return 6;
        }

        if (_cardCounts.Any(c => c.Value == 3) && _cardCounts.Any(c => c.Value == 2))
        {
            // full house
            return 5;
        }

        if (_cardCounts.Any(c => c.Value == 3))
        {
            // three of a kind
            return 4;
        }

        if (_cardCounts.Count(c => c.Value == 2) == 2)
        {
            // two pairs
            return 3;
        }

        if (_cardCounts.Any(c => c.Value == 2))
        {
            // one pair
            return 2;
        }

        // high card
        return 1;
    }

    public int CompareTo(Hand? other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        var strengthComparison = GetStrength().CompareTo(other.GetStrength());
        if (strengthComparison != 0)
        {
            return strengthComparison;
        }
        else
        {
            for (int i = 0; i < _cards.Length; i++)
            {
                var cardComparison = Array.IndexOf(CardRanks, other._cards[i])
                    .CompareTo(Array.IndexOf(CardRanks, _cards[i]));
                if (cardComparison != 0)
                {
                    return cardComparison;
                }
            }
        }

        return 0;
    }
}