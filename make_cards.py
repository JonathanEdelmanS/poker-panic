from PIL import Image, ImageOps, ImageDraw, ImageFont

def save(img, filename, ext='PNG'):
    img.save(filename, ext)

BORDER_WIDTH = 10
WIDTH = 800
HEIGHT = 1280
def card_base():
    base_img = Image.new("RGB", (WIDTH - 2 * BORDER_WIDTH, HEIGHT - 2 * BORDER_WIDTH), (255, 255, 255))
    base_img = ImageOps.expand(base_img, border=BORDER_WIDTH)
    return base_img

SUITS = ('clubs', 'diamonds', 'hearts', 'spades')
def load_suits():
    suits = [Image.open(f'card_pieces/{suit}.png') for suit in SUITS]
    for suit_img in suits:
        suit_img.load()

    return tuple(suits)


def main():
    suits = load_suits()

    # card_vals = ['A', 'J', 'Q', 'K'] + [_ for _ in range(6, 11)]
    card_vals = ['A']

    # font = ImageFont.truetype("arial.ttf", 15)
    fnt = ImageFont.truetype("Pillow/Tests/fonts/FreeMono.ttf", 40)

    # for suit in SUITS:
    for suit in ('spades',):
        for card_val in card_vals:
            img = card_base()
            img_draw = ImageDraw.Draw(img)

            img_draw.text((10, 10), card_val, font=fnt, fill=(0,0,0))

            save(img, f'cards/{card_val}-{suit}.png')

if __name__ == '__main__':
    main()