from PIL import Image, ImageOps, ImageDraw, ImageFont
import os

CARDS_DIR = os.path.join('..', 'Assets', 'Resources')

BORDER_WIDTH = 10
WIDTH = 1280
HEIGHT = 2048

def save(img, filename, ext='PNG'):
    b = Image.new('RGBA', (2048, 2048), (0,0,0,0))
    b.paste(img, (1024 - WIDTH//2, 0))
    b.save(filename, ext)

def card_base():
    base_img = Image.new("RGB", (WIDTH - 2 * BORDER_WIDTH, HEIGHT - 2 * BORDER_WIDTH), (255, 255, 255))
    base_img = ImageOps.expand(base_img, border=BORDER_WIDTH)
    return base_img

def make_suit_img(suit, scale=1.5): 
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

    font = ImageFont.truetype("Amagro.ttf", 800)

    for suit, suit_img in suits.items():
        for card_val in card_vals:
            img = card_base()
            img_draw = ImageDraw.Draw(img)

            img_draw.text((50, 100), str(card_val), font=font, fill=(0,0,0))

            mx, my = (-150, -50)
            if suit == 'Heart':
                mx = -175
            img.paste(suit_img, (WIDTH//2+mx,HEIGHT//2+my))
                    
            save(img, os.path.join(CARDS_DIR, f'{suit}{val_map[card_val]}.png'))

if __name__ == '__main__':
    main()