import cv2
import numpy as np
import os

def load_images(path):
    images = []
    for img in os.listdir(path):
        if img.endswith('.bmp'):
            img_path = os.path.join(path, img)
            img_array = cv2.imread(img_path)
            images.append(img_array)
    return images

def save_images(images, path):
    for i, img in enumerate(images):
        img_path = os.path.join(path, f"unblurred_rune_{i}.bmp")
        cv2.imwrite(img_path, img)

def blur_images(images):
    blurred_images = []
    for img in images:
        blurred_img = cv2.GaussianBlur(img, (91, 91), 0)
        blurred_images.append(blurred_img)
    return blurred_images

def resize_images(images, size):
    resized_images = []
    for img in images:
        resized_img = cv2.resize(img, size)
        resized_images.append(resized_img)
    return resized_images

def grayscale_images(images):
    gray_images = []
    for img in images:
        gray_img = cv2.bitwise_not(cv2.cvtColor(img, cv2.COLOR_BGR2GRAY))
        gray_images.append(gray_img)
    return gray_images

def grayscale_to_binary(images, threshold):
    binary_images = []
    for img in images:
        binary_img = np.where(img > threshold, 255, 0)
        binary_images.append(binary_img)
    return binary_images

def thin_images_down(images):
    thinned_images = []
    for img in images:
        thinned_image = np.vectorize(lambda pixel: thin_math(pixel))(img)
        thinned_images.append(thinned_image)
    return thinned_images

def thin_math(pixel):
    if pixel - 38 >= 0:
        return int((pixel - 38)**1.03)
    else:
        return int(0)


images = load_images("m:/Pictures/Runes/SquaredRunes")

resized_images = resize_images(images, (512, 512))

gray_images = grayscale_images(resized_images)

unblurred_images = grayscale_to_binary(gray_images, 128)

save_images(unblurred_images, "m:/Documents/GameProjects/Rune/Assets/Runes/UnbluredRunes")

#blurred_images = blur_images(gray_images)

#thin_images = thin_images_down(blurred_images)

#save_images(thin_images, "m:/Documents/GameProjects/Rune/Assets/Runes/ThinRunes")