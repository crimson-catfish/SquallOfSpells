import os
import cv2
import glob

RAW_RUNES_PATH = "Runes/Raw/*.bmp"
SQUARE_RUNES_PATH = "Runes/SquareRunes"

def square_image(image):
    width, height = image.shape[:2]
    square_dimension = min(width, height)
    center_square_x = int(width / 2 - square_dimension / 2)
    center_square_y = int(height / 2 - square_dimension / 2)
    return image[center_square_y:center_square_y + square_dimension,
                 center_square_x:center_square_x + square_dimension]

def save_images(images, path):
    for i, img in enumerate(images):
        img_path = os.path.join(path, f"square_rune_{i}.bmp")
        cv2.imwrite(img_path, img)



for image_path in glob.glob(RAW_RUNES_PATH):
    image = cv2.imread(image_path)
    cropped_image = square_image(image)
    save_images(cropped_image, SQUARE_RUNES_PATH)