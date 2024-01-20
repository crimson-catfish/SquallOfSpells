from PIL import Image
import glob
import os

# The path where your PNG files are located
PNG_IMAGES_PATH = "m:/Documents/GameProjects/Rune/Assets/Runes/site/download*.png"

# Find all the PNG files in the specified path
png_files = glob.glob(PNG_IMAGES_PATH)

# Loop through each PNG file
for file in png_files:
    # Open the PNG file
    img = Image.open(file)
    
    # Convert the PNG file to BMP format
    bmp_img = img.convert('RGB')
    
    # Save the BMP file with the same name but with a '.bmp' extension
    bmp_filename = os.path.splitext(file)[0] + '.bmp'
    bmp_img.save(bmp_filename)

print('Conversion completed.')