import os
import sys
import numpy as np
from PIL import Image
try:
    from rembg import remove
except ImportError:
    print("pip install rembg pillow numpy")
    sys.exit(1)

def process_image(filepath):
    print(f"Processing {filepath}")
    with open(filepath, "rb") as i:
        input_data = i.read()
    output_data = remove(input_data)
    with open(filepath, "wb") as o:
        o.write(output_data)
    img = Image.open(filepath).convert("RGBA")
    data = np.array(img)
    r, g, b, a = data[:,:,0], data[:,:,1], data[:,:,2], data[:,:,3]
    magenta_mask = (r > 120) & (g < 100) & (b > 120) & (a > 0)
    data[magenta_mask, 3] = 0
    new_img = Image.fromarray(data)
    new_img.save(filepath)
    print(f"Successfully processed {filepath}")

if __name__ == "__main__":
    dir_path = sys.argv[1]
    for filename in os.listdir(dir_path):
        if filename.lower().endswith(".png"):
            filepath = os.path.join(dir_path, filename)
            try:
                process_image(filepath)
            except Exception as e:
                print(f"Error on {filename}: {e}")
