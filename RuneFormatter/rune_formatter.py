from PIL import Image, ImageFilter, ImageOps
import glob
import numpy as np
import os

RAW_RUNES_PATH = "Runes\\Raw\\*"
OUTPUT_BLURED_RUNES_PATH = "..\\Assets\\Resources\\Runes\\Blured"
OUTPUT_UNBLURED_RUNES_PATH = "..\\Assets\\Resources\\Runes\\Unblured"
TRESHHOLD = 128
TARGET_HEIGHT = 512
CROP_STEP = 2
BLUR_POWER = 15


def crop_image(image):
    white_positions = [(x,y) for x in range(0, image.size[0], CROP_STEP) for y in range(0, image.size[1], CROP_STEP) if image.getpixel((x, y)) == 255]
    rect = (min([x for x,y in white_positions]), min([y for x,y in white_positions]), max([x for x,y in white_positions]), max([y for x,y in white_positions]))
    return image.crop(rect)


raw_images = glob.glob(RAW_RUNES_PATH)

grayscale_images = list(map(lambda file: Image.open(file).convert("L"), raw_images))

binary_images = list(map(lambda image: image.point(lambda pixel: 255 if pixel < TRESHHOLD else 0), grayscale_images))

cropped_images = list(map(crop_image, binary_images))

resized_images = list(map(lambda image: ImageOps.contain(image, (int(TARGET_HEIGHT * 0.5), int(TARGET_HEIGHT * 0.5))), cropped_images))

bordered_images = list(map(lambda image: ImageOps.expand(image, int(TARGET_HEIGHT * 0.25), 'black'), resized_images))

for i, image in enumerate(bordered_images):
    image.save(OUTPUT_UNBLURED_RUNES_PATH + "\\unblured_rune_" + str(i) + ".bmp")

blurred_images = list(map(lambda image: image.filter(ImageFilter.GaussianBlur(BLUR_POWER)), bordered_images))

thin_images = list(map(lambda image: image.point(lambda pixel: pixel**1.05 if pixel**1.05 <= 255 else 255), blurred_images))


for i, image in enumerate(thin_images):
    image.save(OUTPUT_BLURED_RUNES_PATH + "\\blurred_rune_" + str(i) + ".bmp")
