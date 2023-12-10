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
    char[] CardRanks = ['A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'];

    private readonly string _cards;
    private readonly int _bid;

    private Dictionary<char, int> _cardCounts = new();

    public int Bid => _bid;

    public int JokerCount { get; private set; }

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
            if (_cards[i] == 'J')
            {
                JokerCount++;
            }
            else
            {
                _cardCounts[_cards[i]]++;
            }
        }
    }

    public int GetStrength()
    {
        var jokerCount = JokerCount;
        // logic to calculate strength of the hand
        if (_cardCounts.Any(c => c.Value == (5 - jokerCount)))
        {
            // five of a kind
            return 7;
        }

        if (_cardCounts.Any(c => c.Value >= (4 - jokerCount)))
        {
            // four of a kind
            return 6;
        }

        if ((_cardCounts.Any(c => c.Value == 3) && _cardCounts.Any(c => c.Value == 2)) ||
            (jokerCount == 1 && _cardCounts.Count(c => c.Value == 2) == 2) ||
            (jokerCount >= 2 && _cardCounts.Any(c => c.Value == 2)))
            // 0 jokers => classic full house
            // 1 joker => 2 pairs
            // 2 jokers => 1 pair
        {
            // full house
            return 5;
        }

        if (_cardCounts.Any(c => c.Value == (3 - jokerCount)))
        {
            // three of a kind
            return 4;
        }

        if (_cardCounts.Count(c => c.Value == 2) == 2 ||
            (jokerCount == 1 && _cardCounts.Count(c => c.Value == 2) >= 1) ||
            (jokerCount == 2))
        {
            // two pairs
            return 3;
        }

        if (_cardCounts.Any(c => c.Value == (2 - jokerCount)))
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

    public override string ToString() => _cards;
}