import itertools
import pickle
from treys import Card, Evaluator

SUITS = ['h', 's', 'c', 'd']
VALS = [str(i) for i in range(6, 10)] + ['T', 'J', 'Q', 'K', 'A']
CARDS = [v + s for v in VALS for s in SUITS]
CARD_INTS = {c: Card.new(c) for c in CARDS}

evaluator = Evaluator()


SUIT_MAP = {
    'h': 'Heart',
    's': 'Spade',
    'c': 'Club',
    'd': 'Diamond'
}
VAL_MAP = {str(v): f'{v:02d}' for v in range(6, 10)} | {
    'A': '01',
    'T': '10',
    'J': '11',
    'Q': '12',
    'K': '13',
}

def to_other_card_rep(card):
    return f'{SUIT_MAP[card[1]]}{VAL_MAP[card[0]]}'

def hash_cards(cards):
    return ','.join(sorted(to_other_card_rep(card) for card in cards))

VAL_NAMES = {
    '6': ('Six', 'Sixes'),
    '7': ('Seven', 'Sevens'),
    '8': ('Eight', 'Eights'),
    '9': ('Nine', 'Nines'),
    'T': ('Ten', 'Tens'),
    'J': ('Jack', 'Jacks'),
    'Q': ('Queen', 'Queens'),
    'K': ('King', 'Kings'),
    'A': ('Ace', 'Aces'),
}

ORDER = list(VAL_NAMES.keys())

SUIT_NAMES = {
    'h': 'Heart',
    's': 'Spade',
    'c': 'Club',
    'd': 'Diamond',
}


def find_high_card(cards):
    vals = {c[0] for c in cards}
    for val in ORDER[::-1]:
        if val in vals:
            return f"{VAL_NAMES[val][0]} High"

def _find_highest(cards, omit=None, freq=2):
    counts = {}
    for c in cards:
        counts.setdefault(c[0], 0)
        counts[c[0]] += 1

    return max((v for v in counts if counts[v] == freq and v != omit), key=lambda v: ORDER.index(v))

def find_pair(cards):
    val = _find_highest(cards)
    return f'A Pair of {VAL_NAMES[val][1]}'

def find_two_pair(cards):
    first = _find_highest(cards)
    second = _find_highest(cards, omit=first)
    return f"Two Pair, {VAL_NAMES[first][1]} and {VAL_NAMES[second][1]}"

def find_three_of_a_kind(cards):
    val = _find_highest(cards, freq=3)
    return f'Three {VAL_NAMES[val][1]}'

def find_four_of_a_kind(cards):
    val = _find_highest(cards, freq=4)
    return f'Four {VAL_NAMES[val][1]}'

def find_straight(cards):
    vals = sorted({c[0] for c in cards}, key=lambda v: ORDER.index(v))
    start = vals[0]
    last = start
    for v in vals[1:]:
        if ORDER.index(v) == ORDER.index(last) + 1:
            last = v 
        else:
            start = v
            last = start

    return f"Straight, {VAL_NAMES[start][0]} to {VAL_NAMES[last][0]}"


DISPLAY_HAND_FUNCS = {
    "Straight": find_straight,
    "Three of a Kind": find_three_of_a_kind,
    "Four of a Kind": find_four_of_a_kind,
    "Two Pair": find_two_pair,
    "Pair": find_pair,
    "High Card": find_high_card,
}

def eval_cards(cards):
    card_ints = [CARD_INTS[c] for c in cards]
    s = evaluator.hand_size_map[len(card_ints)](card_ints)
    hand = evaluator.class_to_string(evaluator.get_rank_class(s))
    if hand in DISPLAY_HAND_FUNCS:
        hand = DISPLAY_HAND_FUNCS[hand](cards)
    return s, hand


def compute_hands():
    # goal: write <hash of 7/6/5 card hand> -> (score of best hand, display name of best hand)
    data = {}
    for i in [5, 6, 7]:
        for cards in itertools.combinations(CARDS, i):
            data[hash_cards(cards)] = eval_cards(cards) 
        print(f'{i = } done.')

    print(f'{len(data)} hands computed')

    return data

def save_data(data):
    with open('hands.pkl', 'wb') as f:
        pickle.dump(data, f)

def load_data():
    with open('hands.pkl', 'rb') as f:
        o = pickle.load(f)
    return o

def save_csv(data):
    with open('hands.csv', 'w') as f:
        for hand, (score, name) in data.items():
            f.write(f'{hand}\t{score}\t{name}\n')

def main():

    import sys
    if sys.argv[-1] == '-l':
        data = load_data()
    else:
        data = compute_hands()
        save_data(data) 

    print(len(data))

    save_csv(data)




if __name__ == '__main__':
    main()