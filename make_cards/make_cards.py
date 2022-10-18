from PIL import Image, ImageOps, ImageDraw, ImageFont
import os

CARDS_DIR = os.path.join('..', 'Assets', 'Resources')

def save(img, filename, ext='PNG'):
    img.save(filename, ext)

BORDER_WIDTH = 10
WIDTH = 800
HEIGHT = 1280
def card_base():
    base_img = Image.new("RGB", (WIDTH - 2 * BORDER_WIDTH, HEIGHT - 2 * BORDER_WIDTH), (255, 255, 255))
    base_img = ImageOps.expand(base_img, border=BORDER_WIDTH)
    return base_img

def make_suit_img(suit, scale=1): 
    suit_img = Image.open(f'{suit}.png').convert()
    sw, sh = suit_img.size
    return Image.blend(Image.new('RGBA', suit_img.size), suit_img, 1).resize((round(sw*scale), round(sh*scale)))

SUITS = ('Club', 'Diamond', 'Heart', 'Spade')
def load_suits():
    suits = {suit: make_suit_img(suit) for suit in SUITS}
    for suit_img in suits.values():
        suit_img.load()

    return suits


def main():
    suits = load_suits()

    card_vals = ['A', 'J', 'Q', 'K'] + [_ for _ in range(6, 11)]
    val_map = {'A': '01', 'J': '11', 'Q': '12', 'K': '13'} | {_: f'{_:02d}' for _ in range(6, 11)}

    font = ImageFont.truetype("Amagro.ttf", 500)

    for suit, suit_img in suits.items():
        for card_val in card_vals:
            img = card_base()
            img_draw = ImageDraw.Draw(img)

            img_draw.text((50, 100), str(card_val), font=font, fill=(0,0,0))

            mx, my = (-150, -50)
            img.paste(suit_img, (WIDTH//2+mx,HEIGHT//2+my))
                    
            save(img, os.path.join(CARDS_DIR, f'{suit}{val_map[card_val]}.png'))

if __name__ == '__main__':
    main()